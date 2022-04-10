using System.Threading.Tasks;

namespace JacksonVeroneze.NET.EntityFramework.Data
{
    public interface IUnitOfWork
    {
        Task<bool> CommitAsync();
    }
}
