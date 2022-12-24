using JacksonVeroneze.NET.EntityFramework.DatabaseContext;
using JacksonVeroneze.NET.EntityFramework.Interfaces;

namespace JacksonVeroneze.NET.EntityFramework.UnitOfWork;

public class BaseUnitOfWork : IUnitOfWork, IDisposable
{
    private readonly BaseDbContext _options;

    public BaseUnitOfWork(BaseDbContext options)
    {
        _options = options;
    }

    public async Task<bool> CommitAsync()
    {
        bool isSuccess = await _options.SaveChangesAsync() > 0;

        return isSuccess;
    }

    public void Dispose()
    {
        _options.Dispose();
        GC.SuppressFinalize(this);
    }
}