using CodeDesignPlus.Net.Core.Abstractions.Models.Pager;

namespace CodeDesignPlus.Net.Microservice.Rbac.Application.Rbac.Queries.GetAllRbac;

public class GetAllRbacQueryHandler(IRbacRepository repository, IMapper mapper) : IRequestHandler<GetAllRbacQuery, Pagination<RbacDto>>
{
    public async Task<Pagination<RbacDto>> Handle(GetAllRbacQuery request, CancellationToken cancellationToken)
    {
        ApplicationGuard.IsNull(request, Errors.InvalidRequest);

        var tenants = await repository.MatchingAsync<RbacAggregate>(request.Criteria, cancellationToken);

        return mapper.Map<Pagination<RbacDto>>(tenants);
    }
}
