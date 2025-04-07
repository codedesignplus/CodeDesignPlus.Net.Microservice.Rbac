using System;
using CodeDesignPlus.Net.Microservice.Rbac.Domain.ValueObjects;

namespace CodeDesignPlus.Net.Microservice.Rbac.Domain.DomainEvents;

[EventKey<RbacAggregate>(1, "PermissionUpdatedDomainEvent")]
public class PermissionUpdatedDomainEvent(
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

    public static PermissionUpdatedDomainEvent Create(Guid aggregateId, Guid idRbacPermission, Role role, Resource resource)
    {
        return new PermissionUpdatedDomainEvent(aggregateId, idRbacPermission, role, resource);
    }
}
