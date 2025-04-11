using CodeDesignPlus.Net.Core.Abstractions.Models.Pager;

namespace CodeDesignPlus.Net.Microservice.Rbac.Application.Rbac.Queries.GetAllRbac;

public record GetAllRbacQuery(C.Criteria Criteria) : IRequest<Pagination<RbacDto>>;

