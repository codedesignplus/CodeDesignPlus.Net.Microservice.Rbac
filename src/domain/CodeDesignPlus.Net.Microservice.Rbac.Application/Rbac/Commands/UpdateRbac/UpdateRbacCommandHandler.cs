namespace CodeDesignPlus.Net.Microservice.Rbac.Application.Rbac.Commands.UpdateRbac;

public class UpdateRbacCommandHandler(IRbacRepository repository, IUserContext user, IPubSub pubsub) : IRequestHandler<UpdateRbacCommand>
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

        rbac.Update(request.Name, request.Description, request.IsActive, user.IdUser);

        foreach (var permission in request.RbacPermissions)
        {
            rbac.UpdatePermission(permission.Id, permission.Role, permission.Resource, user.IdUser);
        }

        await repository.UpdateAsync(rbac, cancellationToken);

        await pubsub.PublishAsync(rbac.GetAndClearEvents(), cancellationToken);
    }
}