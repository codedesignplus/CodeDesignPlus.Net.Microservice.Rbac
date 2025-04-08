namespace CodeDesignPlus.Net.Microservice.Rbac.Application.Rbac.Commands.CreateRbac;

public class CreateRbacCommandHandler(IRbacRepository repository, IUserContext user, IPubSub pubsub) : IRequestHandler<CreateRbacCommand>
{
    public async Task Handle(CreateRbacCommand request, CancellationToken cancellationToken)
    {
        ApplicationGuard.IsNull(request, Errors.InvalidRequest);
        
        var exist = await repository.ExistsAsync<RbacAggregate>(request.Id, cancellationToken);

        ApplicationGuard.IsTrue(exist, Errors.RbacAlreadyExists);

        var existRbacActive = await repository.HasActiveRbacAsync(cancellationToken);

        ApplicationGuard.IsTrue(existRbacActive, Errors.RbacActive);

        var rbac = RbacAggregate.Create(request.Id, request.Name, request.Description, user.IdUser);

        foreach (var permission in request.RbacPermissions)
        {
            rbac.AddPermission(permission.Id, permission.Role, permission.Resource, user.IdUser);
        }

        await repository.CreateAsync(rbac, cancellationToken);

        await pubsub.PublishAsync(rbac.GetAndClearEvents(), cancellationToken);
    }
}