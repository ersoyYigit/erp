using ArdaManager.Application.Features.Docs.PurchaseOffers.Commands;
using ArdaManager.Application.Features.Docs.PurchaseOffers.Queries;
using ArdaManager.Application.Features.MeasurementUnits.Queries.GetAll;
using ArdaManager.Application.Features.Products.Queries.Search;
using ArdaManager.Application.Responses.Identity;
using ArdaManager.Client.Extensions;
using ArdaManager.Client.Infrastructure.Managers.Catalog.MeasurementUnit;
using ArdaManager.Client.Infrastructure.Managers.GlobalVariable;
using ArdaManager.Client.Shared.Dialogs;
using ArdaManager.Shared.Constants.Application;
using Blazored.FluentValidation;
using Microsoft.AspNetCore.Components.Web;
using Microsoft.AspNetCore.Components;
using Microsoft.AspNetCore.SignalR.Client;
using MudBlazor.Extensions.Options;
using MudBlazor.Extensions;
using MudBlazor;
using System.Collections.Generic;
using System.Threading.Tasks;
using FluentValidation.Results;
using ArdaManager.Client.Infrastructure.Managers.Docs.PurchaseOffer;
using ArdaManager.Application.Features.Companies.Queries.Search;
using ArdaManager.Client.Shared.Dialogs.RelationPickers.Purchase;
using ArdaManager.Application.Features.Docs.PurchaseRequests.Queries;
using ArdaManager.Application.Models.Chat;
using ArdaManager.Application.Features.Currencies.Queries;
using ArdaManager.Application.Features.Taxes.Queries;
using System.Linq;
using System;
using ArdaManager.Client.Pages.Catalog;
using Newtonsoft.Json.Linq;
using Nextended.Core.COM;

namespace ArdaManager.Client.Pages.Docs.PurchaseOffer
{
    public partial class UpsertPurchaseOfferModal
    {
        [Inject] private IPurchaseOfferManager PurchaseOfferManager { get; set; }
        //[Inject] private IMeasurementUnitManager MeasurementUnitManager { get; set; }
        [Inject] private IGlobalVariableService GlobalVarService { get; set; }
        private List<UserResponse> _userList = new();

        [Parameter] public UpsertPurchaseOfferCommand UpsertPurchaseOfferModel { get; set; } = new();
        [CascadingParameter] private MudDialogInstance MudDialog { get; set; }
        [CascadingParameter] private HubConnection HubConnection { get; set; }

        private List<GetAllMeasurementUnitsResponse> _measurementUnits = new();
        private List<GetAllCurrenciesResponse> _currencies = new();
        private List<GetAllTaxesResponse> _taxes = new();

        private FluentValidationValidator _fluentValidationValidator;
        private bool Validated => _fluentValidationValidator.Validate(options => { options.IncludeAllRuleSets(); });
        private ValidationResult _validationResult;
        private int selectedCurrencyId = 1;
        public void Cancel()
        {
            MudDialog.Cancel();
        }

        private async Task SaveAsync()
        {

            var response = await PurchaseOfferManager.SaveAsync(UpsertPurchaseOfferModel);
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
        }

        protected override async Task OnInitializedAsync()
        {
            await LoadDataAsync();

            if (UpsertPurchaseOfferModel.Id == 0)
            {
                UpsertPurchaseOfferModel.CurrencyId = 1;
                UpsertPurchaseOfferModel.ExchangeRate = 1;
                UpsertPurchaseOfferModel.CurrencyName = _currencies.Where(x => x.Id == 1).FirstOrDefault()?.Name;
                UpsertPurchaseOfferModel.CurrencyCode = _currencies.Where(x => x.Id == 1).FirstOrDefault()?.Code;
                //UpsertPurchaseOfferModel.ExchangeDate = DateTime.UtcNow;
                AddRow();
            }
        }

        private async Task LoadDataAsync()
        {
            _measurementUnits = GlobalVarService.MeasurementUnits;
            _currencies = GlobalVarService.Currencies;
            _taxes = GlobalVarService.Taxes;
            //await SetCurrentUser();
            await Task.CompletedTask;
        }



