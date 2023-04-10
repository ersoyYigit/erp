using ArdaManager.Application.Responses.Approval;
using ArdaManager.Client.Extensions;
using ArdaManager.Client.Infrastructure.Managers.Approval;
using Microsoft.AspNetCore.Components;
using System.Collections.Generic;
using System.Threading.Tasks;

namespace ArdaManager.Client.Pages.Content
{
    public partial class Home
    {

        [Inject] private IApproveManager ApproveManager { get; set; }

        public string UserId { get; set; }

        private string amirRole = "c328cd14-3bf4-440b-9e4a-39d856a36516";
        private string amirRoleName = "Departman 1 Amiri";
        private List<DocumentApprovalStatusResponse> _amirStatuses = new();
        private bool _showAmir = false;

        private string mudurRole = "10a74eae-5320-450d-8663-6d10b2a5d8ce";
        private string mudurRoleName = "Satın Alma Müdürü";
        private List<DocumentApprovalStatusResponse> _mudurStatuses = new();
        private bool _showMudur = false;

        protected override async Task OnInitializedAsync()
        {
            await LoadDataAsync();
        }

        private async Task LoadDataAsync()
        {
            var state = await _stateProvider.GetAuthenticationStateAsync();
            var user = state.User;
            

            UserId = user.GetUserId();

            if(user.IsInRole(amirRoleName))
                await LoadAmirStatuses();


            if (user.IsInRole(mudurRoleName))
                await LoadMudurStatuses();


        }
        async Task TalepAc(int id)
        {
            var path = $"/purchase/requests/{id}";
            _navigationManager.NavigateTo(path);
        }
        private async Task LoadMudurStatuses()
        {
            _showMudur = true;
            var mudurResponse = await ApproveManager.GetWaitingStatusesByRoleAsync(mudurRole);
            //UserRolesList = response.Data.UserRoles;

            if (mudurResponse.Succeeded)
            {
                _mudurStatuses = mudurResponse.Data;
            }
        }

        private async Task LoadAmirStatuses()
        {
            _showAmir = true;
            var amirResponse = await ApproveManager.GetWaitingStatusesByRoleAsync(amirRole);
            //UserRolesList = response.Data.UserRoles;

            if (amirResponse.Succeeded)
            {
                _amirStatuses = amirResponse.Data;
            }

        }


    }
}
