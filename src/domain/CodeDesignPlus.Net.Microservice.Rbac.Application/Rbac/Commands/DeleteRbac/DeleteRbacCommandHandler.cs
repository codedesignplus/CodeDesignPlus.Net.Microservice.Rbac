namespace CodeDesignPlus.Net.Microservice.Rbac.Application.Rbac.Commands.DeleteRbac;

public class DeleteRbacCommandHandler(IRbacRepository repository, IUserContext user, IPubSub pubsub) : IRequestHandler<DeleteRbacCommand>
{
    public Task Handle(DeleteRbacCommand request, CancellationToken cancellationToken)
    {
        return Task.CompletedTask;
    }
}