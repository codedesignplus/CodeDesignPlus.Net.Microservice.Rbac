
using CodeDesignPlus.Net.Exceptions.Guards;
using CodeDesignPlus.Net.Microservice.Rbac.Application.Rbac.Queries.GetRbacByMicroservice;
using CodeDesignPlus.Net.Microservice.Rbac.Domain.Enums;

namespace CodeDesignPlus.Net.Microservice.Rbac.gRpc.Services;

public class RbacService(IMediator mediator) : Rbac.RbacBase
{
    public async override Task<GetRbacResponse> GetRbac(GetRbacRequest request, ServerCallContext context)
    {
        var query = new GetRbacByMicroserviceQuery(request.Microservice);

        var result = await mediator.Send(query);

        InfrastructureGuard.IsNull(result, Infrastructure.Errors.ResourceNotFound);

        var response = new GetRbacResponse();

        response.Resources.AddRange(result.Select(x =>
        {
            return new RbacResource
            {
                Role = x.Role,
                Module = x.Module,
                Action = x.Action,
                Controller = x.Controller,
                Method = ToProto(x.Method),
            };
        }));

        return response;
    }

    /// <summary>
    /// Traduce el verbo del dominio al del gRPC. Los dos enums numeran distinto (en el dominio GET es 5 y PATCH es 4;
    /// en el proto GET es 4 y PATCH es 5), asi que un cast enviaba los GET con un numero que el SDK no conoce y los
    /// PATCH como GET (plan 031 de pendings).
    /// </summary>
    public static HttpMethod ToProto(HttpMethodEnum method) => method switch
    {
        HttpMethodEnum.GET => HttpMethod.Get,
        HttpMethodEnum.POST => HttpMethod.Post,
        HttpMethodEnum.PUT => HttpMethod.Put,
        HttpMethodEnum.PATCH => HttpMethod.Patch,
        HttpMethodEnum.DELETE => HttpMethod.Delete,
        _ => HttpMethod.None
    };
}
