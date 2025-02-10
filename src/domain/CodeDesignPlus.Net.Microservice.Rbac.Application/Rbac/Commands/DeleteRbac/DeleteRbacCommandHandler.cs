namespace CodeDesignPlus.Net.Microservice.Rbac.Application.Rbac.Commands.DeleteRbac;

public class DeleteRbacCommandHandler(IRbacRepository repository, IUserContext user, IPubSub pubsub) : IRequestHandler<DeleteRbacCommand>
{
    public async Task Handle(DeleteRbacCommand request, CancellationToken cancellationToken)
    {
        ApplicationGuard.IsNull(request, Errors.InvalidRequest);

        var aggregate = await repository.FindAsync<RbacAggregate>(request.Id, cancellationToken);

        ApplicationGuard.IsNull(aggregate, Errors.RbacNotFound);

        aggregate.Delete(user.IdUser);

        await repository.DeleteAsync<RbacAggregate>(aggregate.Id, cancellationToken);

        await pubsub.PublishAsync(aggregate.GetAndClearEvents(), cancellationToken);
    }
}