namespace CodeDesignPlus.Net.Microservice.Rbac.Application.Rbac.DataTransferObjects;

public class RbacDto: IDtoBase
{
    public required Guid Id { get; set; }

    public required string Name { get; set; }

    public required string Description { get; set; }

    public required List<RbacPermissionDto> Permissions { get; set; } = [];
    
    public required bool IsActive { get; set; } 
}