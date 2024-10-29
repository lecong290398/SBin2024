using Abp.Auditing;
using Abp.Authorization;
using Abp.Domain.Repositories;
using DTKH2024.SbinSolution.Authorization;
using DTKH2024.SbinSolution.Authorization.Users;
using DTKH2024.SbinSolution.OrderHistories;
using DTKH2024.SbinSolution.ProductPromotions;
using DTKH2024.SbinSolution.Tenants.Dashboard.Dto;
using DTKH2024.SbinSolution.TransactionBins;
using System;

namespace DTKH2024.SbinSolution.Tenants.Dashboard
{
    [DisableAuditing]
    [AbpAuthorize(AppPermissions.Pages_Tenant_Dashboard)]
    public class TenantDashboardAppService : SbinSolutionAppServiceBase, ITenantDashboardAppService
    {
        private readonly IRepository<User, long> _userRepository;
        private readonly IRepository<OrderHistory> _orderHistoryRepository;
        private readonly IRepository<ProductPromotion> _productPromotionRepository;
        private readonly IRepository<TransactionBin> _transactionBinRepository;

        public TenantDashboardAppService(IRepository<User, long> userRepository,
            IRepository<OrderHistory> orderHistoryRepository
            , IRepository<ProductPromotion> productPromotionRepository
            , IRepository<TransactionBin> transactionBinRepository)
        {
            _orderHistoryRepository = orderHistoryRepository;
            _userRepository = userRepository;
            _productPromotionRepository = productPromotionRepository;
            _transactionBinRepository = transactionBinRepository;
        }
        public GetMemberActivityOutput GetMemberActivity()
        {
            return new GetMemberActivityOutput
            (
                DashboardRandomDataGenerator.GenerateMemberActivities()
            );
        }

        public GetDashboardDataOutput GetDashboardData(GetDashboardDataInput input)
        {
            var output = new GetDashboardDataOutput
            {
                TotalProfit = DashboardRandomDataGenerator.GetRandomInt(5000, 9000),
                NewFeedbacks = DashboardRandomDataGenerator.GetRandomInt(1000, 5000),
                NewOrders = DashboardRandomDataGenerator.GetRandomInt(100, 900),
                NewUsers = DashboardRandomDataGenerator.GetRandomInt(50, 500),
                SalesSummary = DashboardRandomDataGenerator.GenerateSalesSummaryData(input.SalesSummaryDatePeriod),
                Expenses = DashboardRandomDataGenerator.GetRandomInt(5000, 10000),
                Growth = DashboardRandomDataGenerator.GetRandomInt(5000, 10000),
                Revenue = DashboardRandomDataGenerator.GetRandomInt(1000, 9000),
                TotalSales = DashboardRandomDataGenerator.GetRandomInt(10000, 90000),
                TransactionPercent = DashboardRandomDataGenerator.GetRandomInt(10, 100),
                NewVisitPercent = DashboardRandomDataGenerator.GetRandomInt(10, 100),
                BouncePercent = DashboardRandomDataGenerator.GetRandomInt(10, 100),
                DailySales = DashboardRandomDataGenerator.GetRandomArray(30, 10, 50),
                ProfitShares = DashboardRandomDataGenerator.GetRandomPercentageArray(3)
            };

            return output;
        }

        public GetTopStatsOutput GetTopStats()
        {
            var firstDayOfMonth = new DateTime(DateTime.Now.Year, DateTime.Now.Month, 1);
            var lastDayOfMonth = firstDayOfMonth.AddMonths(1).AddDays(-1);
            var newUsersCount = _userRepository.Count(user => user.CreationTime >= firstDayOfMonth && user.CreationTime <= lastDayOfMonth);
            
            var newOrders =  _orderHistoryRepository.Count(oh =>
              oh.HistoryTypeId == AppConsts.HistoryType_DoiQua &&
              oh.CreationTime >= firstDayOfMonth &&
              oh.CreationTime <= lastDayOfMonth
            );

            var newFeedbacks =  _productPromotionRepository.Count(pp =>
               pp.CreationTime >= firstDayOfMonth &&
              pp.CreationTime <= lastDayOfMonth
            );

            var totalProfit = _transactionBinRepository.Count(pp =>
               pp.CreationTime >= firstDayOfMonth &&
              pp.CreationTime <= lastDayOfMonth
            );

            return new GetTopStatsOutput
            {
                TotalProfit = totalProfit,
                NewFeedbacks = newFeedbacks,
                NewOrders = newOrders,
                NewUsers = newUsersCount
            };
        }

        public GetProfitShareOutput GetProfitShare()
        {
            return new GetProfitShareOutput
            {
                ProfitShares = DashboardRandomDataGenerator.GetRandomPercentageArray(3)
            };
        }

        public GetDailySalesOutput GetDailySales()
        {
            return new GetDailySalesOutput
            {
                DailySales = DashboardRandomDataGenerator.GetRandomArray(30, 10, 50)
            };
        }

        public GetSalesSummaryOutput GetSalesSummary(GetSalesSummaryInput input)
        {
            var salesSummary = DashboardRandomDataGenerator.GenerateSalesSummaryData(input.SalesSummaryDatePeriod);
            return new GetSalesSummaryOutput(salesSummary)
            {
                Expenses = DashboardRandomDataGenerator.GetRandomInt(0, 3000),
                Growth = DashboardRandomDataGenerator.GetRandomInt(0, 3000),
                Revenue = DashboardRandomDataGenerator.GetRandomInt(0, 3000),
                TotalSales = DashboardRandomDataGenerator.GetRandomInt(0, 3000)
            };
        }

        public GetRegionalStatsOutput GetRegionalStats()
        {
            return new GetRegionalStatsOutput(
                DashboardRandomDataGenerator.GenerateRegionalStat()
            );
        }

        public GetGeneralStatsOutput GetGeneralStats()
        {
            return new GetGeneralStatsOutput
            {
                TransactionPercent = DashboardRandomDataGenerator.GetRandomInt(10, 100),
                NewVisitPercent = DashboardRandomDataGenerator.GetRandomInt(10, 100),
                BouncePercent = DashboardRandomDataGenerator.GetRandomInt(10, 100)
            };
        }
    }
}