using CodeDesignPlus.Net.Microservice.Rbac.Domain.ValueObjects;

namespace CodeDesignPlus.Net.Microservice.Rbac.Domain.DomainEvents;

[EventKey<RbacAggregate>(1, "PermissionRemovedDomainEvent")]
public class PermissionRemovedDomainEvent(
    Guid aggregateId,
    Guid idRbacPermission, 
    Role role, 
    Resource resource,
    Guid? eventId = null,
    Instant? occurredAt = null,
    Dictionary<string, object>? metadata = null
) : DomainEvent(aggregateId, eventId, occurredAt, metadata)
{
    public Guid IdRbacPermission { get; private set; } = idRbacPermission;

    public Role Role { get; private set; } = role;

    public Resource Resource { get; private set; } = resource;

    public static PermissionRemovedDomainEvent Create(Guid aggregateId, Guid idRbacPermission, Role role, Resource resource)
    {
        return new PermissionRemovedDomainEvent(aggregateId, idRbacPermission, role, resource);
    }
}
