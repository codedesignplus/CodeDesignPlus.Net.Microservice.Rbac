namespace CodeDesignPlus.Net.Microservice.Rbac.Application.Rbac.Commands.RemovePermission;

public class RemovePermissionCommandHandler(IRbacRepository repository, IUserContext user, IPubSub pubsub) : IRequestHandler<RemovePermissionCommand>
{
    public async Task Handle(RemovePermissionCommand request, CancellationToken cancellationToken)
    {
        ApplicationGuard.IsNull(request, Errors.InvalidRequest);

        var rbac = await repository.FindAsync<RbacAggregate>(request.Id, cancellationToken);

        ApplicationGuard.IsNull(rbac, Errors.RbacNotFound);

        rbac.RemovePermission(request.IdRbacPermission, user.IdUser);

        await repository.UpdateAsync(rbac, cancellationToken);

        await pubsub.PublishAsync(rbac.GetAndClearEvents(), cancellationToken);  
    }
}