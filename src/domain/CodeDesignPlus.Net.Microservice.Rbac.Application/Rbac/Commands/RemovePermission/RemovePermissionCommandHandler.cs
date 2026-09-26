namespace CodeDesignPlus.Net.Microservice.Rbac.Application.Rbac.Commands.RemovePermission;

public class RemovePermissionCommandHandler(IRbacRepository repository, IUserContext user, IPubSub pubsub, ICacheManager cacheManager) : IRequestHandler<RemovePermissionCommand>
{
    public async Task Handle(RemovePermissionCommand request, CancellationToken cancellationToken)
    {
        ApplicationGuard.IsNull(request, Errors.InvalidRequest);

        var rbac = await repository.FindAsync<RbacAggregate>(request.Id, cancellationToken);

        ApplicationGuard.IsNull(rbac, Errors.RbacNotFound);

        // Las claves de antes: el permiso quitado puede ser el ultimo de su micro.
        var keysBefore = RbacCache.Keys(rbac).ToList();

        rbac.RemovePermission(request.IdRbacPermission, user.IdUser);

        await repository.UpdateAsync(rbac, cancellationToken);

        await cacheManager.InvalidateAsync(keysBefore);

        await pubsub.PublishAsync(rbac.GetAndClearEvents(), cancellationToken);  
    }
}