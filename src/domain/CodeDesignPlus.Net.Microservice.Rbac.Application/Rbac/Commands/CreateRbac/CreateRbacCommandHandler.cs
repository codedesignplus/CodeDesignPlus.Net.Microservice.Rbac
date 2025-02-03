namespace CodeDesignPlus.Net.Microservice.Rbac.Application.Rbac.Commands.CreateRbac;

public class CreateRbacCommandHandler(IRbacRepository repository, IUserContext user, IPubSub pubsub) : IRequestHandler<CreateRbacCommand>
{
    public Task Handle(CreateRbacCommand request, CancellationToken cancellationToken)
    {
        return Task.CompletedTask;
    }
}