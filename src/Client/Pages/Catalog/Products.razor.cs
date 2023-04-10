using ArdaManager.Application.Features.Products.Queries.GetAllPaged;
using ArdaManager.Application.Features.Products.Commands.AddEdit;
using ArdaManager.Application.Requests.Catalog;
using ArdaManager.Client.Extensions;
using ArdaManager.Client.Infrastructure.Managers.Catalog.Product;
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

namespace ArdaManager.Client.Pages.Catalog
{
    public partial class Products
    {
        [Inject] private IProductManager ProductManager { get; set; }

        [CascadingParameter] private HubConnection HubConnection { get; set; }

        private IEnumerable<GetAllPagedProductsResponse> _pagedData;
        private MudTable<GetAllPagedProductsResponse> _table;
        private int _totalItems;
        private int _currentPage;
        private string _searchString = "";

        private ClaimsPrincipal _currentUser;
        private bool _canCreateProducts;
        private bool _canEditProducts;
        private bool _canDeleteProducts;
        private bool _canExportProducts;
        private bool _canSearchProducts;
        private bool _loaded;

        protected override async Task OnInitializedAsync()
        {
            _currentUser = await _authenticationManager.CurrentUser();
            _canCreateProducts = (await _authorizationService.AuthorizeAsync(_currentUser, Permissions.Products.Create)).Succeeded;
            _canEditProducts = (await _authorizationService.AuthorizeAsync(_currentUser, Permissions.Products.Edit)).Succeeded;
            _canDeleteProducts = (await _authorizationService.AuthorizeAsync(_currentUser, Permissions.Products.Delete)).Succeeded;
            _canExportProducts = (await _authorizationService.AuthorizeAsync(_currentUser, Permissions.Products.Export)).Succeeded;
            _canSearchProducts = (await _authorizationService.AuthorizeAsync(_currentUser, Permissions.Products.Search)).Succeeded;

            _loaded = true;
            HubConnection = HubConnection.TryInitialize(_navigationManager);
            if (HubConnection.State == HubConnectionState.Disconnected)
            {
                await HubConnection.StartAsync();
            }

            Console.WriteLine("debug flag");


        }

        private async Task<TableData<GetAllPagedProductsResponse>> ServerReload(TableState state)
        {
            if (!string.IsNullOrWhiteSpace(_searchString))
            {
                state.Page = 0;
            }
            await LoadData(state.Page, state.PageSize, state);
            return new TableData<GetAllPagedProductsResponse> { TotalItems = _totalItems, Items = _pagedData };

        }

        private async Task LoadData(int pageNumber, int pageSize, TableState state)
        {

            string[] orderings = null;// new [] { "createdon descending" };

            

            if (!string.IsNullOrEmpty(state.SortLabel))
            {
                //orderings = state.SortDirection != SortDirection.None ? new[] {$"{state.SortLabel}, {state.SortDirection}"} : new[] {$"CreatedOn, Descending"};
                orderings = state.SortDirection != SortDirection.None ? new[] {$"{state.SortLabel}", $"{state.SortDirection}" } : new[] { $"{state.SortLabel}", $"Ascending" };
            }
            else
                orderings = new string[]{ "CreatedOn", "descending"};

            var request = new GetAllPagedProductsRequest { PageSize = pageSize, PageNumber = pageNumber + 1, SearchString = _searchString, isTemplate=false, Orderby = orderings };
            var response = await ProductManager.GetProductsAsync(request);
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

        private async Task ExportToExcel()
        {
            var response = await ProductManager.ExportToExcelAsync(_searchString);
            if (response.Succeeded)
            {
                await _jsRuntime.InvokeVoidAsync("Download", new
                {
                    ByteArray = response.Data,
                    FileName = $"{nameof(Products).ToLower()}_{DateTime.Now:ddMMyyyyHHmmss}.xlsx",
                    MimeType = ApplicationConstants.MimeTypes.OpenXml
                });
                _snackBar.Add(string.IsNullOrWhiteSpace(_searchString)
                    ? _localizer["Products exported"]
                    : _localizer["Filtered Products exported"], Severity.Success);
            }
            else
            {
                foreach (var message in response.Messages)
                {
                    _snackBar.Add(message, Severity.Error);
                }
            }
        }

        private async Task InvokeModal(int id = 0)
        {
            var parameters = new DialogParameters();
            if (id != 0)
            {
                var product = _pagedData.FirstOrDefault(c => c.Id == id);
                if (product != null)
                {
                    parameters.Add(nameof(AddEditProductModal.AddEditProductModel), new AddEditProductCommand
                    {
                        Id = product.Id,
                        Name = product.Name,
                        Description = product.Description,
                        Rate = product.Rate,
                        PatternId = product.PatternId ?? 0,
                        CategoryId = product.CategoryId,
                        Code = product.Code,
                        BoolProperty1 = product.BoolProperty1,
                        Type = product.Type,
                        Kind= product.Kind,
                        LastProductionDate = product.LastProductionDate,
                        MeasurementUnitId = product.MeasurementUnitId,
                        TemplateId = product.TemplateId,
                        //ImageDataURL = product.ImageDataURL,
                        
                        Diameter = product.Diameter,
                        Height = product.Height,
                        Weight = product.Weight,
                        Width = product.Width,
                        Length= product.Length,

                        DiameterTolerance= product.DiameterTolerance,
                        HeightTolerance= product.HeightTolerance,   
                        WeightTolerance= product.WeightTolerance,   
                        WidthTolerance = product.WidthTolerance,
                        LengthTolerance = product.LengthTolerance,

                        DiameterMUId = product.DiameterMUId ?? 0,
                        HeightMUId = product.HeightMUId ?? 0,
                        WeightMUId = product.WeightMUId ?? 0,
                        WidthMUId = product.WidthMUId ?? 0,
                        LengthMUId = product.LengthMUId ?? 0,


                    });
                }
                else {
                    parameters.Add(nameof(AddEditProductModal.AddEditProductModel), new AddEditProductCommand
                    {
                        Guid = Guid.NewGuid(),
                        MeasurementUnitId = 1,

                        

                    });
                }
            }
            else
            {
                parameters.Add(nameof(AddEditProductModal.AddEditProductModel), new AddEditProductCommand
                {
                    MeasurementUnitId = 1

                });
            }
            var options = new DialogOptionsEx
            {
                CloseButton = true,
                MaxWidth = MaxWidth.Medium,
                FullWidth = false,
                DisableBackdropClick= true,
                FullHeight= false,
                DragMode = MudDialogDragMode.WithoutBounds,
                Animations = new[] { AnimationType.Pulse },
                Position = DialogPosition.Center,
                Resizeable = false
            };
            var dialog = await _dialogService.ShowEx<AddEditProductModal>(id == 0 ? _localizer["Create"] : _localizer["Edit"], parameters, options);
            var result =  await dialog.Result;
            if (!result.Canceled)
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
            var options = new DialogOptions { CloseButton = true, MaxWidth = MaxWidth.Small, FullWidth = true, DisableBackdropClick = true };
            var dialog = _dialogService.Show<Shared.Dialogs.DeleteConfirmation>(_localizer["Delete"], parameters, options);
            var result = await dialog.Result;
            if (!result.Cancelled)
            {
                var response = await ProductManager.DeleteAsync(id);
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

    }
}