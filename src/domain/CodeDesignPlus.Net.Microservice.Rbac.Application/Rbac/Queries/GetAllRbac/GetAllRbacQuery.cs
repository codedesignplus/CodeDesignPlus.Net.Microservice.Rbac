namespace CodeDesignPlus.Net.Microservice.Rbac.Application.Rbac.Queries.GetAllRbac;

public record GetAllRbacQuery(C.Criteria Criteria) : IRequest<List<RbacDto>>;

