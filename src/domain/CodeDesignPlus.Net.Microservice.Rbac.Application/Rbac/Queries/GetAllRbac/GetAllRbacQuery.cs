namespace CodeDesignPlus.Net.Microservice.Rbac.Application.Rbac.Queries.GetAllRbac;

public record GetAllRbacQuery(Guid Id) : IRequest<RbacDto>;

