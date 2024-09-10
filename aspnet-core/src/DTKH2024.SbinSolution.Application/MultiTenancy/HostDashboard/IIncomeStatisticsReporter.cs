using System;
using System.Collections.Generic;
using System.Threading.Tasks;
using DTKH2024.SbinSolution.MultiTenancy.HostDashboard.Dto;

namespace DTKH2024.SbinSolution.MultiTenancy.HostDashboard
{
    public interface IIncomeStatisticsService
    {
        Task<List<IncomeStastistic>> GetIncomeStatisticsData(DateTime startDate, DateTime endDate,
            ChartDateInterval dateInterval);
    }
}