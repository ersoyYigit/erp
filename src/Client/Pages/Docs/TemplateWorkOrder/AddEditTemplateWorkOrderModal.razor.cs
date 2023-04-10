using ArdaManager.Client.Extensions;
using ArdaManager.Shared.Constants.Application;
using Microsoft.AspNetCore.Components;
using Microsoft.AspNetCore.SignalR.Client;
using MudBlazor;
using System.Threading.Tasks;
using Blazored.FluentValidation;
using ArdaManager.Client.Infrastructure.Managers.Docs.TemplateWorkOrder;
using ArdaManager.Application.Features.Docs.TemplateWorkOrders.Commands.AddEdit;
using System.Linq;
using ArdaManager.Application.Responses.Identity;
using System.Collections.Generic;
using ArdaManager.Application.Features.Docs.TemplateWorkOrders.Queries.GetAll;
using ArdaManager.Application.Features.Docs.TemplateWorkOrders.Queries.GetById;
using ArdaManager.Application.Requests.Identity;
using AutoMapper;
using ArdaManager.Application.Requests.Catalog;
using ArdaManager.Client.Infrastructure.Managers.Catalog.Template;
using ArdaManager.Application.Features.Templates.Queries.GetAllPaged;
using ArdaManager.Client.Infrastructure.Managers.Inventory.ProductionLine;
using ArdaManager.Application.Features.ProductionLines.Queries.GetAll;
using System;
using ArdaManager.Application.Features.Molds.Queries.GetAllPaged;
using ArdaManager.Client.Infrastructure.Managers.Catalog.Mold;
using ArdaManager.Domain.Enums.Doc;
using ArdaManager.Domain.Entities.Addressing;
using System.Numerics;
using Radzen.Blazor;
using MudBlazor.Extensions;
using ArdaManager.Application.Interfaces.Chat;
using ArdaManager.Application.Models.Chat;
using ArdaManager.Client.Infrastructure.Managers.Communication;
using ArdaManager.Client.Shared.Components;
using System.Security.Cryptography;
using ArdaManager.Shared.Models;

namespace ArdaManager.Client.Pages.Docs.TemplateWorkOrder
{
    public partial class AddEditTemplateWorkOrderModal
    {

        private List<UserResponse> _userList = new();



        [Inject] private ITemplateWorkOrderManager TemplateWorkOrderManager { get; set; }
        [Inject] private ITemplateManager TemplateManager { get; set; }
        [Inject] private IMoldManager MoldManager { get; set; }
        [Inject] private IProductionLineManager ProductionLineManager { get; set; }
        [Inject] private IChatManager ChatManager { get; set; }
        private IMapper _mapper;
        [Parameter] public AddEditTemplateWorkOrderCommand AddEditTemplateWorkOrderModel { get; set; } = new();
        [Parameter] public int Id { get; set; }
        [CascadingParameter] private MudDialogInstance MudDialog { get; set; }
        [CascadingParameter] private HubConnection HubConnection { get; set; }

        private FluentValidationValidator _fluentValidationValidator;



        private GetTemplateWorkOrderByIdResponse _templateWorkOrder = new();


        private IEnumerable<GetAllPagedTemplatesResponse> _templatesData;
        private IEnumerable<GetAllPagedMoldsResponse> _moldsData;
        private IEnumerable<GetAllProductionLinesResponse> _productionLinesData;

        private string ownerUserId;
        private string ownerUserName;
        private bool isRework = false;
        //private string selectedProductionLineCode = AddEditTemplateWorkOrderModel.ProductionLine;



        private bool Validated => _fluentValidationValidator.Validate(options => { options.IncludeAllRuleSets(); });

        public void Cancel()
        {
            MudDialog.Cancel();
        }

        private async Task SaveAsync()
        {
            SetDocState();
            
            await NotifyIfStateChanged();
            /*
            var response = await TemplateWorkOrderManager.SaveAsync(AddEditTemplateWorkOrderModel);
            if (response.Succeeded)
            {
                _snackBar.Add(response.Messages[0], Severity.Success);
                MudDialog.Close();
            }
            else
            {
                foreach (var message in response.Messages)
                {
                    _snackBar.Add(message, Severity.Error);
                }
            }
            */
            await HubConnection.SendAsync(ApplicationConstants.SignalR.SendUpdateDashboard);
        }


