using JacksonVeroneze.NET.EntityFramework.Context;
using JacksonVeroneze.NET.EntityFramework.Interfaces;

namespace JacksonVeroneze.NET.EntityFramework.UnitOfWork;

public class BaseUnitOfWork : IUnitOfWork, IDisposable
{
    private readonly DatabaseContext _options;

    public BaseUnitOfWork(DatabaseContext options)
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