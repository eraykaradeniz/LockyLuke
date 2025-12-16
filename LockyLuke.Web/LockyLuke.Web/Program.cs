using Blazored.LocalStorage;
using Blazored.Modal;
using LockyLuke.Web.Components;
using LockyLuke.Web.Configuration;
using LockyLuke.Web.DatabaseContext;
using LockyLuke.Web.Entities;
using LockyLuke.Web.Services;
using LockyLuke.Web.Utils;
using Mapster;
using Microsoft.AspNetCore.Authorization;
using Microsoft.AspNetCore.Components.Authorization;
using Microsoft.AspNetCore.DataProtection;
using Microsoft.AspNetCore.DataProtection.AuthenticatedEncryption;
using Microsoft.AspNetCore.DataProtection.AuthenticatedEncryption.ConfigurationModel;
using Microsoft.EntityFrameworkCore;
using SharedLibrary.Configuration;
using SharedLibrary.Dtos.Auth;
using SharedLibrary.Dtos.Information;
using SharedLibrary.Dtos.User;
using SharedLibrary.Extension;
using System.Reflection;
using System.Text.Json;

var builder = WebApplication.CreateBuilder(args);
builder.Services.AddServerSideBlazor().AddCircuitOptions(options => { options.DetailedErrors = true; });

builder.Services.AddEndpointsApiExplorer();
builder.Services.AddSwaggerGen();
// Add services to the container.
builder.Services.AddRazorComponents()
    .AddInteractiveWebAssemblyComponents()
    .AddInteractiveServerComponents(); 
builder.Services.AddAuthorization();
builder.Services.AddBlazoredLocalStorage();
builder.Services.AddBlazoredModal();
builder.Services.AddScoped<IInformationService, InformationService>();
builder.Services.AddScoped<IChiperService, ChiperService>();
builder.Services.AddScoped<ModalManager>();

builder.Services.AddScoped<ClipboardService>();
builder.Services.AddScoped(sp => new HttpClient { BaseAddress = new Uri(builder.Configuration["BaseAddress"].ToString()) });
//builder.Services.AddScoped<AuthenticationStateProvider, AuthStateProvider>();
builder.Services.Configure<CustomTokenOption>(builder.Configuration.GetSection("TokenOptions"));
var tokenOptions = builder.Configuration.GetSection("TokenOptions").Get<CustomTokenOption>();
builder.Services.AddCustomTokenAuth(tokenOptions);


builder.Services.AddDataProtection()
       .UseCryptographicAlgorithms(new AuthenticatedEncryptorConfiguration
       {
           EncryptionAlgorithm = EncryptionAlgorithm.AES_256_GCM,
           ValidationAlgorithm = ValidationAlgorithm.HMACSHA256
       });
builder.Services.AddDbContext<AppDbContext>(options =>
    options.UseSqlServer(builder.Configuration.GetConnectionString("DbConnection"), opt =>
    {
        opt.MigrationsAssembly(Assembly.GetExecutingAssembly().GetName().Name);

    }));
var MyAllowSpecificOrigins = "_myAllowSpecificOrigins";
builder.Services.AddCors(options =>
{
    options.AddPolicy(name: MyAllowSpecificOrigins,
                      policy =>
                      {
                          policy.AllowAnyOrigin().AllowAnyHeader().AllowAnyMethod(); ;
                      });
});
builder.Services.AddAuthorizationCore();
builder.Services.AddScoped<AuthenticationStateProvider, AuthStateProvider>();

MapsterConfig.Configure();
var app = builder.Build();

// Configure the HTTP request pipeline.
if (app.Environment.IsDevelopment())
{
    app.UseWebAssemblyDebugging();
}
else
{
    app.UseExceptionHandler("/Error", createScopeForErrors: true);
    // The default HSTS value is 30 days. You may want to change this for production scenarios, see https://aka.ms/aspnetcore-hsts.
    app.UseHsts();
}

app.UseHttpsRedirection();

app.MapPost("login", async (UserLoginDto userLoginDto, CancellationToken cancellationToken) =>
{
    var client = new HttpClient();
    var request = new HttpRequestMessage(HttpMethod.Post, builder.Configuration.GetSection("AuthUrl").Value + "/api/Auth/CreateToken");
    var content = new StringContent(JsonSerializer.Serialize(userLoginDto), null, "application/json");
    request.Content = content;
    var response = await client.SendAsync(request);
    response.EnsureSuccessStatusCode();
    string token = await response.Content.ReadAsStringAsync(cancellationToken);

    LoginResponseDto tokenDto = JsonSerializer.Deserialize<LoginResponseDto>(token)!;
    return tokenDto.data is null ? Results.BadRequest("Something went wrong!!") : Results.Ok(tokenDto);
}).WithTags("Information");


