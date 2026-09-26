namespace CodeDesignPlus.Net.Microservice.Rbac.Application.Rbac.Commands.CreateRbac;

public class CreateRbacCommandHandler(IRbacRepository repository, IUserContext user, IPubSub pubsub, ICacheManager cacheManager) : IRequestHandler<CreateRbacCommand>
{
    public async Task Handle(CreateRbacCommand request, CancellationToken cancellationToken)
    {
        ApplicationGuard.IsNull(request, Errors.InvalidRequest);
        
        var exist = await repository.ExistsAsync<RbacAggregate>(request.Id, cancellationToken);

        ApplicationGuard.IsTrue(exist, Errors.RbacAlreadyExists);

        // Solo puede haber una configuracion activa; una inactiva es un borrador y se crea aunque haya otra (plan 032).
        if (request.IsActive)
        {
            var existRbacActive = await repository.HasActiveRbacAsync(cancellationToken);

            ApplicationGuard.IsTrue(existRbacActive, Errors.RbacActive);
        }

        var rbac = RbacAggregate.Create(request.Id, request.Name, request.Description, request.IsActive, user.IdUser);

        foreach (var permission in request.RbacPermissions)
        {
            rbac.AddPermission(permission.Id, permission.Role, permission.Resource, user.IdUser);
        }

        await repository.CreateAsync(rbac, cancellationToken);

        // Un micro que consulto antes de existir el RBAC tiene cacheada su lista vacia.
        await cacheManager.InvalidateAsync(RbacCache.Keys(rbac));

        await pubsub.PublishAsync(rbac.GetAndClearEvents(), cancellationToken);
    }
}