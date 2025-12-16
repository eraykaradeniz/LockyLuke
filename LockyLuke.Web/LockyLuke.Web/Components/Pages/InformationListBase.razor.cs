using LockyLuke.Web.Services;
using LockyLuke.Web.Utils;
using Microsoft.AspNetCore.Components;
using Microsoft.AspNetCore.Components.Authorization;
using SharedLibrary.Dtos.Information;
using System.Security.Claims;

namespace LockyLuke.Web.Components.Pages
{
    public class InformationListBase : ComponentBase
    {
        [Inject]
        public HttpClient Client { get; set; }

        [CascadingParameter]
        public Task<AuthenticationState> AuthState { get; set; }
        [Inject]
        public ModalManager ModalManager { get; set; }
        [Inject] NavigationManager NavigationManager { get; set; }

        [Inject]
        ClipboardService ClipboardService { get; set; } 
        protected IEnumerable<InformationDetailDto> Informations { get; set; }

        protected InformationAddDto informationAddDto = new InformationAddDto();
        protected string Username { get; set; }
        protected string Password { get; set; }

        protected string searchValue { get; set; }
        override protected async Task OnInitializedAsync()
        {
            await LoadInformations();
        }

        protected async Task LoadInformations()
        {

            var serviceResponse = await Client.GetFromJsonAsync<List<InformationDetailDto>>("/information/user-informations");

            if (serviceResponse is not null)
            {
                Informations = serviceResponse;
            }
        }

        protected async Task DeleteInformation(string id)
        {
            try
            {
                bool confirmed = await ModalManager.ShowConfirmationAsync();
                if (confirmed)
                {
                    await Client.DeleteAsync($"/information/{id}");
                    await LoadInformations();
                }
                else { return; }


            }
            catch (Exception ex)
            {
                await ModalManager.ShowMessageAsync("Error", $"An error occurred while deleting the information: {ex.Message}");
            }

        }
        protected async Task AddInformation()
        {
            try
            {

                var authState = await AuthState;
                if (authState.User.Identity.IsAuthenticated)
                {
                    informationAddDto.UserId = authState.User.Claims.FirstOrDefault(x => x.Type == ClaimTypes.NameIdentifier)?.Value;
                }
                else
                {
                    NavigationManager.NavigateTo("/login");
                }
                informationAddDto.Description = "";

                await Client.PostAsJsonAsync("/information", informationAddDto);

                await LoadInformations();
            }
            catch (Exception ex)
            {
                await ModalManager.ShowMessageAsync("Error", $"An error occurred while adding the information: {ex.Message}");
            }
        }

        protected async Task GetInformationDetailbyId(string id,string type)
        {

           await ClipboardService.WriteTextAsync(await Client.GetFromJsonAsync<string>("/information/"+id+"&t="+type));

        }

        protected async Task GetInformationsbySearchKey()
        {
            if (!string.IsNullOrEmpty(searchValue))
            {
                if (searchValue.Length > 2)
                {
                   
                    var serviceResponse =  await Client.GetFromJsonAsync<List<InformationDetailDto>>("/information-search/" + searchValue);

                    if (serviceResponse is not null)
                    {
                        Informations = serviceResponse;
                    }
                }

            }
        }

    }


}
