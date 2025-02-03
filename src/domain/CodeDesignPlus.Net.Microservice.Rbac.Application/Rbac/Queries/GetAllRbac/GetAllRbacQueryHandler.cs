namespace CodeDesignPlus.Net.Microservice.Rbac.Application.Rbac.Queries.GetAllRbac;

public class GetAllRbacQueryHandler(IRbacRepository repository, IMapper mapper, IUserContext user) : IRequestHandler<GetAllRbacQuery, RbacDto>
{
    public Task<RbacDto> Handle(GetAllRbacQuery request, CancellationToken cancellationToken)
    {
        return Task.FromResult<RbacDto>(default!);
    }
}
