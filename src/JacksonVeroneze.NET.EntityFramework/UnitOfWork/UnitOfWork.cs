using JacksonVeroneze.NET.EntityFramework.Extensions;
using JacksonVeroneze.NET.EntityFramework.Interfaces;
using Microsoft.EntityFrameworkCore;

namespace JacksonVeroneze.NET.EntityFramework.UnitOfWork;

public class BaseUnitOfWork : IUnitOfWork
{
    protected readonly ILogger<BaseUnitOfWork> _logger;

    private readonly DbContext _options;

    public BaseUnitOfWork(ILogger<BaseUnitOfWork> logger,
        DbContext options)
    {
        _logger = logger;
        _options = options;
    }

    public async Task<bool> CommitAsync()
    {
        int total = await _options.SaveChangesAsync();

        bool success = total > 0;

        _logger.LogCommit(nameof(BaseUnitOfWork),
            nameof(CommitAsync),
            success);

        return success;
    }
}