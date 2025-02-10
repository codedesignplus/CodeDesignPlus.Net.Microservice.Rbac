namespace CodeDesignPlus.Net.Microservice.Rbac.Application.Rbac.Queries.GetRbacByMicroservice;

public class GetRbacByMicroserviceQueryHandler(IRbacRepository repository, ICacheManager cacheManager, IMapper mapper) : IRequestHandler<GetRbacByMicroserviceQuery, List<RbacResourceDto>>
{
    public async Task<List<RbacResourceDto>> Handle(GetRbacByMicroserviceQuery request, CancellationToken cancellationToken)
    {
        ApplicationGuard.IsNull(request, Errors.InvalidRequest);

        var exist = await cacheManager.ExistsAsync(request.Microservice);

        if (exist)
            return await cacheManager.GetAsync<List<RbacResourceDto>>(request.Microservice);

        var permissions = await repository.GetPermissionsByMicroserviceAsync(request.Microservice, cancellationToken);

        var dto = mapper.Map<List<RbacResourceDto>>(permissions);

        await cacheManager.SetAsync(request.Microservice, dto);

        return dto;
    }
}
