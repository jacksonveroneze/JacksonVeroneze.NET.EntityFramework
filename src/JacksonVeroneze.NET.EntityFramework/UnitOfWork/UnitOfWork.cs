using JacksonVeroneze.NET.EntityFramework.Interfaces;
using Microsoft.EntityFrameworkCore;

namespace JacksonVeroneze.NET.EntityFramework.UnitOfWork;

public class BaseUnitOfWork : IUnitOfWork
{
    private readonly DbContext _options;

    public BaseUnitOfWork(DbContext options)
    {
        _options = options;
    }

    public async Task<bool> CommitAsync()
    {
        int total = await _options.SaveChangesAsync();

        return total > 0;
    }
}