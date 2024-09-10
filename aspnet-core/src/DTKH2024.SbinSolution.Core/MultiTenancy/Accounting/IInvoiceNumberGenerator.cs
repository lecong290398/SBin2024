using System.Threading.Tasks;
using Abp.Dependency;

namespace DTKH2024.SbinSolution.MultiTenancy.Accounting
{
    public interface IInvoiceNumberGenerator : ITransientDependency
    {
        Task<string> GetNewInvoiceNumber();
    }
}