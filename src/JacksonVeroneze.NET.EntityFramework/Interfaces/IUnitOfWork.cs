namespace JacksonVeroneze.NET.EntityFramework.Interfaces;

public interface IUnitOfWork
{
    Task<bool> CommitAsync();
}