        private async Task GetUsersAsync()
        {
            var response = await _userManager.GetAllAsync();
            if (response.Succeeded)
            {
                _userList = response.Data.ToList();
            }
            else
            {
                foreach (var message in response.Messages)
                {
                    _snackBar.Add(message, Severity.Error);
                }
            }
        }

        protected override async Task OnInitializedAsync()
        {
            await LoadDataAsync();

            if (AddEditTemplateWorkOrderModel.Id == 0)
            {
                overrideCurrentUser();
                BackupOldVals();
            }



            HubConnection = HubConnection.TryInitialize(_navigationManager);
            if (HubConnection.State == HubConnectionState.Disconnected)
            {
                await HubConnection.StartAsync();
            }
        }
        private bool waitTemplates = true;
        private async Task LoadDataAsync()
        {
            //await GetTemplateWorkOrder();
            await GetUsersAsync();
            await SetCurrentUser();
            await LoadTemplates();
            waitTemplates = false;
            StateHasChanged();
            if (AddEditTemplateWorkOrderModel.Id != 0)
                await CheckWorkType(AddEditTemplateWorkOrderModel.TemplateWorkOrderType);
        }

        private async Task SetCurrentUser()
        {

            var state = await _stateProvider.GetAuthenticationStateAsync();
            var user = state.User;
            ownerUserId = user.GetUserId();
            ownerUserName = _userList.FirstOrDefault(x => x.Id == ownerUserId).FullName;
            if (AddEditTemplateWorkOrderModel.Id == 0)
                overrideCurrentUser();

        }
        private void overrideCurrentUser()
        {
            AddEditTemplateWorkOrderModel.OwnerUserId = ownerUserId;
            AddEditTemplateWorkOrderModel.OwnerUserName = ownerUserName;

        }

        private void ResponUserChanged(IEnumerable<string> values)
        {
            var respId = values.FirstOrDefault();
            if (respId != null) {
                AddEditTemplateWorkOrderModel.ResponseUserName = _userList.FirstOrDefault(c => c.Id == respId)?.FullName;
            }
        }
        private async void WorkTypeChanged(IEnumerable<TemplateWorkOrderType> values)
        {
            var type = values.FirstOrDefault();
            await CheckWorkType(type);
            /*
            if (type == TemplateWorkOrderType.ReWork) {
                
                await LoadProductionLines();
                await LoadMolds();
                isRework = true;
            }
            else
                isRework = false;
            */
        }
        private bool waitRework = false;
        private async Task CheckWorkType(TemplateWorkOrderType type)
        {
            if (type == TemplateWorkOrderType.ReWork)
            {
                isRework = true;
                if (_productionLinesData == null)
                {
                    waitRework = true;
                    await LoadProductionLines();
                    await LoadMolds();
                    waitRework = false;
                    StateHasChanged();
                }
            }
            else
                isRework = false;
        }

        private async Task LoadTemplates()
        {


            var request = new GetAllPagedTemplatesRequest { PageSize = 10000, PageNumber = 1, SearchString = "", Orderby = null };
            var response = await TemplateManager.GetTemplatesAsync(request);
            if (response.Succeeded)
            {
                _templatesData = response.Data;
            }
            else
            {
                foreach (var message in response.Messages)
                {
                    _snackBar.Add(message, Severity.Error);
                }
            }
        }



        private async Task LoadProductionLines()
        {


            //var request = new GetAllPagedTemplatesRequest { PageSize = 10000, PageNumber = 1, SearchString = "", Orderby = null };
            var response = await ProductionLineManager.GetAllAsync();
            if (response.Succeeded)
            {
                _productionLinesData = response.Data;
            }
            else
            {
                foreach (var message in response.Messages)
                {
                    _snackBar.Add(message, Severity.Error);
                }
            }
        }
        private async Task LoadMolds()
        {


            var request = new GetAllMoldsRequest { PageSize = 10000, PageNumber = 1, SearchString = "", Orderby = null };
            var response = await MoldManager.GetMoldsAsync(request);
            if (response.Succeeded)
            {
                _moldsData = response.Data;
            }
            else
            {
                foreach (var message in response.Messages)
                {
                    _snackBar.Add(message, Severity.Error);
                }
            }
        }

        private string CombineName(int Id)
        {
            string retval = "";
            var obj = _templatesData.FirstOrDefault(x => x.Id == Id);
            if (obj != null)
                retval = obj.Code + " - " + obj.Name;

            return retval;
        }

