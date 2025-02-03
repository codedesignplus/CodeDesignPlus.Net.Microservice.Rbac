using CodeDesignPlus.Net.Microservice.Rbac.Domain.ValueObjects;

namespace CodeDesignPlus.Net.Microservice.Rbac.Application.Rbac.DataTransferObjects;

public class RbacPermissionDto : IDtoBase
{
    public Guid Id { get; set; }

    public required Role Role { get; set; }

    public required Resource Resource { get; set; }
}
