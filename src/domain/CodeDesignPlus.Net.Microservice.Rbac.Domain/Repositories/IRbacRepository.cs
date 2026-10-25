using CodeDesignPlus.Net.Microservice.Rbac.Domain.Entities;

namespace CodeDesignPlus.Net.Microservice.Rbac.Domain.Repositories;

public interface IRbacRepository : IRepositoryBase
{
    Task<bool> HasActiveRbacAsync(CancellationToken cancellationToken);
    Task<bool> HasActiveRbacAsync(Guid id, CancellationToken cancellationToken);
    Task<List<RbacPermissionEntity>> GetPermissionsByMicroserviceAsync(string microservice, CancellationToken cancellationToken);

    /// <summary>
    /// Guarda el registro nuevo si su id no existe todavía (pendings/589, regla 66).
    /// </summary>
    /// <param name="rbac">El agregado nuevo.</param>
    /// <param name="cancellationToken">Token de cancelación.</param>
    /// <returns><c>false</c> si otro envío del mismo formulario ya lo guardó: el segundo choca contra la clave primaria.</returns>
    Task<bool> CreateIfAbsentAsync(RbacAggregate rbac, CancellationToken cancellationToken);
}