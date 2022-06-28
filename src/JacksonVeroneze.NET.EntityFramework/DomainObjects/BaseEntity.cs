namespace JacksonVeroneze.NET.EntityFramework.DomainObjects;

public abstract class BaseEntity<TType>
{
    public TType Id { get; set; }

    public DateTime CreatedAt { get; } = DateTime.Now;

    public DateTime? UpdatedAt { get; set; }

    public DateTime? DeletedAt { get; set; }

    public int Version { get; set; } = 1;

    public Guid TenantId { get; set; }

    public override string ToString()
        => $"{GetType().Name}: Id: {Id}, CreatedAt: {CreatedAt}, " +
           $"UpdatedAt: {UpdatedAt}, DeletedAt: {DeletedAt}, " +
           $"Version: {Version}, TenantId: {TenantId}";
}