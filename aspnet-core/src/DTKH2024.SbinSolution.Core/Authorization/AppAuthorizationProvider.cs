﻿using Abp.Authorization;
using Abp.Configuration.Startup;
using Abp.Localization;
using Abp.MultiTenancy;

namespace DTKH2024.SbinSolution.Authorization
{
    /// <summary>
    /// Application's authorization provider.
    /// Defines permissions for the application.
    /// See <see cref="AppPermissions"/> for all permission names.
    /// </summary>
    public class AppAuthorizationProvider : AuthorizationProvider
    {
        private readonly bool _isMultiTenancyEnabled;

        public AppAuthorizationProvider(bool isMultiTenancyEnabled)
        {
            _isMultiTenancyEnabled = isMultiTenancyEnabled;
        }

        public AppAuthorizationProvider(IMultiTenancyConfig multiTenancyConfig)
        {
            _isMultiTenancyEnabled = multiTenancyConfig.IsEnabled;
        }

        public override void SetPermissions(IPermissionDefinitionContext context)
        {
            //COMMON PERMISSIONS (FOR BOTH OF TENANTS AND HOST)

            var pages = context.GetPermissionOrNull(AppPermissions.Pages) ?? context.CreatePermission(AppPermissions.Pages, L("Pages"));

            var adminDevice = pages.CreateChildPermission(AppPermissions.Pages_AdministrationDevice_TransactionBins_Create, L("CreateNewTransactionBinAdminDevice"));
            var userTransaction = pages.CreateChildPermission(AppPermissions.Pages_CustomerTransactionBins_Update, L("CustomerUpdateTransaction"));


            var scanQR = pages.CreateChildPermission(AppPermissions.Pages_ScanQR, L("ScanQR"));

            var redeemGifts = pages.CreateChildPermission(AppPermissions.Pages_RedeemGifts, L("RedeemGifts"));
       
            var orderHistories = pages.CreateChildPermission(AppPermissions.Pages_OrderHistories, L("OrderHistories"));
            orderHistories.CreateChildPermission(AppPermissions.Pages_OrderHistories_Create, L("CreateNewOrderHistory"));
            orderHistories.CreateChildPermission(AppPermissions.Pages_OrderHistories_Edit, L("EditOrderHistory"));
            orderHistories.CreateChildPermission(AppPermissions.Pages_OrderHistories_Delete, L("DeleteOrderHistory"));

            var wareHouseGifts = pages.CreateChildPermission(AppPermissions.Pages_WareHouseGifts, L("WareHouseGifts"));
            wareHouseGifts.CreateChildPermission(AppPermissions.Pages_WareHouseGifts_Create, L("CreateNewWareHouseGift"));
            wareHouseGifts.CreateChildPermission(AppPermissions.Pages_WareHouseGifts_Edit, L("EditWareHouseGift"));
            wareHouseGifts.CreateChildPermission(AppPermissions.Pages_WareHouseGifts_Delete, L("DeleteWareHouseGift"));

            pages.CreateChildPermission(AppPermissions.Pages_DemoUiComponents, L("DemoUiComponents"));

            var administration = pages.CreateChildPermission(AppPermissions.Pages_Administration, L("Administration"));

            var transactionBins = administration.CreateChildPermission(AppPermissions.Pages_Administration_TransactionBins, L("TransactionBins"));
            transactionBins.CreateChildPermission(AppPermissions.Pages_Administration_TransactionBins_Create, L("CreateNewTransactionBin"));
            transactionBins.CreateChildPermission(AppPermissions.Pages_Administration_TransactionBins_Edit, L("EditTransactionBin"));
            transactionBins.CreateChildPermission(AppPermissions.Pages_Administration_TransactionBins_Delete, L("DeleteTransactionBin"));

            var categoryPromotions = administration.CreateChildPermission(AppPermissions.Pages_Administration_CategoryPromotions, L("CategoryPromotions"));
            categoryPromotions.CreateChildPermission(AppPermissions.Pages_Administration_CategoryPromotions_Create, L("CreateNewCategoryPromotion"));
            categoryPromotions.CreateChildPermission(AppPermissions.Pages_Administration_CategoryPromotions_Edit, L("EditCategoryPromotion"));
            categoryPromotions.CreateChildPermission(AppPermissions.Pages_Administration_CategoryPromotions_Delete, L("DeleteCategoryPromotion"));

            var productPromotions = administration.CreateChildPermission(AppPermissions.Pages_Administration_ProductPromotions, L("ProductPromotions"));
            productPromotions.CreateChildPermission(AppPermissions.Pages_Administration_ProductPromotions_Create, L("CreateNewProductPromotion"));
            productPromotions.CreateChildPermission(AppPermissions.Pages_Administration_ProductPromotions_Edit, L("EditProductPromotion"));
            productPromotions.CreateChildPermission(AppPermissions.Pages_Administration_ProductPromotions_Delete, L("DeleteProductPromotion"));

            var products = administration.CreateChildPermission(AppPermissions.Pages_Administration_Products, L("Products"));
            products.CreateChildPermission(AppPermissions.Pages_Administration_Products_Create, L("CreateNewProduct"));
            products.CreateChildPermission(AppPermissions.Pages_Administration_Products_Edit, L("EditProduct"));
            products.CreateChildPermission(AppPermissions.Pages_Administration_Products_Delete, L("DeleteProduct"));

            var productTypes = administration.CreateChildPermission(AppPermissions.Pages_Administration_ProductTypes, L("ProductTypes"));
            productTypes.CreateChildPermission(AppPermissions.Pages_Administration_ProductTypes_Create, L("CreateNewProductType"));
            productTypes.CreateChildPermission(AppPermissions.Pages_Administration_ProductTypes_Edit, L("EditProductType"));
            productTypes.CreateChildPermission(AppPermissions.Pages_Administration_ProductTypes_Delete, L("DeleteProductType"));

            var historyTypes = administration.CreateChildPermission(AppPermissions.Pages_Administration_HistoryTypes, L("HistoryTypes"));
            historyTypes.CreateChildPermission(AppPermissions.Pages_Administration_HistoryTypes_Create, L("CreateNewHistoryType"));
            historyTypes.CreateChildPermission(AppPermissions.Pages_Administration_HistoryTypes_Edit, L("EditHistoryType"));
            historyTypes.CreateChildPermission(AppPermissions.Pages_Administration_HistoryTypes_Delete, L("DeleteHistoryType"));

            var transactionStatuses = administration.CreateChildPermission(AppPermissions.Pages_Administration_TransactionStatuses, L("TransactionStatuses"));
            transactionStatuses.CreateChildPermission(AppPermissions.Pages_Administration_TransactionStatuses_Create, L("CreateNewTransactionStatus"));
            transactionStatuses.CreateChildPermission(AppPermissions.Pages_Administration_TransactionStatuses_Edit, L("EditTransactionStatus"));
            transactionStatuses.CreateChildPermission(AppPermissions.Pages_Administration_TransactionStatuses_Delete, L("DeleteTransactionStatus"));

            var benefitsRankLevels = administration.CreateChildPermission(AppPermissions.Pages_Administration_BenefitsRankLevels, L("BenefitsRankLevels"));
            benefitsRankLevels.CreateChildPermission(AppPermissions.Pages_Administration_BenefitsRankLevels_Create, L("CreateNewBenefitsRankLevel"));
            benefitsRankLevels.CreateChildPermission(AppPermissions.Pages_Administration_BenefitsRankLevels_Edit, L("EditBenefitsRankLevel"));
            benefitsRankLevels.CreateChildPermission(AppPermissions.Pages_Administration_BenefitsRankLevels_Delete, L("DeleteBenefitsRankLevel"));

            var rankLevels = administration.CreateChildPermission(AppPermissions.Pages_Administration_RankLevels, L("RankLevels"));
            rankLevels.CreateChildPermission(AppPermissions.Pages_Administration_RankLevels_Create, L("CreateNewRankLevel"));
            rankLevels.CreateChildPermission(AppPermissions.Pages_Administration_RankLevels_Edit, L("EditRankLevel"));
            rankLevels.CreateChildPermission(AppPermissions.Pages_Administration_RankLevels_Delete, L("DeleteRankLevel"));

            var devices = administration.CreateChildPermission(AppPermissions.Pages_Administration_Devices, L("Devices"));
            devices.CreateChildPermission(AppPermissions.Pages_Administration_Devices_Create, L("CreateNewDevice"));
            devices.CreateChildPermission(AppPermissions.Pages_Administration_Devices_Edit, L("EditDevice"));
            devices.CreateChildPermission(AppPermissions.Pages_Administration_Devices_Delete, L("DeleteDevice"));

            var statusDevices = administration.CreateChildPermission(AppPermissions.Pages_Administration_StatusDevices, L("StatusDevices"));
            statusDevices.CreateChildPermission(AppPermissions.Pages_Administration_StatusDevices_Create, L("CreateNewStatusDevice"));
            statusDevices.CreateChildPermission(AppPermissions.Pages_Administration_StatusDevices_Edit, L("EditStatusDevice"));
            statusDevices.CreateChildPermission(AppPermissions.Pages_Administration_StatusDevices_Delete, L("DeleteStatusDevice"));

            var brands = administration.CreateChildPermission(AppPermissions.Pages_Administration_Brands, L("Brands"));
            brands.CreateChildPermission(AppPermissions.Pages_Administration_Brands_Create, L("CreateNewBrand"));
            brands.CreateChildPermission(AppPermissions.Pages_Administration_Brands_Edit, L("EditBrand"));
            brands.CreateChildPermission(AppPermissions.Pages_Administration_Brands_Delete, L("DeleteBrand"));

            var roles = administration.CreateChildPermission(AppPermissions.Pages_Administration_Roles, L("Roles"));
            roles.CreateChildPermission(AppPermissions.Pages_Administration_Roles_Create, L("CreatingNewRole"));
            roles.CreateChildPermission(AppPermissions.Pages_Administration_Roles_Edit, L("EditingRole"));
            roles.CreateChildPermission(AppPermissions.Pages_Administration_Roles_Delete, L("DeletingRole"));

            var users = administration.CreateChildPermission(AppPermissions.Pages_Administration_Users, L("Users"));
            users.CreateChildPermission(AppPermissions.Pages_Administration_Users_Create, L("CreatingNewUser"));
            users.CreateChildPermission(AppPermissions.Pages_Administration_Users_Edit, L("EditingUser"));
            users.CreateChildPermission(AppPermissions.Pages_Administration_Users_Delete, L("DeletingUser"));
            users.CreateChildPermission(AppPermissions.Pages_Administration_Users_ChangePermissions, L("ChangingPermissions"));
            users.CreateChildPermission(AppPermissions.Pages_Administration_Users_Impersonation, L("LoginForUsers"));
            users.CreateChildPermission(AppPermissions.Pages_Administration_Users_Unlock, L("Unlock"));
            users.CreateChildPermission(AppPermissions.Pages_Administration_Users_ChangeProfilePicture, L("UpdateUsersProfilePicture"));

            var languages = administration.CreateChildPermission(AppPermissions.Pages_Administration_Languages, L("Languages"));
            languages.CreateChildPermission(AppPermissions.Pages_Administration_Languages_Create, L("CreatingNewLanguage"), multiTenancySides: _isMultiTenancyEnabled ? MultiTenancySides.Host : MultiTenancySides.Tenant);
            languages.CreateChildPermission(AppPermissions.Pages_Administration_Languages_Edit, L("EditingLanguage"), multiTenancySides: _isMultiTenancyEnabled ? MultiTenancySides.Host : MultiTenancySides.Tenant);
            languages.CreateChildPermission(AppPermissions.Pages_Administration_Languages_Delete, L("DeletingLanguages"), multiTenancySides: _isMultiTenancyEnabled ? MultiTenancySides.Host : MultiTenancySides.Tenant);
            languages.CreateChildPermission(AppPermissions.Pages_Administration_Languages_ChangeTexts, L("ChangingTexts"));
            languages.CreateChildPermission(AppPermissions.Pages_Administration_Languages_ChangeDefaultLanguage, L("ChangeDefaultLanguage"));

            administration.CreateChildPermission(AppPermissions.Pages_Administration_AuditLogs, L("AuditLogs"));

            var organizationUnits = administration.CreateChildPermission(AppPermissions.Pages_Administration_OrganizationUnits, L("OrganizationUnits"));
            organizationUnits.CreateChildPermission(AppPermissions.Pages_Administration_OrganizationUnits_ManageOrganizationTree, L("ManagingOrganizationTree"));
            organizationUnits.CreateChildPermission(AppPermissions.Pages_Administration_OrganizationUnits_ManageMembers, L("ManagingMembers"));
            organizationUnits.CreateChildPermission(AppPermissions.Pages_Administration_OrganizationUnits_ManageRoles, L("ManagingRoles"));

            administration.CreateChildPermission(AppPermissions.Pages_Administration_UiCustomization, L("VisualSettings"));

            var webhooks = administration.CreateChildPermission(AppPermissions.Pages_Administration_WebhookSubscription, L("Webhooks"));
            webhooks.CreateChildPermission(AppPermissions.Pages_Administration_WebhookSubscription_Create, L("CreatingWebhooks"));
            webhooks.CreateChildPermission(AppPermissions.Pages_Administration_WebhookSubscription_Edit, L("EditingWebhooks"));
            webhooks.CreateChildPermission(AppPermissions.Pages_Administration_WebhookSubscription_ChangeActivity, L("ChangingWebhookActivity"));
            webhooks.CreateChildPermission(AppPermissions.Pages_Administration_WebhookSubscription_Detail, L("DetailingSubscription"));
            webhooks.CreateChildPermission(AppPermissions.Pages_Administration_Webhook_ListSendAttempts, L("ListingSendAttempts"));
            webhooks.CreateChildPermission(AppPermissions.Pages_Administration_Webhook_ResendWebhook, L("ResendingWebhook"));

            var dynamicProperties = administration.CreateChildPermission(AppPermissions.Pages_Administration_DynamicProperties, L("DynamicProperties"));
            dynamicProperties.CreateChildPermission(AppPermissions.Pages_Administration_DynamicProperties_Create, L("CreatingDynamicProperties"));
            dynamicProperties.CreateChildPermission(AppPermissions.Pages_Administration_DynamicProperties_Edit, L("EditingDynamicProperties"));
            dynamicProperties.CreateChildPermission(AppPermissions.Pages_Administration_DynamicProperties_Delete, L("DeletingDynamicProperties"));

            var dynamicPropertyValues = dynamicProperties.CreateChildPermission(AppPermissions.Pages_Administration_DynamicPropertyValue, L("DynamicPropertyValue"));
            dynamicPropertyValues.CreateChildPermission(AppPermissions.Pages_Administration_DynamicPropertyValue_Create, L("CreatingDynamicPropertyValue"));
            dynamicPropertyValues.CreateChildPermission(AppPermissions.Pages_Administration_DynamicPropertyValue_Edit, L("EditingDynamicPropertyValue"));
            dynamicPropertyValues.CreateChildPermission(AppPermissions.Pages_Administration_DynamicPropertyValue_Delete, L("DeletingDynamicPropertyValue"));

            var dynamicEntityProperties = dynamicProperties.CreateChildPermission(AppPermissions.Pages_Administration_DynamicEntityProperties, L("DynamicEntityProperties"));
            dynamicEntityProperties.CreateChildPermission(AppPermissions.Pages_Administration_DynamicEntityProperties_Create, L("CreatingDynamicEntityProperties"));
            dynamicEntityProperties.CreateChildPermission(AppPermissions.Pages_Administration_DynamicEntityProperties_Edit, L("EditingDynamicEntityProperties"));
            dynamicEntityProperties.CreateChildPermission(AppPermissions.Pages_Administration_DynamicEntityProperties_Delete, L("DeletingDynamicEntityProperties"));

            var dynamicEntityPropertyValues = dynamicProperties.CreateChildPermission(AppPermissions.Pages_Administration_DynamicEntityPropertyValue, L("EntityDynamicPropertyValue"));
            dynamicEntityPropertyValues.CreateChildPermission(AppPermissions.Pages_Administration_DynamicEntityPropertyValue_Create, L("CreatingDynamicEntityPropertyValue"));
            dynamicEntityPropertyValues.CreateChildPermission(AppPermissions.Pages_Administration_DynamicEntityPropertyValue_Edit, L("EditingDynamicEntityPropertyValue"));
            dynamicEntityPropertyValues.CreateChildPermission(AppPermissions.Pages_Administration_DynamicEntityPropertyValue_Delete, L("DeletingDynamicEntityPropertyValue"));

            var massNotification = administration.CreateChildPermission(AppPermissions.Pages_Administration_MassNotification, L("MassNotifications"));
            massNotification.CreateChildPermission(AppPermissions.Pages_Administration_MassNotification_Create, L("MassNotificationCreate"));

            //TENANT-SPECIFIC PERMISSIONS

            pages.CreateChildPermission(AppPermissions.Pages_Tenant_Dashboard, L("Dashboard"), multiTenancySides: MultiTenancySides.Tenant);

            administration.CreateChildPermission(AppPermissions.Pages_Administration_Tenant_Settings, L("Settings"), multiTenancySides: MultiTenancySides.Tenant);
            administration.CreateChildPermission(AppPermissions.Pages_Administration_Tenant_SubscriptionManagement, L("Subscription"), multiTenancySides: MultiTenancySides.Tenant);

            //HOST-SPECIFIC PERMISSIONS

            var editions = pages.CreateChildPermission(AppPermissions.Pages_Editions, L("Editions"), multiTenancySides: MultiTenancySides.Host);
            editions.CreateChildPermission(AppPermissions.Pages_Editions_Create, L("CreatingNewEdition"), multiTenancySides: MultiTenancySides.Host);
            editions.CreateChildPermission(AppPermissions.Pages_Editions_Edit, L("EditingEdition"), multiTenancySides: MultiTenancySides.Host);
            editions.CreateChildPermission(AppPermissions.Pages_Editions_Delete, L("DeletingEdition"), multiTenancySides: MultiTenancySides.Host);
            editions.CreateChildPermission(AppPermissions.Pages_Editions_MoveTenantsToAnotherEdition, L("MoveTenantsToAnotherEdition"), multiTenancySides: MultiTenancySides.Host);

            var tenants = pages.CreateChildPermission(AppPermissions.Pages_Tenants, L("Tenants"), multiTenancySides: MultiTenancySides.Host);
            tenants.CreateChildPermission(AppPermissions.Pages_Tenants_Create, L("CreatingNewTenant"), multiTenancySides: MultiTenancySides.Host);
            tenants.CreateChildPermission(AppPermissions.Pages_Tenants_Edit, L("EditingTenant"), multiTenancySides: MultiTenancySides.Host);
            tenants.CreateChildPermission(AppPermissions.Pages_Tenants_ChangeFeatures, L("ChangingFeatures"), multiTenancySides: MultiTenancySides.Host);
            tenants.CreateChildPermission(AppPermissions.Pages_Tenants_Delete, L("DeletingTenant"), multiTenancySides: MultiTenancySides.Host);
            tenants.CreateChildPermission(AppPermissions.Pages_Tenants_Impersonation, L("LoginForTenants"), multiTenancySides: MultiTenancySides.Host);

            administration.CreateChildPermission(AppPermissions.Pages_Administration_Host_Settings, L("Settings"), multiTenancySides: MultiTenancySides.Host);

            var maintenance = administration.CreateChildPermission(AppPermissions.Pages_Administration_Host_Maintenance, L("Maintenance"), multiTenancySides: _isMultiTenancyEnabled ? MultiTenancySides.Host : MultiTenancySides.Tenant);
            maintenance.CreateChildPermission(AppPermissions.Pages_Administration_NewVersion_Create, L("SendNewVersionNotification"));

            administration.CreateChildPermission(AppPermissions.Pages_Administration_HangfireDashboard, L("HangfireDashboard"), multiTenancySides: _isMultiTenancyEnabled ? MultiTenancySides.Host : MultiTenancySides.Tenant);
            administration.CreateChildPermission(AppPermissions.Pages_Administration_Host_Dashboard, L("Dashboard"), multiTenancySides: MultiTenancySides.Host);
        }

        private static ILocalizableString L(string name)
        {
            return new LocalizableString(name, SbinSolutionConsts.LocalizationSourceName);
        }
    }
}