        private void AddRow()
        {
            UpsertPurchaseOfferModel.PurchaseOfferRowResponse ??= new List<PurchaseOfferRowResponse>();
            UpsertPurchaseOfferModel.PurchaseOfferRowResponse.Add(new PurchaseOfferRowResponse() { 
                CurrencyId = UpsertPurchaseOfferModel.CurrencyId,
                CurrencyCode = UpsertPurchaseOfferModel.CurrencyCode,
                CurrencyName = UpsertPurchaseOfferModel.CurrencyName,
                ExchangeRate = UpsertPurchaseOfferModel.ExchangeRate,
                TaxId = 4
            });
        }


        private async Task DeleteRow(PurchaseOfferRowResponse row)
        {
            UpsertPurchaseOfferModel.PurchaseOfferRowResponse.Remove(row);
            await Task.CompletedTask;
        }


        private void OnDescriptionFieldBlur()
        {
            //AddRow();
        }

        private void CurrencyChanged(IEnumerable<int> values)
        {
            var currencyId = values.FirstOrDefault();
            selectedCurrencyId = currencyId;
            if (UpsertPurchaseOfferModel.PurchaseOfferRowResponse?.Count > 0)
            {
                foreach (var item in UpsertPurchaseOfferModel.PurchaseOfferRowResponse)
                {
                    item.CurrencyId = currencyId;
                }
            }
        }

        private async Task OpenCompanyDialogOnF4Code(KeyboardEventArgs e, UpsertPurchaseOfferCommand context)
        {
            if (e.Code == "F4")
            {
                await OpenCompanySearchDialog(context.RequesterName, context);
            }
        }

        private async Task OpenCompanySearchDialog(string searchStr, UpsertPurchaseOfferCommand context)
        {
            var parameters = new DialogParameters();
            parameters.Add(nameof(CompanySearchDialog.SearchQuery), new CompanySearchQuery
            {
                SearchTerm = searchStr,
                Type = Domain.Enums.CompanyType.Supplier
            });


            var optionsEx = new DialogOptionsEx
            {
                CloseButton = true,
                MaximizeButton = true,
                MaxWidth = MaxWidth.Large,
                FullWidth = false,
                FullScreen = false,
                DisableBackdropClick = true,
                FullHeight = false,
                DragMode = MudDialogDragMode.WithoutBounds,
                Animations = new[] { AnimationType.Pulse },
                Position = DialogPosition.Center,
                Resizeable = true
            };

            var dialog = await _dialogService.ShowEx<CompanySearchDialog>("Ara", parameters, optionsEx);

            var result = await dialog.Result;
            if (!result.Cancelled)
            {
                var selectedCompany = result.Data as CompanySearchResultDto;
                context.CompanyId = selectedCompany.CompanyId;
                context.CompanyName = selectedCompany.Name;


            }
        }
        private async Task OpenProductSearchDialogOnF4Code(KeyboardEventArgs e, PurchaseOfferRowResponse rowCtx)
        {
            if (e.Code == "F4")
            {
                await OpenSearchDialog(rowCtx.ProductCode, rowCtx);
            }
        }
        private async Task OpenProductSearchDialogOnF4Name(KeyboardEventArgs e, PurchaseOfferRowResponse rowCtx)
        {
            if (e.Code == "F4")
            {
                await OpenSearchDialog(rowCtx.ProductName, rowCtx);
            }
        }
        private async Task OpenRequestPickerDialogOnF4Code(KeyboardEventArgs e, UpsertPurchaseOfferCommand context)
        {
            if (e.Code == "F4")
            {
                await OpenRequestPickerDialog(context.RequesterName, context);
            }
        }
        private async Task OpenRequestPickerDialog(string searchStr, UpsertPurchaseOfferCommand context)
        {
            var parameters = new DialogParameters();
            parameters.Add("SearchTerm", searchStr);


            var optionsEx = new DialogOptionsEx{  CloseButton = true, MaximizeButton = true,MaxWidth = MaxWidth.Large,FullWidth = false,FullScreen = false,DisableBackdropClick = true,FullHeight = false,DragMode = MudDialogDragMode.WithoutBounds,Animations = new[] { AnimationType.Pulse },Position = DialogPosition.Center,Resizeable = true};

            var dialog = await _dialogService.ShowEx<PurchaseRequestPicker>("Ara", parameters, optionsEx);

            var result = await dialog.Result;
            if (!result.Cancelled)
            {
                var selectedPurchaseRequest = result.Data as PurchaseRequestDto;
                
                FillDataFromPrevDoc(selectedPurchaseRequest);
            }
        }
        private void FillDataFromPrevDoc(PurchaseRequestDto selectedPurchaseRequest)
        {
            UpsertPurchaseOfferModel.PrevDocId = selectedPurchaseRequest.Id;
            UpsertPurchaseOfferModel.PrevDocNo = selectedPurchaseRequest.DocNo;
            UpsertPurchaseOfferModel.RequesterId = selectedPurchaseRequest.RequesterId;
            UpsertPurchaseOfferModel.RequesterName = selectedPurchaseRequest.RequesterName;    
            UpsertPurchaseOfferModel.RequesterDepartment = selectedPurchaseRequest.RequesterDepartment;
            UpsertPurchaseOfferModel.PurchaseRequestId = selectedPurchaseRequest.Id;
            UpsertPurchaseOfferModel.PurchaseOfferRowResponse.Clear();
            foreach (PurchaseRequestRowDto item in selectedPurchaseRequest.PurchaseRequestRowsDto)
            {

                UpsertPurchaseOfferModel.PurchaseOfferRowResponse.Add(new PurchaseOfferRowResponse() { 
                    Description= item.Description,
                    MeasurementUnitCode= item.MeasurementUnitCode,
                    MeasurementUnitId = item.MeasurementUnitId,
                    MeasurementUnitName = item.MeasurementUnitName,
                    MeasurementUnitSystem= item.MeasurementUnitSystem,
                    Price = 0,
                    ProductCode = item.ProductCode,
                    ProductName = item.ProductName,
                    ProductId = item.ProductId,
                    Quantity = item.Quantity,
                    RowNo = item.RowNo,
                    PurchaseRequestRowId = item.Id,
                    PrevRowId = item.Id,
                    CurrencyId = selectedCurrencyId,
                    ExchangeRate = 1,
                    TaxId = 4
                });
            }
            _snackBar.Add("Talep teklife dönüştürüldü", Severity.Success, config => { config.ShowTransitionDuration = 5000; });
            _snackBar.Add("Lütfen teklif aldığınız firmayı seçin.", Severity.Warning, config => { config.ShowTransitionDuration = 5000; });
            _snackBar.Add("Lütfen ürün fiyatlarını doldurun", Severity.Warning, config => { config.ShowTransitionDuration = 5000; });
        }

