namespace CodeDesignPlus.Net.Microservice.Rbac.Application.Rbac.Queries.GetRbacById;

public record GetRbacByIdQuery(Guid Id) : IRequest<RbacDto>;

