namespace CodeDesignPlus.Net.Microservice.Rbac.Application.Rbac.Queries.GetRbacById;

public class GetRbacByIdQueryHandler(IRbacRepository repository, IMapper mapper, ICacheManager cacheManager) : IRequestHandler<GetRbacByIdQuery, RbacDto>
{
    public async Task<RbacDto> Handle(GetRbacByIdQuery request, CancellationToken cancellationToken)
    {
        ApplicationGuard.IsNull(request, Errors.InvalidRequest);

        var exists = await cacheManager.ExistsAsync(request.Id.ToString());

        if (exists)
            return await cacheManager.GetAsync<RbacDto>(request.Id.ToString());

        var rbac = await repository.FindAsync<RbacAggregate>(request.Id, cancellationToken);

        ApplicationGuard.IsNull(rbac, Errors.RbacNotFound);

        var dto = mapper.Map<RbacDto>(rbac);

        await cacheManager.SetAsync(request.Id.ToString(), dto);

        return dto;
    }
}
