using Abp.Application.Navigation;
using Abp.Authorization;
using Abp.Localization;
using DTKH2024.SbinSolution.Authorization;

namespace DTKH2024.SbinSolution.Web.Areas.App.Startup
{
    public class AppNavigationProvider : NavigationProvider
    {
        public const string MenuName = "App";

        public override void SetNavigation(INavigationProviderContext context)
        {
            var menu = context.Manager.Menus[MenuName] = new MenuDefinition(MenuName, new FixedLocalizableString("Main Menu"));

            menu
                .AddItem(new MenuItemDefinition(
                        AppPageNames.Host.Dashboard,
                        L("Dashboard"),
                        url: "App/HostDashboard",
                        icon: "flaticon-line-graph",
                        permissionDependency: new SimplePermissionDependency(AppPermissions.Pages_Administration_Host_Dashboard)
                    )
                )
                .AddItem(new MenuItemDefinition(
                        AppPageNames.Common.OrderHistories,
                        L("OrderHistories"),
                        url: "App/OrderHistories",
                        icon: "fa-solid fa-clock-rotate-left",
                        permissionDependency: new SimplePermissionDependency(AppPermissions.Pages_OrderHistories)
                    )
                )
                .AddItem(new MenuItemDefinition(
                        AppPageNames.Common.WareHouseGifts,
                        L("WareHouseGifts"),
                        url: "App/WareHouseGifts",
                        icon: "fa-solid fa-gifts",
                        permissionDependency: new SimplePermissionDependency(AppPermissions.Pages_WareHouseGifts)
                    )
                ).AddItem(new MenuItemDefinition(
                        AppPageNames.Host.Tenants,
                        L("Tenants"),
                        url: "App/Tenants",
                        icon: "flaticon-list-3",
                        permissionDependency: new SimplePermissionDependency(AppPermissions.Pages_Tenants)
                    )
                ).AddItem(new MenuItemDefinition(
                        AppPageNames.Host.Editions,
                        L("Editions"),
                        url: "App/Editions",
                        icon: "flaticon-app",
                        permissionDependency: new SimplePermissionDependency(AppPermissions.Pages_Editions)
                    )
                ).AddItem(new MenuItemDefinition(
                        AppPageNames.Tenant.Dashboard,
                        L("Dashboard"),
                        url: "App/TenantDashboard",
                        icon: "flaticon-line-graph",
                        permissionDependency: new SimplePermissionDependency(AppPermissions.Pages_Tenant_Dashboard)
                    )
                ).AddItem(new MenuItemDefinition(
                        AppPageNames.Common.Administration,
                        L("Administration"),
                        icon: "flaticon-interface-8"
                    )

                 .AddItem(new MenuItemDefinition(
                     AppPageNames.Common.PromotionsManagement,
                     L("PromotionsManagement"),
                     icon: ("fa-solid fa-tags"))
                     .AddItem(new MenuItemDefinition(
                                    AppPageNames.Common.ProductPromotions,
                                    L("ProductPromotions"),
                                    url: "App/ProductPromotions",
                                    icon: "fa-solid fa-hand-holding-heart",
                                    permissionDependency: new SimplePermissionDependency(AppPermissions.Pages_Administration_ProductPromotions)
                                )
                            )
                             .AddItem(new MenuItemDefinition(
                                    AppPageNames.Common.CategoryPromotions,
                                    L("CategoryPromotions"),
                                    url: "App/CategoryPromotions",
                                    icon: "fa-solid fa-list",
                                    permissionDependency: new SimplePermissionDependency(AppPermissions.Pages_Administration_CategoryPromotions)
                                )
                            )
                 )

                 .AddItem(new MenuItemDefinition(
                     AppPageNames.Common.ProductManagement,
                     L("ProductManagement"),
                     icon: ("fa-brands fa-product-hunt"))

                         .AddItem(new MenuItemDefinition(
                                AppPageNames.Common.Products,
                                L("Products"),
                                url: "App/Products",
                                icon: "fa-solid fa-box",
                                permissionDependency: new SimplePermissionDependency(AppPermissions.Pages_Administration_Products)
                            )
                        )
                          .AddItem(new MenuItemDefinition(
                                AppPageNames.Common.ProductTypes,
                                L("ProductTypes"),
                                url: "App/ProductTypes",
                                icon: "fa-solid fa-list",
                                permissionDependency: new SimplePermissionDependency(AppPermissions.Pages_Administration_ProductTypes)
                            )
                        )
                 )

               .AddItem(new MenuItemDefinition(
                     AppPageNames.Common.Ranks,
                     L("Ranks"),
                     icon: ("fa-solid fa-ranking-star"))
                         .AddItem(new MenuItemDefinition(
                                 AppPageNames.Common.RankLevels,
                                 L("RankLevels"),
                                 url: "App/RankLevels",
                                 icon: "fa-solid fa-star",
                                 permissionDependency: new SimplePermissionDependency(AppPermissions.Pages_Administration_RankLevels)
                             )
                      )
                     .AddItem(new MenuItemDefinition(
                            AppPageNames.Common.BenefitsRankLevels,
                            L("BenefitsRankLevels"),
                            url: "App/BenefitsRankLevels",
                            icon: "fa-solid fa-heart",
                            permissionDependency: new SimplePermissionDependency(AppPermissions.Pages_Administration_BenefitsRankLevels)
                        )
                 )
                 )
               .AddItem(new MenuItemDefinition(
                    AppPageNames.Common.SettingTransaction,
                    L("SettingTransaction"),
                    icon: "fa-solid fa-arrow-right-arrow-left")
                           .AddItem(new MenuItemDefinition(
                                    AppPageNames.Common.TransactionBins,
                                    L("TransactionBins"),
                                    url: "App/TransactionBins",
                                    icon: "fa-solid fa-hand-holding-dollar",
                                    permissionDependency: new SimplePermissionDependency(AppPermissions.Pages_Administration_TransactionBins)
                                )
                            )

                           .AddItem(new MenuItemDefinition(
                                AppPageNames.Common.TransactionStatuses,
                                L("TransactionStatuses"),
                                url: "App/TransactionStatuses",
                                icon: "fa-solid fa-gears",
                                permissionDependency: new SimplePermissionDependency(AppPermissions.Pages_Administration_TransactionStatuses)
                            )
                        )
                        .AddItem(new MenuItemDefinition(
                        AppPageNames.Common.HistoryTypes,
                        L("HistoryTypes"),
                        url: "App/HistoryTypes",
                        icon: "fa-solid fa-swatchbook",
                        permissionDependency: new SimplePermissionDependency(AppPermissions.Pages_Administration_HistoryTypes)
                        )
                    )

                   )
                .AddItem(new MenuItemDefinition(
                    AppPageNames.Common.SettingDevice,
                    L("SettingDevice"),
                    icon: "fa-solid fa-dumpster")
                                .AddItem(new MenuItemDefinition(
                                        AppPageNames.Common.StatusDevices,
                                        L("StatusDevices"),
                                        url: "App/StatusDevices",
                                        icon: "fa-solid fa-gears",
                                        permissionDependency: new SimplePermissionDependency(AppPermissions.Pages_Administration_StatusDevices)
                            )
                        )
                        .AddItem(new MenuItemDefinition(
                                                AppPageNames.Common.Devices,
                                                L("Devices"),
                                                url: "App/Devices",
                                                icon: "fa-solid fa-dumpster-fire",
                                                permissionDependency: new SimplePermissionDependency(AppPermissions.Pages_Administration_Devices)
                                            )
                                        )
                   )

                .AddItem(new MenuItemDefinition(
                        AppPageNames.Common.Brands,
                        L("Brands"),
                        url: "App/Brands",
                        icon: "fa-solid fa-swatchbook",
                        permissionDependency: new SimplePermissionDependency(AppPermissions.Pages_Administration_Brands)
                    )
                )
                .AddItem(new MenuItemDefinition(
                            AppPageNames.Common.OrganizationUnits,
                            L("OrganizationUnits"),
                            url: "App/OrganizationUnits",
                            icon: "flaticon-map",
                            permissionDependency: new SimplePermissionDependency(AppPermissions
                                .Pages_Administration_OrganizationUnits)
                        )
                    ).AddItem(new MenuItemDefinition(
                            AppPageNames.Common.Roles,
                            L("Roles"),
                            url: "App/Roles",
                            icon: "flaticon-suitcase",
                            permissionDependency: new SimplePermissionDependency(AppPermissions
                                .Pages_Administration_Roles)
                        )
                    ).AddItem(new MenuItemDefinition(
                            AppPageNames.Common.Users,
                            L("Users"),
                            url: "App/Users",
                            icon: "flaticon-users",
                            permissionDependency: new SimplePermissionDependency(AppPermissions
                                .Pages_Administration_Users)
                        )
                    ).AddItem(new MenuItemDefinition(
                            AppPageNames.Common.Languages,
                            L("Languages"),
                            url: "App/Languages",
                            icon: "flaticon-tabs",
                            permissionDependency: new SimplePermissionDependency(AppPermissions
                                .Pages_Administration_Languages)
                        )
                    ).AddItem(new MenuItemDefinition(
                            AppPageNames.Common.AuditLogs,
                            L("AuditLogs"),
                            url: "App/AuditLogs",
                            icon: "flaticon-folder-1",
                            permissionDependency: new SimplePermissionDependency(AppPermissions
                                .Pages_Administration_AuditLogs)
                        )
                    ).AddItem(new MenuItemDefinition(
                            AppPageNames.Host.Maintenance,
                            L("Maintenance"),
                            url: "App/Maintenance",
                            icon: "flaticon-lock",
                            permissionDependency: new SimplePermissionDependency(AppPermissions
                                .Pages_Administration_Host_Maintenance)
                        )
                    ).AddItem(new MenuItemDefinition(
                            AppPageNames.Tenant.SubscriptionManagement,
                            L("Subscription"),
                            url: "App/SubscriptionManagement",
                            icon: "flaticon-refresh",
                            permissionDependency: new SimplePermissionDependency(AppPermissions
                                .Pages_Administration_Tenant_SubscriptionManagement)
                        )
                    ).AddItem(new MenuItemDefinition(
                            AppPageNames.Common.UiCustomization,
                            L("VisualSettings"),
                            url: "App/UiCustomization",
                            icon: "flaticon-medical",
                            permissionDependency: new SimplePermissionDependency(AppPermissions
                                .Pages_Administration_UiCustomization)
                        )
                    ).AddItem(new MenuItemDefinition(
                            AppPageNames.Common.WebhookSubscriptions,
                            L("WebhookSubscriptions"),
                            url: "App/WebhookSubscription",
                            icon: "flaticon2-world",
                            permissionDependency: new SimplePermissionDependency(AppPermissions
                                .Pages_Administration_WebhookSubscription)
                        )
                    )
                    .AddItem(new MenuItemDefinition(
                            AppPageNames.Common.DynamicProperties,
                            L("DynamicProperties"),
                            url: "App/DynamicProperty",
                            icon: "flaticon-interface-8",
                            permissionDependency: new SimplePermissionDependency(AppPermissions
                                .Pages_Administration_DynamicProperties)
                        )
                    )
                    .AddItem(new MenuItemDefinition(
                            AppPageNames.Common.Notifications,
                            L("Notifications"),
                            icon: "flaticon-alarm"
                        ).AddItem(new MenuItemDefinition(
                                AppPageNames.Common.Notifications_Inbox,
                                L("Inbox"),
                                url: "App/Notifications",
                                icon: "flaticon-mail-1"
                            )
                        )
                        .AddItem(new MenuItemDefinition(
                                AppPageNames.Common.Notifications_MassNotifications,
                                L("MassNotifications"),
                                url: "App/Notifications/MassNotifications",
                                icon: "flaticon-paper-plane",
                                permissionDependency: new SimplePermissionDependency(AppPermissions.Pages_Administration_MassNotification)
                            )
                        )
                    )
                    .AddItem(new MenuItemDefinition(
                            AppPageNames.Host.Settings,
                            L("Settings"),
                            url: "App/HostSettings",
                            icon: "flaticon-settings",
                            permissionDependency: new SimplePermissionDependency(AppPermissions
                                .Pages_Administration_Host_Settings)
                        )
                    )
                    .AddItem(new MenuItemDefinition(
                            AppPageNames.Tenant.Settings,
                            L("Settings"),
                            url: "App/Settings",
                            icon: "flaticon-settings",
                            permissionDependency: new SimplePermissionDependency(AppPermissions
                                .Pages_Administration_Tenant_Settings)
                        )
                    )
                ).AddItem(new MenuItemDefinition(
                        AppPageNames.Common.DemoUiComponents,
                        L("DemoUiComponents"),
                        url: "App/DemoUiComponents",
                        icon: "flaticon-shapes",
                        permissionDependency: new SimplePermissionDependency(AppPermissions.Pages_DemoUiComponents)
                    )
                );
        }

        private static ILocalizableString L(string name)
        {
            return new LocalizableString(name, SbinSolutionConsts.LocalizationSourceName);
        }
    }
}