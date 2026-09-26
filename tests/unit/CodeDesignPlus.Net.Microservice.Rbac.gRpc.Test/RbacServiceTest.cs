using CodeDesignPlus.Net.Microservice.Rbac.Domain.Enums;
using CodeDesignPlus.Net.Microservice.Rbac.gRpc;

namespace CodeDesignPlus.Net.Microservice.Rbac.gRpc.Test;

/// <summary>
/// El dominio y el gRPC numeran los verbos distinto; el gRPC tiene que traducir, no castear (plan 031 de pendings).
/// </summary>
public class RbacServiceTest
{
    [Theory]
    [InlineData(HttpMethodEnum.GET, HttpMethod.Get)]
    [InlineData(HttpMethodEnum.POST, HttpMethod.Post)]
    [InlineData(HttpMethodEnum.PUT, HttpMethod.Put)]
    [InlineData(HttpMethodEnum.PATCH, HttpMethod.Patch)]
    [InlineData(HttpMethodEnum.DELETE, HttpMethod.Delete)]
    [InlineData(HttpMethodEnum.None, HttpMethod.None)]
    public void ToProto_TraduceCadaVerbo(HttpMethodEnum domain, HttpMethod expected)
    {
        Assert.Equal(expected, RbacService.ToProto(domain));
    }

    [Fact]
    public async Task GetRbac_EnviaLosGetComoGet()
    {
        var mediator = new Mock<IMediator>();
        mediator
            .Setup(x => x.Send(It.IsAny<Application.Rbac.Queries.GetRbacByMicroservice.GetRbacByMicroserviceQuery>(), It.IsAny<CancellationToken>()))
            .ReturnsAsync([new RbacResourceDto { Role = "20000000-0000-0000-0000-000000000008", Module = "PruebaRbac", Controller = "Rbac", Action = "GetRbac", Method = HttpMethodEnum.GET }]);

        var response = await new RbacService(mediator.Object).GetRbac(new GetRbacRequest { Microservice = "ms-rbac" }, null!);

        var resource = Assert.Single(response.Resources);
        Assert.Equal(HttpMethod.Get, resource.Method);
    }
}
