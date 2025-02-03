namespace CodeDesignPlus.Net.Microservice.Rbac.Application.Rbac.Commands.CreateRbac;

public class CreateRbacCommandHandler(IRbacRepository repository, IUserContext user, IPubSub pubsub) : IRequestHandler<CreateRbacCommand>
{
    public async Task Handle(CreateRbacCommand request, CancellationToken cancellationToken)
    {
        ApplicationGuard.IsNull(request, Errors.InvalidRequest);
        
        var exist = await repository.ExistsAsync<RbacAggregate>(request.Id, user.Tenant, cancellationToken);

        ApplicationGuard.IsTrue(exist, Errors.RbacAlreadyExists);

        var rbac = RbacAggregate.Create(request.Id, request.Name, request.Description, user.IdUser);

        await repository.CreateAsync(rbac, cancellationToken);

        await pubsub.PublishAsync(rbac.GetAndClearEvents(), cancellationToken);
    }
}