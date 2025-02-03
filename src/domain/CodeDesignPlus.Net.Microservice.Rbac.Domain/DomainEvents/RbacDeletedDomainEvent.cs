using CodeDesignPlus.Net.Microservice.Rbac.Domain.Entities;

namespace CodeDesignPlus.Net.Microservice.Rbac.Domain.DomainEvents;

[EventKey<RbacAggregate>(1, "RbacDeletedDomainEvent")]
public class RbacDeletedDomainEvent(
    Guid aggregateId,
    string name,
    string description,
    List<RbacPermissionEntity> permissions,
    bool isActive,
    Guid? eventId = null,
    Instant? occurredAt = null,
    Dictionary<string, object>? metadata = null
) : DomainEvent(aggregateId, eventId, occurredAt, metadata)
{
    public string Name { get; private set; } = name;

    public string Description { get; private set; } = description;

    public List<RbacPermissionEntity> Permissions { get; private set; } = permissions;

    public bool IsActive { get; private set; } = isActive;

    public static RbacCreatedDomainEvent Create(Guid aggregateId, string name, string description, List<RbacPermissionEntity> permissions, bool isActive)
    {
        return new RbacCreatedDomainEvent(aggregateId, name, description, permissions, isActive);
    }
}