        private async Task OpenSearchDialog(string searchStr, PurchaseOfferRowResponse rowCtx)
        {
            var parameters = new DialogParameters();

            parameters.Add("SearchTerm", searchStr);

            //parameters.Add("QrCodeText", $"/catalog/warehouseEntryReceipts/{25}");

            var optionsEx = new DialogOptionsEx
            {
                CloseButton = true,
                MaximizeButton = true,
                MaxWidth = MaxWidth.Large,
                FullWidth = false,
                FullScreen = false,
                DisableBackdropClick = true,
                FullHeight = false,
                DragMode = MudDialogDragMode.WithoutBounds,
                Animations = new[] { AnimationType.Pulse },
                Position = DialogPosition.Center,
                Resizeable = true
            };
            var options = new MudBlazor.DialogOptions() { CloseButton = true, MaxWidth = MaxWidth.ExtraSmall };

            var dialog = await _dialogService.ShowEx<ProductSearchDialog>("Ara", parameters, optionsEx);
            //var dialog = await _dialogService.ShowAsync<Barcode>("QR Kod", parameters, options);
            var result = await dialog.Result;
            if (!result.Cancelled)
            {
                var selectedProduct = result.Data as ProductSearchResultDto;
                rowCtx.ProductId = selectedProduct.ProductId;
                rowCtx.ProductCode = selectedProduct.Code;
                rowCtx.ProductName = selectedProduct.Name;
                rowCtx.MeasurementUnitSystem = selectedProduct.MeasurementUnitSystem;
                rowCtx.MeasurementUnitId = selectedProduct.MeasurementUnitId;


            }
        }

    }
}
