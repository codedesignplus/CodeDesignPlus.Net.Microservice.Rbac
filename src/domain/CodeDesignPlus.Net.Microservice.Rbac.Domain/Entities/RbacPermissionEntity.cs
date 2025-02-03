using CodeDesignPlus.Net.Microservice.Rbac.Domain.ValueObjects;

namespace CodeDesignPlus.Net.Microservice.Rbac.Domain.Entities;

public class RbacPermissionEntity : IEntityBase
{
    public Guid Id { get; set; }

    public required Role Role { get; set; }

    public required Resource Resource { get; set; }
}
