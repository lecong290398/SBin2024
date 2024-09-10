using Abp.AutoMapper;
using DTKH2024.SbinSolution.MultiTenancy.Payments.Dto;

namespace DTKH2024.SbinSolution.Web.Areas.App.Models.SubscriptionManagement;

[AutoMapFrom(typeof(SubscriptionPaymentProductDto))]
public class ShowDetailModalViewModel : SubscriptionPaymentProductDto
{
}