        private void OnChangetmpDDL(object args)
        {
            int i = 0;
            int.TryParse(args.ToString(), out i);
            var place = _templatesData.FirstOrDefault(p => p.Id == i);
            _templatesData.FirstOrDefault(p => p.Id == i).Code = place.Code + " : " + place.Name;
            StateHasChanged();

        }


        private DateTime? backupPlannedStart;
        private DateTime? backupPlannedEnd;
        private DateTime? backupStart;
        private DateTime? backupEnd;
        private TemplateWorkOrderState backupDocState;
        private bool hasBackups = false;

        private void BackupOldVals()
        {
            hasBackups = true;
            backupPlannedStart = AddEditTemplateWorkOrderModel.PlannedWorkStartDate;
            backupPlannedEnd = AddEditTemplateWorkOrderModel.PlannedWorkEndDate;
            backupStart = AddEditTemplateWorkOrderModel.WorkStartDate;
            backupEnd = AddEditTemplateWorkOrderModel.WorkEndDate;
            backupDocState = AddEditTemplateWorkOrderModel.TemplateWorkOrderState;

        }
        private void SetDocState()
        {
            if (AddEditTemplateWorkOrderModel.TemplateWorkOrderState != TemplateWorkOrderState.Cancelled)
            {
                //AddEdit
                if (AddEditTemplateWorkOrderModel.WorkEndDate != null) {
                    AddEditTemplateWorkOrderModel.TemplateWorkOrderState = TemplateWorkOrderState.Done;
                }
                else if (AddEditTemplateWorkOrderModel.WorkStartDate != null) {
                    AddEditTemplateWorkOrderModel.TemplateWorkOrderState = TemplateWorkOrderState.Working;
                }
                else if (AddEditTemplateWorkOrderModel.PlannedWorkEndDate != null || AddEditTemplateWorkOrderModel.PlannedWorkStartDate != null) {
                    AddEditTemplateWorkOrderModel.TemplateWorkOrderState = TemplateWorkOrderState.Planned;
                }
                else {
                    AddEditTemplateWorkOrderModel.TemplateWorkOrderState = TemplateWorkOrderState.Waiting;
                }
            }
            Console.WriteLine("NEW STATE " + AddEditTemplateWorkOrderModel.TemplateWorkOrderState.ToDescriptionString());

        }
        MudDatePicker _pickerPS;
        MudDatePicker _pickerPE;
        MudDatePicker _pickerS;
        MudDatePicker _pickerE;
        void TextChanged(string text)
        {
            Console.WriteLine("changed text : " + text);
            StateHasChanged();
            SetDocState();
        }

        async Task NotifyIfStateChanged()
        {
            if (AddEditTemplateWorkOrderModel.Id == 0) {
                Console.WriteLine("NOTIFY  RESPONSIBLE");
                await NotifyResponsibleUser();
            }
            else {
                if (AddEditTemplateWorkOrderModel.TemplateWorkOrderState != backupDocState)
                {
                    if (AddEditTemplateWorkOrderModel.TemplateWorkOrderState == TemplateWorkOrderState.Waiting)
                    {
                        Console.WriteLine("NOTIFY  RESPONSIBLE");
                        await NotifyResponsibleUser();
                    }
                    else
                    {
                        Console.WriteLine("NOTIFY  OWNER");
                        await NotifyOwnerUser();
                    }
                }
            }
        }