//information add
app.MapPost("information", [Authorize] async (InformationAddDto model, IInformationService service, IChiperService chiperService, CancellationToken cancellationToken) =>
{
    model.Password = chiperService.Encrypt(model.Password);
    model.UserName = chiperService.Encrypt(model.UserName);
    var result = await service.AddInformationAsync(model.Adapt<Information>(), cancellationToken);
    return result ? Results.Ok(new { Message = "Information is created with successfully." }) : Results.BadRequest("Something went wrong!!");
}).WithTags("Information");

//get user informations
app.MapGet("information/user-informations", [Authorize] async (HttpContext context, IInformationService service, CancellationToken cancellationToken) =>
{
    return context is not null ? Results.Ok((await service.GetAllInformationByUserIdAsync(context.User.Claims.First().Value, cancellationToken)).Adapt<IEnumerable<InformationDetailDto>>()) : Results.BadRequest("Login Sayfasýna yönlendiriliyorsunuz.");

}).WithTags("Information");

//get information by id
app.MapGet("information/{id}", [Authorize] async (Guid id, IInformationService service, IChiperService chiperService, CancellationToken cancellationToken) =>
{
    //informationDetail.Password = chiperService.Decrypt(informationDetail.Password);
    return Results.Ok((await service.GetInformationByIdAsync(id, cancellationToken)).Adapt<InformationUpdateDto>());
}).WithTags("Information");

app.MapGet("information/{id}&t={type}", [Authorize] async (Guid id,string type, HttpContext context, IInformationService service, IChiperService chiperService, CancellationToken cancellationToken) =>
{
    if (type == "u")
    {
        return Results.Ok(chiperService.Decrypt((await service.GetInformationByIdAsync(id, cancellationToken)).Adapt<InformationDetailDto>().UserName));
    }
    else if (type == "p")
    {
        return Results.Ok(chiperService.Decrypt((await service.GetInformationByIdAsync(id, cancellationToken)).Adapt<InformationDetailDto>().Password));
    }
    else
    {
        return Results.Ok("boþ geldi;");
    }

}).WithTags("Information");

//get information by search key
app.MapGet("information-search/{searchKey}", [Authorize] async (string searchKey, HttpContext context, IInformationService service, CancellationToken cancellationToken) =>
{
    return context.User is not null ? Results.Ok((await service.GetAllInformationBySearchAsync(context.User.Claims.First().Value, searchKey, cancellationToken)).Adapt<IEnumerable<InformationDetailDto>>()) : Results.BadRequest("Login Sayfasýna yönlendiriliyorsunuz.");

}).WithTags("Information");

//delete information
app.MapDelete("information/{id}", [Authorize] async (Guid id, IInformationService service, CancellationToken cancellationToken) =>
{
    var result = await service.DeleteInformationAsync(id, cancellationToken);
    if (!result) return Results.BadRequest("Something went wrong!!");
    return Results.Ok(new { Message = "Information deleted with successfully" });
}).WithTags("Information");

//update information
app.MapPut("information", [Authorize] async (InformationUpdateDto model, IInformationService service, IChiperService chiperService, CancellationToken cancellationToken) =>
{
    model.Password = chiperService.Encrypt(model.Password);
    model.UserName = chiperService.Encrypt(model.UserName);

    var result = await service.UpdateInformationAsync(model, cancellationToken);
    if (!result) return Results.BadRequest("Something went wrong!!");

    return Results.Ok(new { Message = "Information updated with successfully" });

}).WithTags("Information");

app.UseAuthentication();
app.UseAuthorization();
app.UseAntiforgery();

//if (app.Environment.IsDevelopment())
//{
//app.MapOpenApi();
app.UseSwagger();
app.UseSwaggerUI();


//}


app.MapStaticAssets();
app.MapRazorComponents<App>()
    .AddInteractiveWebAssemblyRenderMode()
    .AddInteractiveServerRenderMode()
    .AddAdditionalAssemblies(typeof(LockyLuke.Web.Client._Imports).Assembly);

app.Run();
