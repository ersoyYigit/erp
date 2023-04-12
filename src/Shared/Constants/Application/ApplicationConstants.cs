using Microsoft.Extensions.Configuration;
using System;
using System.Collections.Generic;
using System.Drawing;

namespace ArdaManager.Shared.Constants.Application
{
    public static class ApplicationConstants
    {
        public static IConfiguration config;
        public static void Initialize(IConfiguration Configuration)
        {
            config = Configuration;
        }
        public static class SignalR
        {
            /// <summary>
            /// TODO: Publish
            /// </summary>
            //public const string Server = "https://localhost:44398";
            public static readonly string Server = config["BaseAddress"];
            //public static readonly string Server = "https://ardaapi.azurewebsites.net";
            //public const string Server = "https://api.ardaglassware.com:44398";
            public const string HubUrl = "/signalRHub";
            public const string SendUpdateDashboard = "UpdateDashboardAsync";
            public const string ReceiveUpdateDashboard = "UpdateDashboard";
            public const string SendRegenerateTokens = "RegenerateTokensAsync";
            public const string ReceiveRegenerateTokens = "RegenerateTokens";
            public const string ReceiveChatNotification = "ReceiveChatNotification";
            public const string SendChatNotification = "ChatNotificationAsync";
            public const string ReceiveMessage = "ReceiveMessage";
            public const string SendMessage = "SendMessageAsync";
            public const string SendDocStateNotification = "DocStateNotificationAsync";
            public const string ReceiveDocStateNotification = "ReceiveDocStateNotificationAsync";

            public const string OnConnect = "OnConnectAsync";
            public const string ConnectUser = "ConnectUser";
            public const string OnDisconnect = "OnDisconnectAsync";
            public const string DisconnectUser = "DisconnectUser";
            public const string OnChangeRolePermissions = "OnChangeRolePermissions";
            public const string LogoutUsersByRole = "LogoutUsersByRole";
        }
        public static class Cache
        {
            public static TimeSpan? SlidingExpiration = TimeSpan.FromMinutes(5);
            public static TimeSpan? AbsoluteExpirationRelativeToNow = TimeSpan.FromDays(1);

            public const string GetAllPatternsCacheKey = "all-patterns";
            public const string GetAllCategoriesCacheKey = "all-categories";
            public const string GetAllCategoriesByTypeCacheKey = "all-categories-by-type-";
            public const string GetAllWarehousesCacheKey = "all-warehouses";
            public const string GetAllProductionLinesCacheKey = "all-production-lines-key";
            public const string GetAllRacksCacheKey = "all-racks-key";
            public const string GetAllFairsCacheKey = "all-fairs-key";
            public const string GetAllCompanyFairsCacheKey = "all-company-fairs-key";

            public const string GetAllDocumentTypesCacheKey = "all-document-types";
            public const string GetAllCountriesCacheKey = "all-countries";
            public const string GetAllCitiesCacheKey = "all-cities";
            public const string GetAllCitiesByCountryIdCacheKey = "all-cities-by-country-id-";

            public const string GetAllTemplateWorkOrdersCacheKey = "all-template-work-orders";
            public const string GetTemplateWorkOrdersByIdCacheKey = "template-work-order-by-id";            
            
            public const string GetAllWarehouseReceiptsCacheKey = "all-warehouse-receipts";
            public const string GetAllWarehouseRequestsCacheKey = "all-warehouse-requests";
            public const string GetAllWarehouseReceiptsWarehouseIdCacheKey = "all-warehouse-by-warehouseId-receipts";
            public const string GetWarehouseReceiptsByIdCacheKey = "template-warehouse-receipt-by-id";


            public const string GetAllPurchaseRequestsCacheKey = "all-purchase-requests";
            public const string GetPurchaseRequestByIdCacheKey = "template-purchase-requests-by-id";
            
            public const string GetAllPurchaseOffersCacheKey = "all-purchase-offers";
            public const string GetPurchaseOfferByIdCacheKey = "template-purchase-offer-by-id";
            
            
            public const string GetAllPurchaseOrdersCacheKey = "all-purchase-order";
            public const string GetPurchaseOrderByIdCacheKey = "template-purchase-order-by-id";


            public const string GetAllProductCategoriesCacheKey = "all-product-categories";

            public static string GetAllRecipeItemssByTypeCacheKey = "all-recipe-items";

            public static string GetAllMUsCacheKey = "all-mus-items";
            public static string GetAllWarehousesStocksCacheKey = "all-warehouses-stocks";
            public static string GetAllCurrenciesCacheKey = "all-currencies";
            public static string GetAllTaxesCacheKey = "all-taxes";
            public static string GetAllExchangeRatesCacheKey = "all-exchangerates";
            public static string GetExchangeRatesByDateCacheKey = "by-date-exchangerates-";
            
            


            public static string GetAllEntityExtendedAttributesCacheKey(string entityFullName)
            {
                return $"all-{entityFullName}-extended-attributes";
            }

            public static string GetAllEntityExtendedAttributesByEntityIdCacheKey<TEntityId>(string entityFullName, TEntityId entityId)
            {
                return $"all-{entityFullName}-extended-attributes-{entityId}";
            }
        }

        public static class MimeTypes
        {
            public const string OpenXml = "application/vnd.openxmlformats-officedocument.spreadsheetml.sheet";
        }
        public static class Shared
        {
            public static readonly List<string> Departments = new List<string>() { "", "Departman 1","Departman 2", "Departman 3" };
        }
    }
}