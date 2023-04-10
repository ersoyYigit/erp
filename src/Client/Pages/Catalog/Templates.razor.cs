using ArdaManager.Application.Features.Templates.Queries.GetAllPaged;
using ArdaManager.Application.Features.Templates.Commands.AddEdit;
using ArdaManager.Application.Requests.Catalog;
using ArdaManager.Client.Extensions;
using ArdaManager.Client.Infrastructure.Managers.Catalog.Template;
using ArdaManager.Shared.Constants.Application;
using ArdaManager.Shared.Constants.Permission;
using Microsoft.AspNetCore.Components;
using Microsoft.AspNetCore.SignalR.Client;
using Microsoft.JSInterop;
using MudBlazor;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Security.Claims;
using System.Threading.Tasks;
using Microsoft.AspNetCore.Authorization;
using MudBlazor.Extensions.Options;
using MudBlazor.Extensions;
using MudBlazor.Extensions.Options;
using BlazorContextMenu;

namespace ArdaManager.Client.Pages.Catalog
{
    public partial class Templates 
    {
        [Inject] private ITemplateManager TemplateManager { get; set; }
        
        [CascadingParameter] private HubConnection HubConnection { get; set; }

        private IEnumerable<GetAllPagedTemplatesResponse> _pagedData;
        private MudTable<GetAllPagedTemplatesResponse> _table;
        private int _totalItems;
        private int _currentPage;
        private string _searchString = "";

        private ClaimsPrincipal _currentUser;
        private bool _canCreateTemplates;
        private bool _canEditTemplates;
        private bool _canDeleteTemplates;
        private bool _canExportTemplates;
        private bool _canSearchTemplates;
        private bool _loaded = false;

        private IJSRuntime JsRuntime;

        protected override async Task OnInitializedAsync()
        {
            _currentUser = await _authenticationManager.CurrentUser();
            _canCreateTemplates = (await _authorizationService.AuthorizeAsync(_currentUser, Permissions.Templates.Create)).Succeeded;
            _canEditTemplates = (await _authorizationService.AuthorizeAsync(_currentUser, Permissions.Templates.Edit)).Succeeded;
            _canDeleteTemplates = (await _authorizationService.AuthorizeAsync(_currentUser, Permissions.Templates.Delete)).Succeeded;
            _canExportTemplates = (await _authorizationService.AuthorizeAsync(_currentUser, Permissions.Templates.Export)).Succeeded;
            _canSearchTemplates = (await _authorizationService.AuthorizeAsync(_currentUser, Permissions.Templates.Search)).Succeeded;

            
            HubConnection = HubConnection.TryInitialize(_navigationManager);
            if (HubConnection.State == HubConnectionState.Disconnected)
            {
                await HubConnection.StartAsync();
            }

            Console.WriteLine("debug flag");
            _loaded = true;


        }

        private async Task<TableData<GetAllPagedTemplatesResponse>> ServerReload(TableState state)
        {
            if (!string.IsNullOrWhiteSpace(_searchString))
            {
                state.Page = 0;
            }
            await LoadData(state.Page, state.PageSize, state);
            return new TableData<GetAllPagedTemplatesResponse> { TotalItems = _totalItems, Items = _pagedData };

        }

        private async Task LoadData(int pageNumber, int pageSize, TableState state)
        {
            string[] orderings = null;
            if (!string.IsNullOrEmpty(state.SortLabel))
            {
                orderings = state.SortDirection != SortDirection.None ? new[] { $"{state.SortLabel}", $"{state.SortDirection}" } : new[] { $"{state.SortLabel}", $"Ascending" };
            }

            var request = new GetAllPagedTemplatesRequest { PageSize = pageSize, PageNumber = pageNumber + 1, SearchString = _searchString,  Orderby = orderings };
            var response = await TemplateManager.GetTemplatesAsync(request);
            if (response.Succeeded)
            {
                _totalItems = response.TotalCount;
                _currentPage = response.CurrentPage;
                _pagedData = response.Data;

            }
            else
            {

            
                foreach (var message in response.Messages)
                {
                    _snackBar.Add(message, Severity.Error);
                }
            }
        }

        private void OnSearch(string text)
        {
            _searchString = text;
            _table.ReloadServerData();
            StateHasChanged();
        }

        

