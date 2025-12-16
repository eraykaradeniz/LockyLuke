using LockyLuke.Web.DatabaseContext;
using LockyLuke.Web.Entities;
using Mapster;
using Microsoft.AspNetCore.Identity;
using Microsoft.EntityFrameworkCore;
using SharedLibrary.Dtos.Information;
using System.Reflection.Metadata;
namespace LockyLuke.Web.Services
{
    public sealed class InformationService(AppDbContext appContext) : IInformationService
    {
        public async Task<bool> AddInformationAsync(Information model, CancellationToken cancellationToken = default)
        {
            await appContext.Informations.AddAsync(model, cancellationToken);
            return await appContext.SaveChangesAsync(cancellationToken) > 0;
        }

        public async Task<bool> DeleteInformationAsync(Guid id, CancellationToken cancellationToken = default)
        {
            Information? item = await appContext.Informations.FirstOrDefaultAsync(x => x.Id == id);

            if (item is null) return false;

            appContext.Informations.Remove(item);
            return await appContext.SaveChangesAsync(cancellationToken) > 0;
        }

        public async Task<IEnumerable<Information>> GetAllInformationBySearchAsync(string userId, string searchKey, CancellationToken cancellationToken = default)
        {
            return await appContext.Informations
                .Where(i => i.Site.Contains(searchKey) || i.UserName.Contains(searchKey) || i.Description.Contains(searchKey))
                .ToListAsync(cancellationToken);
        }

        public async Task<IEnumerable<Information>> GetAllInformationByUserIdAsync(string userId, CancellationToken cancellationToken = default)
        {
            return await appContext.Informations.Where(x => x.UserId == userId).ToListAsync(cancellationToken);
        }

        public async Task<Information?> GetInformationByIdAsync(Guid id, CancellationToken cancellationToken = default)
        {
            return await appContext.Informations.FirstOrDefaultAsync(x => x.Id == id ,cancellationToken) ?? null;
        }

        public async Task<bool> UpdateInformationAsync(InformationUpdateDto model, CancellationToken cancellationToken = default)
        {
            
            Information information = appContext.Find<Information>(model.id)!;
            information.Site = model.Site;
            information.UserName = model.UserName;
            information.Password = model.Password;
            information.Description = model.Description ?? information.Description;
            information.LastUpdateDate = DateTime.UtcNow;
            appContext.Informations.Update(information);
            return await appContext.SaveChangesAsync(cancellationToken) > 0;
        }
    }
}
