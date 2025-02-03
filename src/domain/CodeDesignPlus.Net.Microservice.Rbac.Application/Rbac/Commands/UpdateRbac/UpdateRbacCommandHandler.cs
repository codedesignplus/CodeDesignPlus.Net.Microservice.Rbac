namespace CodeDesignPlus.Net.Microservice.Rbac.Application.Rbac.Commands.UpdateRbac;

public class UpdateRbacCommandHandler(IRbacRepository repository, IUserContext user, IPubSub pubsub) : IRequestHandler<UpdateRbacCommand>
{
    public Task Handle(UpdateRbacCommand request, CancellationToken cancellationToken)
    {
        return Task.CompletedTask;
    }
}