        async Task NotifyResponsibleUser()
        {
            //Save Message to DB
            var chatHistory = new ChatHistory<IChatUser>
            {
                Message = "Adınıza yeni bir Kalıp İş Emri atanmıştır. Evrak No: {0}" + AddEditTemplateWorkOrderModel.DocNo,
                ToUserId = AddEditTemplateWorkOrderModel.ResponseUserId,
                CreatedDate = DateTime.Now,
                DocType = DocType.TemplateWorkOrder,
                FromUserName = AddEditTemplateWorkOrderModel.OwnerUserName,
                FromUserId = AddEditTemplateWorkOrderModel.OwnerUserId,
                //FromUser = (IChatUser)_userList.First(x=>x.Id == AddEditTemplateWorkOrderModel.OwnerUserId),
                IsRead = false,
                MessageType = Domain.Enums.MessageType.Notification,
                ToUserName = AddEditTemplateWorkOrderModel.ResponseUserName,
                RelatedLink = (AddEditTemplateWorkOrderModel.Id != 0) ? "/catalog/templateworkorders/" : "/catalog/templateworkorders/" + AddEditTemplateWorkOrderModel.Id.ToString()
            };
            var state = await _stateProvider.GetAuthenticationStateAsync();
            var user = state.User;
            if (chatHistory.ToUserId != user.GetUserId())
            {

                var response = await ChatManager.SaveMessageAsync(chatHistory);
                if (true)
                {
                    //var state = await _stateProvider.GetAuthenticationStateAsync();
                    //var user = state.User;
                    var CurrentUserId = AddEditTemplateWorkOrderModel.OwnerUserId;
                    chatHistory.FromUserId = CurrentUserId;
                    var userName = AddEditTemplateWorkOrderModel.OwnerUserName;// $"{user.GetFirstName()} {user.GetLastName()}";
                                                                               //await HubConnection.SendAsync(ApplicationConstants.SignalR.SendDocStateNotification, chatHistory, userName);
                    await HubConnection.SendAsync(ApplicationConstants.SignalR.SendChatNotification, string.Format("Adınıza {0} tarafından yeni bir Kalıp İş Emri atanmıştır. Evrak No: {1}", AddEditTemplateWorkOrderModel.OwnerUserName, AddEditTemplateWorkOrderModel.DocNo), AddEditTemplateWorkOrderModel.ResponseUserId, AddEditTemplateWorkOrderModel.OwnerUserId);
                    //CurrentMessage = string.Empty;
                }
                else
                {
                    /*
                    foreach (var message in response.Messages)
                    {
                        _snackBar.Add(message, Severity.Error);
                    }
                    */
                }
            }
        }


        async Task NotifyOwnerUser()
        {
            //Save Message to DB
            var chatHistory = new ChatHistory<IChatUser>
            {
                Message = "Atadığınız Kalıp İş Emri durumu güncellenmiştir. Evrak No:" + AddEditTemplateWorkOrderModel.DocNo,
                ToUserId = AddEditTemplateWorkOrderModel.ResponseUserId,
                CreatedDate = DateTime.Now,
                DocType = DocType.TemplateWorkOrder,
                FromUserName = AddEditTemplateWorkOrderModel.OwnerUserName,
                FromUserId = AddEditTemplateWorkOrderModel.OwnerUserId,
                //FromUser = (IChatUser)_userList.First(x=>x.Id == AddEditTemplateWorkOrderModel.OwnerUserId),
                IsRead = false,
                MessageType = Domain.Enums.MessageType.Notification,
                ToUserName = AddEditTemplateWorkOrderModel.ResponseUserName,
                RelatedDocId = AddEditTemplateWorkOrderModel.Id,
                RelatedLink = "/catalog/templateworkorders/" + AddEditTemplateWorkOrderModel.Id.ToString()
            };
            var state = await _stateProvider.GetAuthenticationStateAsync();
            var user = state.User;
            //ownerUserId = user.GetUserId();
            if (chatHistory.ToUserId != user.GetUserId())
            {

                var response = await ChatManager.SaveMessageAsync(chatHistory);
                if (response.Succeeded)
                //if (true)
                {
                    //var state = await _stateProvider.GetAuthenticationStateAsync();
                    //var user = state.User;
                    var CurrentUserId = AddEditTemplateWorkOrderModel.OwnerUserId;
                    chatHistory.FromUserId = CurrentUserId;
                    var userName = AddEditTemplateWorkOrderModel.OwnerUserName;// $"{user.GetFirstName()} {user.GetLastName()}";
                    //await HubConnection.SendAsync(ApplicationConstants.SignalR.SendDocStateNotification, chatHistory, userName);
                    await HubConnection.SendAsync(ApplicationConstants.SignalR.SendChatNotification, string.Format("Atadığınız Kalıp İş Emri durumu güncellenmiştir. Evrak No: {0}", AddEditTemplateWorkOrderModel.DocNo), AddEditTemplateWorkOrderModel.OwnerUserId, AddEditTemplateWorkOrderModel.ResponseUserId);
                    //CurrentMessage = string.Empty;
                }
                else
                {
                    /*
                    foreach (var message in response.Messages)
                    {
                        _snackBar.Add(message, Severity.Error);
                    }
                    */
                }
            }
        }

    }
}