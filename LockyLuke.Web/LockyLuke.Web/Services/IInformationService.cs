using LockyLuke.Web.Entities;
using SharedLibrary.Dtos.Information;
using System.Collections.Immutable;

namespace LockyLuke.Web.Services
{
    public interface IInformationService
    {
        Task<bool> AddInformationAsync(Information model,CancellationToken cancellationToken = default);

        Task<bool> DeleteInformationAsync(Guid id, CancellationToken cancellationToken = default);

        Task<IEnumerable<Information>> GetAllInformationByUserIdAsync(string userName, CancellationToken cancellationToken = default);

        Task<IEnumerable<Information>> GetAllInformationBySearchAsync(string userName, string searchKey, CancellationToken cancellationToken = default);

        Task<Information?> GetInformationByIdAsync(Guid id, CancellationToken cancellationToken = default);

        Task<bool> UpdateInformationAsync(InformationUpdateDto model, CancellationToken cancellationToken = default);

    }
}
