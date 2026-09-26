namespace CodeDesignPlus.Net.Microservice.Rbac.Application.Rbac;

/// <summary>
/// Las claves de cache que dependen de un RBAC, y su invalidacion.
/// </summary>
/// <remarks>
/// Hay dos lecturas cacheadas: el detalle, por el id del RBAC (<c>GetRbacByIdQueryHandler</c>), y los permisos de cada
/// microservicio, por su nombre (<c>GetRbacByMicroserviceQueryHandler</c>, la que consume el SDK para autorizar).
/// Toda escritura tiene que borrar las dos: la del RBAC y la de cada micro que tenia o tiene un permiso. Un permiso
/// quitado tambien cambia la lista del micro que lo tenia, por eso se toman las claves antes y despues del cambio
/// (plan 030 de pendings). Sin esto, el detalle mostraba datos viejos durante la expiracion de la cache y un RBAC
/// borrado seguia respondiendo.
/// </remarks>
public static class RbacCache
{
    /// <summary>
    /// El id del RBAC y los microservicios de sus permisos.
    /// </summary>
    public static IEnumerable<string> Keys(RbacAggregate rbac)
        => rbac.Permissions
            .Select(x => x.Resource.Service)
            .Prepend(rbac.Id.ToString());

    /// <summary>
    /// Borra cada clave una sola vez.
    /// </summary>
    public static async Task InvalidateAsync(this ICacheManager cacheManager, IEnumerable<string> keys)
    {
        foreach (var key in keys.Where(x => !string.IsNullOrWhiteSpace(x)).Distinct())
            await cacheManager.RemoveAsync(key);
    }
}
