namespace DTKH2024.SbinSolution.Web.Areas.App.Startup
{
    public class AppPageNames
    {
        public static class Common
        {
            public const string HistoryTypes = "Administration.HistoryTypes.HistoryTypes";
            public const string TransactionStatuses = "Administration.TransactionStatuses.TransactionStatuses";
            public const string BenefitsRankLevels = "Administration.BenefitsRankLevels.BenefitsRankLevels";
            public const string RankLevels = "Administration.RankLevels.RankLevels";
            public const string Devices = "Administration.Devices.Devices";
            public const string StatusDevices = "Administration.StatusDevices.StatusDevices";
            public const string Brands = "Administration.Brands.Brands";
            public const string Administration = "Administration";
            public const string Roles = "Administration.Roles";
            public const string Users = "Administration.Users";
            public const string AuditLogs = "Administration.AuditLogs";
            public const string OrganizationUnits = "Administration.OrganizationUnits";
            public const string Languages = "Administration.Languages";
            public const string DemoUiComponents = "Administration.DemoUiComponents";
            public const string UiCustomization = "Administration.UiCustomization";
            public const string WebhookSubscriptions = "Administration.WebhookSubscriptions";
            public const string DynamicProperties = "Administration.DynamicProperties";
            public const string DynamicEntityProperties = "Administration.DynamicEntityProperties";
            public const string Notifications = "Administration.Notifications";
            public const string Notifications_Inbox = "Administration.Notifications.Inbox";
            public const string Notifications_MassNotifications = "Administration.Notifications.MassNotifications";

            public const string SettingDevice = "Administration.SettingDevice";
            public const string SettingTransaction = "Administration.SettingTransaction";
            public const string SettingNotify = "Administration.SettingNotify";
            public const string Categories = "Administration.Categories";
            public const string Ranks = "Administration.Ranks";
        }

        public static class Host
        {
            public const string Tenants = "Tenants";
            public const string Editions = "Editions";
            public const string Maintenance = "Administration.Maintenance";
            public const string Settings = "Administration.Settings.Host";
            public const string Dashboard = "HostDashboard";
        }

        public static class Tenant
        {
            public const string Dashboard = "TenantDashboard";
            public const string Settings = "Administration.Settings.Tenant";
            public const string SubscriptionManagement = "Administration.SubscriptionManagement.Tenant";
        }
    }
}