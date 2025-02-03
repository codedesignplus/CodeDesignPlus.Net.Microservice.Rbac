namespace CodeDesignPlus.Net.Microservice.Rbac.Application.Rbac.Commands.AddPermission;

public class AddPermissionCommandHandler(IRbacRepository repository, IUserContext user, IPubSub pubsub) : IRequestHandler<AddPermissionCommand>
{
    public async Task Handle(AddPermissionCommand request, CancellationToken cancellationToken)
    {
        ApplicationGuard.IsNull(request, Errors.InvalidRequest);

        var rbac = await repository.FindAsync<RbacAggregate>(request.Id, cancellationToken);

        ApplicationGuard.IsNull(rbac, Errors.RbacNotFound);

        rbac.AddPermission(request.IdRbacPermission, request.Role, request.Resource, user.IdUser);

        await repository.UpdateAsync(rbac, cancellationToken);

        await pubsub.PublishAsync(rbac.GetAndClearEvents(), cancellationToken);   
    }
}