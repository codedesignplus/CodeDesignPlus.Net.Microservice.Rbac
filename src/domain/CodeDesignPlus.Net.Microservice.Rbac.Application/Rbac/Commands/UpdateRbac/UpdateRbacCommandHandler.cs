namespace CodeDesignPlus.Net.Microservice.Rbac.Application.Rbac.Commands.UpdateRbac;

public class UpdateRbacCommandHandler(IRbacRepository repository, IUserContext user, IPubSub pubsub) : IRequestHandler<UpdateRbacCommand>
{
    public async Task Handle(UpdateRbacCommand request, CancellationToken cancellationToken)
    {
        ApplicationGuard.IsNull(request, Errors.InvalidRequest);

        var rbac = await repository.FindAsync<RbacAggregate>(request.Id, cancellationToken);

        ApplicationGuard.IsNull(rbac, Errors.RbacNotFound);

        if(request.IsActive)
        {
            var existRbacActive = await repository.HasActiveRbacAsync(request.Id, cancellationToken);

            ApplicationGuard.IsFalse(existRbacActive, Errors.RbacActive);
        }

        rbac.Update(request.Name, request.Description, request.IsActive, user.IdUser);

        await repository.UpdateAsync(rbac, cancellationToken);

        await pubsub.PublishAsync(rbac.GetAndClearEvents(), cancellationToken);    
    }
}