        private async Task InvokeModal(int id = 0)
        {
            var parameters = new DialogParameters();
            if (id != 0)
            {
                var template = _pagedData.FirstOrDefault(c => c.Id == id);

                parameters.Add(nameof(AddEditTemplateModal.AddEditTemplateModel), new AddEditTemplateCommand
                {
                    Id = template.Id,
                    Name = template.Name,
                    Description = template.Description,
                    Rate = template.Rate,
                    PatternId = template.PatternId ?? 0,
                    CategoryId = template.CategoryId,
                    Code = template.Code,
                    BoolProperty1 = template.BoolProperty1,
                    ProductType = template.Type,
                    Kind = template.TemplateKind,
                    LastProductionDate = template.LastProductionDate,
                    MeasurementUnitId = template.MeasurementUnitId,
                    TemplateId = template.TemplateId,
                    TemplateState = template.TemplateState,
                    //ImageDataURL = template.ImageDataURL,

                    String1 = template.String1,

                    Diameter = template.Diameter,
                    Height = template.Height,
                    Weight = template.Weight,
                    Width = template.Width,
                    Length = template.Length,

                    DiameterTolerance = template.DiameterTolerance,
                    HeightTolerance = template.HeightTolerance,
                    WeightTolerance = template.WeightTolerance,
                    WidthTolerance = template.WidthTolerance,
                    LengthTolerance = template.LengthTolerance,

                    DiameterMUId = template.DiameterMUId ?? 0,
                    HeightMUId = template.HeightMUId ?? 0,
                    WeightMUId = template.WeightMUId ?? 0,
                    WidthMUId = template.WidthMUId ?? 0,
                    LengthMUId = template.LengthMUId ?? 0,


                    TemplatePatternId = template.PatternId,
                    ProductionDate = template.ProductionDate,
                    TemplateKind = template.TemplateKind,
                    State = template.TemplateState,
                    IsAlabastr = template.IsAlabastr,
                    RevisionDate = template.RevisionDate,
                    RevisionRequester = template.RevisionRequester,
                    RevisionRequesterDepartment = template.RevisionRequesterDepartment,
                    FaultDate = template.FaultDate,
                    FixDate = template.FixDate,
                    FaultCause = template.FaultCause,
                    FaultFixer = template.FaultFixer,
                    CancelCause = template.CancelCause,
                    Canceller = template.Canceller,
                    CancelationDate = template.CancelationDate,
                    Guid = template.Guid
                });
            }
            else
            {
                parameters.Add(nameof(AddEditTemplateModal.AddEditTemplateModel), new AddEditTemplateCommand
                {
                    Guid = Guid.NewGuid(),
                    MeasurementUnitId = 1

                });
            }

            //var options = new MudBlazor.DialogOptions { CloseButton = true, MaxWidth = MaxWidth.Medium, FullWidth = true, DisableBackdropClick = true  };
            var options = new DialogOptionsEx
            {
                CloseButton = true,
                MaxWidth = MaxWidth.Medium,
                FullWidth = false,
                DisableBackdropClick = true,
                FullHeight = false,
                
                DragMode = MudDialogDragMode.WithoutBounds,
                Animations = new[] { AnimationType.Pulse },
                Position = DialogPosition.Center,
                Resizeable = true
            };
            var dialog = await _dialogService.ShowEx<AddEditTemplateModal>(id == 0 ? _localizer["Create"] : _localizer["Edit"], parameters, options);
            var result = await dialog.Result;
            if (!result.Cancelled)
            {
                OnSearch("");
            }









        }
        private async Task Delete(int id)
        {
            string deleteContent = _localizer["Delete Content"];
            var parameters = new DialogParameters
            {
                {nameof(Shared.Dialogs.DeleteConfirmation.ContentText), string.Format(deleteContent, id)}
            };
            var options = new MudBlazor.DialogOptions { CloseButton = true, MaxWidth = MaxWidth.Small, FullWidth = true, DisableBackdropClick = true };
            var dialog = _dialogService.Show<Shared.Dialogs.DeleteConfirmation>(_localizer["Delete"], parameters, options);
            var result = await dialog.Result;
            if (!result.Cancelled)
            {
                var response = await TemplateManager.DeleteAsync(id);
                if (response.Succeeded)
                {
                    OnSearch("");
                    await HubConnection.SendAsync(ApplicationConstants.SignalR.SendUpdateDashboard);
                    _snackBar.Add(response.Messages[0], Severity.Success);
                }
                else
                {
                    OnSearch("");
                    foreach (var message in response.Messages)
                    {
                        _snackBar.Add(message, Severity.Error);
                    }
                }
            }
        }

        

        void OnClick(ItemClickEventArgs e)
        {
            Console.WriteLine($"Item Clicked => Menu: {e.ContextMenuId}, MenuTarget: {e.ContextMenuTargetId}, " +
                $"IsCanceled: {e.IsCanceled}, MenuItem: {e.MenuItemElement}, MouseEvent: {e.MouseEvent}");
        }

    }
}