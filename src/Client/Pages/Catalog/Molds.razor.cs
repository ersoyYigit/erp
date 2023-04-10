using ArdaManager.Application.Features.Molds.Queries.GetAllPaged;
using ArdaManager.Application.Features.Molds.Commands.AddEdit;
using ArdaManager.Application.Requests.Catalog;
using ArdaManager.Client.Extensions;
using ArdaManager.Client.Infrastructure.Managers.Catalog.Mold;
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
    public partial class Molds
    {
        [Inject] private IMoldManager MoldManager { get; set; }

        [CascadingParameter] private HubConnection HubConnection { get; set; }

        private IEnumerable<GetAllPagedMoldsResponse> _pagedData;
        private MudTable<GetAllPagedMoldsResponse> _table;
        private int _totalItems;
        private int _currentPage;
        private string _searchString = "";

        private ClaimsPrincipal _currentUser;
        private bool _canCreateMolds;
        private bool _canEditMolds;
        private bool _canDeleteMolds;
        private bool _canExportMolds;
        private bool _canSearchMolds;
        private bool _loaded = false;

        private IJSRuntime JsRuntime;

        protected override async Task OnInitializedAsync()
        {
            _currentUser = await _authenticationManager.CurrentUser();
            _canCreateMolds = (await _authorizationService.AuthorizeAsync(_currentUser, Permissions.Molds.Create)).Succeeded;
            _canEditMolds = (await _authorizationService.AuthorizeAsync(_currentUser, Permissions.Molds.Edit)).Succeeded;
            _canDeleteMolds = (await _authorizationService.AuthorizeAsync(_currentUser, Permissions.Molds.Delete)).Succeeded;
            _canExportMolds = (await _authorizationService.AuthorizeAsync(_currentUser, Permissions.Molds.Export)).Succeeded;
            _canSearchMolds = (await _authorizationService.AuthorizeAsync(_currentUser, Permissions.Molds.Search)).Succeeded;


            HubConnection = HubConnection.TryInitialize(_navigationManager);
            if (HubConnection.State == HubConnectionState.Disconnected)
            {
                await HubConnection.StartAsync();
            }

            Console.WriteLine("debug flag");
            _loaded = true;


        }

        private async Task<TableData<GetAllPagedMoldsResponse>> ServerReload(TableState state)
        {
            if (!string.IsNullOrWhiteSpace(_searchString))
            {
                state.Page = 0;
            }
            await LoadData(state.Page, state.PageSize, state);
            return new TableData<GetAllPagedMoldsResponse> { TotalItems = _totalItems, Items = _pagedData };

        }

        private async Task LoadData(int pageNumber, int pageSize, TableState state)
        {
            string[] orderings = null;
            if (!string.IsNullOrEmpty(state.SortLabel))
            {
                orderings = state.SortDirection != SortDirection.None ? new[] { $"{state.SortLabel}", $"{state.SortDirection}" } : new[] { $"{state.SortLabel}", $"Ascending" };
            }

            var request = new GetAllMoldsRequest { PageSize = pageSize, PageNumber = pageNumber + 1, SearchString = _searchString, Orderby = orderings };
            var response = await MoldManager.GetMoldsAsync(request);
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
        }



        private async Task InvokeModal(int id = 0)
        {
            var parameters = new DialogParameters();
            if (id != 0)
            {
                var mold = _pagedData.FirstOrDefault(c => c.Id == id);

                parameters.Add(nameof(AddEditMoldModal.AddEditMoldModel), new AddEditMoldCommand
                {
                    Id = mold.Id,
                    Name = mold.Name,
                    Description = mold.Description,
                    Rate = mold.Rate,
                    PatternId = mold.PatternId ?? 0,
                    CategoryId = mold.CategoryId,
                    Code = mold.Code,
                    BoolProperty1 = mold.BoolProperty1,
                    ProductType = mold.Type,
                    LastProductionDate = mold.LastProductionDate,
                    MeasurementUnitId = mold.MeasurementUnitId,
                    //ImageDataURL = mold.ImageDataURL,

                    IsNew = mold.IsNew,
                    ModelOwner = mold.ModelOwner,

                    String1 = mold.String1,

                    Diameter = mold.Diameter,
                    Height = mold.Height,
                    Weight = mold.Weight,
                    Width = mold.Width,
                    Length = mold.Length,

                    DiameterTolerance = mold.DiameterTolerance,
                    HeightTolerance = mold.HeightTolerance,
                    WeightTolerance = mold.WeightTolerance,
                    WidthTolerance = mold.WidthTolerance,
                    LengthTolerance = mold.LengthTolerance,

                    DiameterMUId = mold.DiameterMUId ?? 0,
                    HeightMUId = mold.HeightMUId ?? 0,
                    WeightMUId = mold.WeightMUId ?? 0,
                    WidthMUId = mold.WidthMUId ?? 0,
                    LengthMUId = mold.LengthMUId ?? 0,
                    CompanyId = mold.CompanyId ?? 0,

                    TemplatePatternId = mold.PatternId,
                    ProductionDate = mold.ProductionDate,
                    Guid = mold.Guid
                });
            }
            else
            {
                parameters.Add(nameof(AddEditMoldModal.AddEditMoldModel), new AddEditMoldCommand
                {
                    IsNew= true,
                    Guid = Guid.NewGuid()
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
            var dialog = _dialogService.ShowEx<AddEditMoldModal>(id == 0 ? _localizer["Create"] : _localizer["Edit"], parameters, options);
            var result = dialog.Result;
            if (!result.Result.IsCanceled)
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
                var response = await MoldManager.DeleteAsync(id);
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