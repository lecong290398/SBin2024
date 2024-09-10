using System.Threading.Tasks;
using Abp.Application.Services.Dto;
using DTKH2024.SbinSolution.MultiTenancy.Accounting.Dto;

namespace DTKH2024.SbinSolution.MultiTenancy.Accounting
{
    public interface IInvoiceAppService
    {
        Task<InvoiceDto> GetInvoiceInfo(EntityDto<long> input);

        Task CreateInvoice(CreateInvoiceDto input);
    }
}
