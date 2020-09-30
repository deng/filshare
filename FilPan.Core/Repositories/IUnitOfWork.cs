using System.Threading.Tasks;

namespace FilPan.Repositories
{
    public interface IUnitOfWork
    {
        Task Commit();
    }
}
