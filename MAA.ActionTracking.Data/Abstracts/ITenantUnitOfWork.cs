using System.Threading.Tasks;

namespace MAA.ActionTracking.Data.Abstracts
{
    public interface ITenantUnitOfWork
    {
        void Save();
        Task SaveAsync();

    }
}
