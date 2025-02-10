using CodeDesignPlus.Net.Microservice.Rbac.Domain.Enums;

namespace CodeDesignPlus.Net.Microservice.Rbac.Application.Rbac.DataTransferObjects;

public class RbacResourceDto : IDtoBase
{
    public Guid Id { get; set; }
    public string Role { get; set; } = null!;
    public string Module { get; set; } = null!;
    public string Service { get; set; } = null!;
    public string Controller { get; set; } = null!;
    public string Action { get; set; } = null!;
    public HttpMethodEnum Method { get; set; }
}
