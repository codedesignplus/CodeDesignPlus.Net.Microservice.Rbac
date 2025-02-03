namespace CodeDesignPlus.Net.Microservice.Rbac.Application.Rbac.Queries.GetAllRbac;

public class GetAllRbacQueryHandler(IRbacRepository repository, IMapper mapper) : IRequestHandler<GetAllRbacQuery, List<RbacDto>>
{
    public async Task<List<RbacDto>> Handle(GetAllRbacQuery request, CancellationToken cancellationToken)
    {
        ApplicationGuard.IsNull(request, Errors.InvalidRequest);

        var tenants = await repository.MatchingAsync<RbacAggregate>(request.Criteria, cancellationToken);

        return mapper.Map<List<RbacDto>>(tenants);
    }
}
