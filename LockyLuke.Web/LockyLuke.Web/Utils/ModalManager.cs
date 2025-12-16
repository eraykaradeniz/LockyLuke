using Blazored.Modal;
using Blazored.Modal.Services;
using LockyLuke.Web.Components.Shared;
using LockyLuke.Web.CustomComponents.Modals;

namespace LockyLuke.Web.Utils
{
    public class ModalManager
    {

        public IModalService modalService { get; }
        public ModalManager(IModalService ModalService)
        {
            modalService = ModalService;
        }

        public async Task ShowMessageAsync(string title,string message, int Duration = 0)
        {
            ModalParameters mParams = new ModalParameters();
            mParams.Add("Message", message);
            var modalRef =  modalService.Show<ShowMessagePopupComponent>(title, mParams);
            if (Duration > 0)
            {
                await Task.Delay(Duration);
                modalRef.Close();
            }
        }

        public async Task<bool> ShowConfirmationAsync()
        {
            var modalRef = modalService.Show<Confirm>();
            var result = await modalRef.Result;
            return !result.Cancelled;
        }
    }
}
