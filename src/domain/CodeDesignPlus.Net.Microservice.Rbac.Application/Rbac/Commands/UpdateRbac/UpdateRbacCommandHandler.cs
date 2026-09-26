namespace CodeDesignPlus.Net.Microservice.Rbac.Application.Rbac.Commands.UpdateRbac;

public class UpdateRbacCommandHandler(IRbacRepository repository, IUserContext user, IPubSub pubsub, ICacheManager cacheManager) : IRequestHandler<UpdateRbacCommand>
{
    public async Task Handle(UpdateRbacCommand request, CancellationToken cancellationToken)
    {
        ApplicationGuard.IsNull(request, Errors.InvalidRequest);

        var rbac = await repository.FindAsync<RbacAggregate>(request.Id, cancellationToken);

        ApplicationGuard.IsNull(rbac, Errors.RbacNotFound);

        if (request.IsActive)
        {
            var existRbacActive = await repository.HasActiveRbacAsync(request.Id, cancellationToken);

            ApplicationGuard.IsTrue(existRbacActive, Errors.RbacActive);
        }

        // Las claves de antes del cambio: un permiso quitado tambien cambia la lista de su micro.
        var keysBefore = RbacCache.Keys(rbac).ToList();

        rbac.Update(request.Name, request.Description, request.IsActive, user.IdUser);

        // La pantalla envia la lista completa de lo marcado; un permiso recien marcado trae un id nuevo. Por eso se
        // reemplaza el conjunto y no se actualiza permiso por permiso (plan 029 de pendings).
        rbac.ReplacePermissions([.. request.RbacPermissions.Select(x => (x.Id, x.Role, x.Resource))], user.IdUser);

        await repository.UpdateAsync(rbac, cancellationToken);

        await cacheManager.InvalidateAsync(keysBefore.Concat(RbacCache.Keys(rbac)));

        await pubsub.PublishAsync(rbac.GetAndClearEvents(), cancellationToken);
    }
}