using ArdaManager.Client.Extensions;
using Microsoft.AspNetCore.Components;
using System.Threading.Tasks;

namespace ArdaManager.Client.Shared.Components
{
    public partial class CurrentUsers
    {
        [Parameter] public string Class { get; set; }
        //[Inject] public UserSessionManagerService UserSessionManagerService  { get; set; }
        private string FirstName { get; set; }
        private string SecondName { get; set; }
        private string Email { get; set; }
        private char FirstLetterOfName { get; set; }

        [Parameter]
        public string ImageDataUrl { get; set; }

        protected override async Task OnInitializedAsync()
        {
            //UserSessionManagerService.
            await LoadDataAsync();
        }

        private async Task LoadDataAsync()
        {
            var state = await _stateProvider.GetAuthenticationStateAsync();
            var user = state.User;

            this.Email = user.GetEmail().Replace(".com", string.Empty);
            this.FirstName = user.GetFirstName();
            this.SecondName = user.GetLastName();
            if (this.FirstName.Length > 0)
            {
                FirstLetterOfName = FirstName[0];
            }
            var UserId = user.GetUserId();
            var imageResponse = await _accountManager.GetProfilePictureAsync(UserId);
            if (imageResponse.Succeeded)
            {
                ImageDataUrl = imageResponse.Data;
            }
        }
    }
}

