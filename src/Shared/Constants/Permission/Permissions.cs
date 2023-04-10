using System.Collections.Generic;
using System.Linq;
using System.Reflection;

namespace ArdaManager.Shared.Constants.Permission
{
    public static class Permissions
    {
        public static class Products
        {
            public const string View = "Permissions.Products.View";
            public const string Create = "Permissions.Products.Create";
            public const string Edit = "Permissions.Products.Edit";
            public const string Delete = "Permissions.Products.Delete";
            public const string Export = "Permissions.Products.Export";
            public const string Search = "Permissions.Products.Search";
        }

        public static class Templates
        {
            public const string View = "Permissions.Templates.View";
            public const string Create = "Permissions.Templates.Create";
            public const string Edit = "Permissions.Templates.Edit";
            public const string Delete = "Permissions.Templates.Delete";
            public const string Export = "Permissions.Templates.Export";
            public const string Search = "Permissions.Templates.Search";
        }


        public static class Molds
        {
            public const string View = "Permissions.Molds.View";
            public const string Create = "Permissions.Molds.Create";
            public const string Edit = "Permissions.Molds.Edit";
            public const string Delete = "Permissions.Molds.Delete";
            public const string Export = "Permissions.Molds.Export";
            public const string Search = "Permissions.Molds.Search";
        }

        public static class Persons
        {
            public const string View = "Permissions.Persons.View";
            public const string Create = "Permissions.Persons.Create";
            public const string Edit = "Permissions.Persons.Edit";
            public const string Delete = "Permissions.Persons.Delete";
            public const string Export = "Permissions.Persons.Export";
            public const string Search = "Permissions.Persons.Search";
        }



        public static class Patterns
        {
            public const string View = "Permissions.Patterns.View";
            public const string Create = "Permissions.Patterns.Create";
            public const string Edit = "Permissions.Patterns.Edit";
            public const string Delete = "Permissions.Patterns.Delete";
            public const string Export = "Permissions.Patterns.Export";
            public const string Search = "Permissions.Patterns.Search";
        }


        public static class Categories
        {
            public const string View = "Permissions.Categories.View";
            public const string Create = "Permissions.Categories.Create";
            public const string Edit = "Permissions.Categories.Edit";
            public const string Delete = "Permissions.Categories.Delete";
            public const string Export = "Permissions.Categories.Export";
            public const string Search = "Permissions.Categories.Search";
        }



        public static class Documents
        {
            public const string View = "Permissions.Documents.View";
            public const string Create = "Permissions.Documents.Create";
            public const string Edit = "Permissions.Documents.Edit";
            public const string Delete = "Permissions.Documents.Delete";
            public const string Search = "Permissions.Documents.Search";
        }

        public static class DocumentTypes
        {
            public const string View = "Permissions.DocumentTypes.View";
            public const string Create = "Permissions.DocumentTypes.Create";
            public const string Edit = "Permissions.DocumentTypes.Edit";
            public const string Delete = "Permissions.DocumentTypes.Delete";
            public const string Export = "Permissions.DocumentTypes.Export";
            public const string Search = "Permissions.DocumentTypes.Search";
        }

        public static class DocumentExtendedAttributes
        {
            public const string View = "Permissions.DocumentExtendedAttributes.View";
            public const string Create = "Permissions.DocumentExtendedAttributes.Create";
            public const string Edit = "Permissions.DocumentExtendedAttributes.Edit";
            public const string Delete = "Permissions.DocumentExtendedAttributes.Delete";
            public const string Export = "Permissions.DocumentExtendedAttributes.Export";
            public const string Search = "Permissions.DocumentExtendedAttributes.Search";
        }

        public static class Users
        {
            public const string View = "Permissions.Users.View";
            public const string Create = "Permissions.Users.Create";
            public const string Edit = "Permissions.Users.Edit";
            public const string Delete = "Permissions.Users.Delete";
            public const string Export = "Permissions.Users.Export";
            public const string Search = "Permissions.Users.Search";
        }

        public static class Roles
        {
            public const string View = "Permissions.Roles.View";
            public const string Create = "Permissions.Roles.Create";
            public const string Edit = "Permissions.Roles.Edit";
            public const string Delete = "Permissions.Roles.Delete";
            public const string Search = "Permissions.Roles.Search";
        }

        public static class RoleClaims
        {
            public const string View = "Permissions.RoleClaims.View";
            public const string Create = "Permissions.RoleClaims.Create";
            public const string Edit = "Permissions.RoleClaims.Edit";
            public const string Delete = "Permissions.RoleClaims.Delete";
            public const string Search = "Permissions.RoleClaims.Search";
        }

        public static class Currencies
        {
            public const string View = "Permissions.Currencies.View";
            public const string Create = "Permissions.Currencies.Create";
            public const string Edit = "Permissions.Currencies.Edit";
            public const string Delete = "Permissions.Currencies.Delete";
            public const string Search = "Permissions.Currencies.Search";
        }
        public static class Taxes
        {
            public const string View = "Permissions.Taxes.View";
            public const string Create = "Permissions.Taxes.Create";
            public const string Edit = "Permissions.Taxes.Edit";
            public const string Delete = "Permissions.Taxes.Delete";
        }

