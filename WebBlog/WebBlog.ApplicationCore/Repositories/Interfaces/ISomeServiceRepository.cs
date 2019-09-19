using System.Threading.Tasks;

namespace WebBlog.ApplicationCore.Repositories
{
    public interface ISomeServiceRepository<TEntity>
        where TEntity : class
    {
        Task<TEntity> GetData();
    }
}
