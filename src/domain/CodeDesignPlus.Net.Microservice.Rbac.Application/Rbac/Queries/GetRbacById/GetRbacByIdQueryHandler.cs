namespace CodeDesignPlus.Net.Microservice.Rbac.Application.Rbac.Queries.GetRbacById;

public class GetRbacByIdQueryHandler(IRbacRepository repository, IMapper mapper, IUserContext user) : IRequestHandler<GetRbacByIdQuery, RbacDto>
{
    public Task<RbacDto> Handle(GetRbacByIdQuery request, CancellationToken cancellationToken)
    {
        return Task.FromResult<RbacDto>(default!);
    }
}