        public static class Communication
        {
            public const string Chat = "Permissions.Communication.Chat";
        }

        public static class Approval
        {
            public const string Scenaios = "Permissions.Approval.Scenaios";
        }

        public static class Preferences
        {
            public const string ChangeLanguage = "Permissions.Preferences.ChangeLanguage";

            //TODO - add permissions
        }

        public static class Dashboards
        {
            public const string View = "Permissions.Dashboards.View";
        }

        public static class Hangfire
        {
            public const string View = "Permissions.Hangfire.View";
        }

        public static class AuditTrails
        {
            public const string View = "Permissions.AuditTrails.View";
            public const string Export = "Permissions.AuditTrails.Export";
            public const string Search = "Permissions.AuditTrails.Search";
        }

        public static class Companies
        {
            public const string View = "Permissions.Companies.View";
            public const string Create = "Permissions.Companies.Create";
            public const string Edit = "Permissions.Companies.Edit";
            public const string Delete = "Permissions.Companies.Delete";
            public const string Export = "Permissions.Companies.Export";
            public const string Search = "Permissions.Companies.Search";
        }


        public static class Warehouses
        {
            public const string View = "Permissions.Warehouses.View";
            public const string Create = "Permissions.Warehouses.Create";
            public const string Edit = "Permissions.Warehouses.Edit";
            public const string Delete = "Permissions.Warehouses.Delete";
            public const string Search = "Permissions.Warehouses.Search";
        }
        public static class ProductionLines
        {
            public const string View = "Permissions.ProductionLines.View";
            public const string Create = "Permissions.ProductionLines.Create";
            public const string Edit = "Permissions.ProductionLines.Edit";
            public const string Delete = "Permissions.ProductionLines.Delete";
            public const string Search = "Permissions.ProductionLines.Search";
        }

        public static class Orders
        {
            public const string View = "Permissions.Orders.View";
            public const string Create = "Permissions.Orders.Create";
            public const string Edit = "Permissions.Orders.Edit";
            public const string Delete = "Permissions.Orders.Delete";
            public const string Search = "Permissions.Orders.Search";
        }

        public static class RecipeItems
        {
            public const string View = "Permissions.RecipeItems.View";
            public const string Create = "Permissions.RecipeItems.Create";
            public const string Edit = "Permissions.RecipeItems.Edit";
            public const string Delete = "Permissions.RecipeItems.Delete";
            public const string Export = "Permissions.RecipeItems.Export";
            public const string Search = "Permissions.RecipeItems.Search";
        }



        public static class TemplateWorkOrders
        {
            public const string View = "Permissions.TemplateWorkOrders.View";
            public const string Create = "Permissions.TemplateWorkOrders.Create";
            public const string Edit = "Permissions.TemplateWorkOrders.Edit";
            public const string Delete = "Permissions.TemplateWorkOrders.Delete";
            public const string Search = "Permissions.TemplateWorkOrders.Search";
        }



        public static class WarehouseReceipts
        {
            public const string View = "Permissions.WarehouseReceipts.View";
            public const string Create = "Permissions.WarehouseReceipts.Create";
            public const string Edit = "Permissions.WarehouseReceipts.Edit";
            public const string Delete = "Permissions.WarehouseReceipts.Delete";
            public const string Search = "Permissions.WarehouseReceipts.Search";
        }


        
        public static class PurchaseRequests
        {
            public const string View = "Permissions.PurchaseRequests.View";
            public const string Create = "Permissions.PurchaseRequests.Create";
            public const string Edit = "Permissions.PurchaseRequests.Edit";
            public const string Delete = "Permissions.PurchaseRequests.Delete";
            public const string Search = "Permissions.PurchaseRequests.Search";
        }

        public static class PurchaseOffers
        {
            public const string View = "Permissions.PurchaseOffers.View";
            public const string Create = "Permissions.PurchaseOffers.Create";
            public const string Edit = "Permissions.PurchaseOffers.Edit";
            public const string Delete = "Permissions.PurchaseOffers.Delete";
            public const string Search = "Permissions.PurchaseOffers.Search";
        }
        public static class PurchaseOrders
        {
            public const string View = "Permissions.PurchaseOrders.View";
            public const string Create = "Permissions.PurchaseOrders.Create";
            public const string Edit = "Permissions.PurchaseOrders.Edit";
            public const string Delete = "Permissions.PurchaseOrders.Delete";
            public const string Search = "Permissions.PurchaseOrders.Search";
        }




        public static class Reports
        {
            public const string GetAllWarehousesStocks = "Permissions.GetAllWarehousesStocks.View";
        }






        /// <summary>
        /// Returns a list of Permissions.
        /// </summary>
        /// <returns></returns>
        public static List<string> GetRegisteredPermissions()
        {
            var permssions = new List<string>();
            foreach (var prop in typeof(Permissions).GetNestedTypes().SelectMany(c => c.GetFields(BindingFlags.Public | BindingFlags.Static | BindingFlags.FlattenHierarchy)))
            {
                var propertyValue = prop.GetValue(null);
                if (propertyValue is not null)
                    permssions.Add(propertyValue.ToString());
            }
            return permssions;
        }
    }
}