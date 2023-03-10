namespace JacksonVeroneze.NET.EntityFramework.DomainObjects;

public abstract class BaseEntity<TKey>
{
    public TKey? Id { get; init; }

    public DateTime CreatedAt { get; init; } = DateTime.Now;

    public DateTime? UpdatedAt { get; set; }

    public DateTime? DeletedAt { get; set; }

    public int Version { get; set; } = 1;

    public Guid TenantId { get; init; }

    public override string ToString()
    {
        return $"{nameof(BaseEntity<TKey>)}: Id: {Id} - " +
               $"CreatedAt: {CreatedAt} - UpdatedAt: {UpdatedAt} - " +
               $"DeletedAt: {DeletedAt} - Version: {Version} - " +
               $"TenantId: {TenantId}";
    }
}