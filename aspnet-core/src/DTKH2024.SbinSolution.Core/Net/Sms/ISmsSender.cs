using System.Threading.Tasks;

namespace DTKH2024.SbinSolution.Net.Sms
{
    public interface ISmsSender
    {
        Task SendAsync(string number, string message);
    }
}