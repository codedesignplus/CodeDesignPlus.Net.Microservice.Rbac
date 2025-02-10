
using CodeDesignPlus.Net.Exceptions.Guards;
using CodeDesignPlus.Net.Microservice.Rbac.Application.Rbac.Queries.GetRbacByMicroservice;

namespace CodeDesignPlus.Net.Microservice.Rbac.gRpc.Services;

public class RbacService(IMediator mediator) : Rbac.RbacBase
{
    public async override Task<GetRbacResponse> GetRbac(GetRbacRequest request, ServerCallContext context)
    {
        var query = new GetRbacByMicroserviceQuery(request.Microservice);

        var result = await mediator.Send(query);

        InfrastructureGuard.IsNull(result, "301 : The resource was not found");

        var response = new GetRbacResponse();

        response.Resources.AddRange(result.Select(x =>
        {
            return new RbacResource
            {
                Role = x.Role,
                Module = x.Module,
                Action = x.Action,
                Controller = x.Controller,
                Method = (HttpMethod)x.Method,
            };
        }));

        return response;
    }
}
