using ArdaManager.Application.Models.Chat;
using ArdaManager.Application.Requests.Approval;
using ArdaManager.Application.Responses.Approval;
using ArdaManager.Application.Responses.Identity;
using ArdaManager.Client.Extensions;
using ArdaManager.Client.Infrastructure.Managers.Approval;
using ArdaManager.Client.Infrastructure.Managers.Approval.Scenario;
using ArdaManager.Client.Infrastructure.Managers.Identity.Roles;
using ArdaManager.Client.Infrastructure.Managers.Identity.Users;
using ArdaManager.Client.Pages.Docs.PurchaseRequest;
using ArdaManager.Domain.Entities;
using ArdaManager.Domain.Enums.Doc;
using ArdaManager.Shared.Models;
using Microsoft.AspNetCore.Components;
using MudBlazor;
using Nextended.Core.Extensions;
using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;

namespace ArdaManager.Client.Shared.Cards
{
    public partial class DocApproveCard
    {
        [Parameter] public DocType DocumentType { get; set; }
        [Parameter] public int DocumentId { get; set; }


        [Inject] private IScenarioManager ScenarioManager { get; set; }
        [Inject] private IApproveManager ApproveManager { get; set; }
        [Inject] private IRoleManager RoleManager { get; set; }
        [Inject] private IUserManager UserManager { get; set; }

        private ApprovalScenarioResponse _scenario = new();
        private List<ApprovalScenarioResponse> _scenarios = new();
        private List<RoleResponse> _roles = new();
        private List<DocumentApprovalStatusResponse> _statuses = new();

        public List<UserRoleModel> UserRolesList = new();
        private bool ShowActionBar = false;
        ApproveRejectRequest approveRejectRequest = null;

        protected override async Task OnInitializedAsync()
        {
            await LoadDataAsync();
            //ShowActionBar = _statuses.Any(s => UserRolesList.Any(r => r.RoleName == s.UserRoleName));
        }


        private async Task SetCurrentUser()
        {

            var state = await _stateProvider.GetAuthenticationStateAsync();
            var user = state.User;

            var userId = user.GetUserId();
            var userName = user.GetFullName();
            

        }


        protected override async Task OnAfterRenderAsync(bool firstRender)
        {
            if (firstRender)
            {
                if (DocumentId == 0)
                {
                    await LoadScenario();
                    await LoadRoles();
                }
                else
                {
                    await LoadApprovalStatusesAsync();
                    await LoadUserRolesAsync();
                    await CheckUserHasPriglivies();
                }
                await Task.CompletedTask;
                //await Table.SetFocusAsync();
            }
        }
        private async Task LoadDataAsync()
        {
            
        }

        private async Task CheckUserHasPriglivies()
        {
            var state = await _stateProvider.GetAuthenticationStateAsync();
            var user = state.User;
            var userId = user.GetUserId();

            foreach (var item in _statuses)
            {
                // Onay tarihi boş ve reddedilmemişse işlem yap
                if (true)
                {
                    // Kullanıcı belirtilen roldeyse içeri gir
                    if (user.IsInRole(item.UserRoleName))
                    {
                        approveRejectRequest = new ApproveRejectRequest()
                        {
                            DocumentId = item.BaseDocId,
                            StepNumber = item.StepNumber,
                            UserId = userId
                        };
                        ShowActionBar = true;
                        StateHasChanged();
                        break;
                    }
                    else
                        approveRejectRequest = null;
                }
                /*
                // Onay tarihi boş ve reddedilmişse işlem yap
                else if (item.ApproveDate.HasValue && item.IsRejected)
                {
                    // Kullanıcı belirtilen roldeyse içeri gir
                    if (user.IsInRole(item.UserRoleName))
                    {
                        approveRejectRequest = new ApproveRejectRequest()
                        {
                            DocumentId = item.BaseDocId,
                            StepNumber = item.StepNumber,
                            UserId = userId
                        };
                        ShowActionBar = true;
                        StateHasChanged();
                        break;
                    }
                    approveRejectRequest = null;
                }*/
            }
            /*
            foreach (var item in _statuses)
            {
                if ((!item.ApproveDate.HasValue && item.IsRejected))
                {
                    if(user.IsInRole(item.UserRoleName))
                    {
                        approveRejectRequest = new ApproveRejectRequest() {
                            DocumentId = item.BaseDocId,
                            StepNumber = item.StepNumber,
                            UserId = userId
                        };
                        ShowActionBar = true;
                        StateHasChanged();
                        break;
                    }
                    approveRejectRequest = null;
                }
            }
            */
        }

        private async Task LoadUserRolesAsync() 
        {
            var state = await _stateProvider.GetAuthenticationStateAsync();
            var user = state.User;

            var userId = user.GetUserId();
            var userName = user.IsInRole("");


            var response = await UserManager.GetRolesAsync(userId);
            //UserRolesList = response.Data.UserRoles;

            if (response.Succeeded)
            {
                UserRolesList = response.Data.UserRoles;
            }
            else
            {
                foreach (var message in response.Messages)
                {
                    _snackBar.Add(message, Severity.Error);
                }
            }

        }



        private async Task LoadApprovalStatusesAsync()
        {
            var response = await ApproveManager.GetDocumentApprovalStatusAsync(DocumentId);
            if (response.Succeeded)
            {
                _statuses = response.Data.ToList();
            }
            else
            {
                foreach (var message in response.Messages)
                {
                    _snackBar.Add(message, Severity.Error);
                }
            }
        }

        private async Task LoadRoles()
        {
            var response = await RoleManager.GetRolesAsync();
            if (response.Succeeded)
            {
                _roles = response.Data.ToList();
            }
            else
            {
                foreach (var message in response.Messages)
                {
                    _snackBar.Add(message, Severity.Error);
                }
            }
        }
        private async Task LoadScenario()
        {
            var response = await ScenarioManager.GetAllScenariosAsync();
            if (response.Succeeded)
            {
                _scenarios = response.Data;
            }
            if (_scenarios != null)
            {
                _scenario = _scenarios.Where(x => x.DocType == DocumentType).FirstOrDefault();
            }

        }



        private async Task Approve() 
        {
            var response = await ApproveManager.ApproveStepAsync(approveRejectRequest);
            if (response.Succeeded)
            {
                _snackBar.Add("Onay Durumu değişti", Severity.Success);
                await LoadApprovalStatusesAsync();
            }
            else
            {
                foreach (var message in response.Messages)
                {
                    _snackBar.Add(message, Severity.Error);
                }
            }
        }

        private async Task Reject()
        {
            var response = await ApproveManager.RejectStepAsync(approveRejectRequest);
            if (response.Succeeded)
            {
                _snackBar.Add("Onay Durumu değişti", Severity.Success);
                await LoadApprovalStatusesAsync();
                StateHasChanged();
            }
            else
            {
                foreach (var message in response.Messages)
                {
                    _snackBar.Add(message, Severity.Error);
                }
            }
        }
    }
}
