using System.Threading.Tasks;

namespace MAA.ActionTracking.WebHost.Services
{
    public interface ISmsSender
    {
        Task SendSmsAsync(string number, string message);
    }
}
