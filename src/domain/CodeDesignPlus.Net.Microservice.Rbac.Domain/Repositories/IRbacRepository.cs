using CodeDesignPlus.Net.Microservice.Rbac.Domain.Entities;

namespace CodeDesignPlus.Net.Microservice.Rbac.Domain.Repositories;

public interface IRbacRepository : IRepositoryBase
{
    Task<bool> HasActiveRbacAsync(CancellationToken cancellationToken);
    Task<bool> HasActiveRbacAsync(Guid id, CancellationToken cancellationToken);
    Task<List<RbacPermissionEntity>> GetPermissionsByMicroserviceAsync(string microservice, CancellationToken cancellationToken);
}