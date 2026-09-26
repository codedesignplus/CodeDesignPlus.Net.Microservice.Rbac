using CodeDesignPlus.Net.Microservice.Rbac.Application.Setup;
using CodeDesignPlus.Net.Microservice.Rbac.Domain.Entities;
using CodeDesignPlus.Net.Microservice.Rbac.Domain.Enums;
using CodeDesignPlus.Net.Microservice.Rbac.Domain.ValueObjects;
using Xunit;

namespace CodeDesignPlus.Net.Microservice.Rbac.Application.Test.Setup;

/// <summary>
/// Lo que ms-rbac entrega al SDK para autorizar. El rol va como el id del catalogo de ms-roles, que es lo que devuelve
/// el directorio de roles; con el nombre no casaba nunca (plan 035 de pendings).
/// </summary>
public class RbacResourceMappingTest
{
    [Fact]
    public void ElPermisoSaleConElIdDelRolDelCatalogoYNoConSuNombre()
    {
        MapsterConfigRbac.Configure();
        var mapper = new Mapper(TypeAdapterConfig.GlobalSettings);

        var permission = new RbacPermissionEntity
        {
            Id = Guid.NewGuid(),
            Role = Role.Create(Guid.Parse("20000000-0000-0000-0000-000000000008"), "Revisor Fiscal"),
            Resource = Resource.Create(Guid.NewGuid(), "PruebaRbac", "ms-catalogs", "TypeDocument", "GetTypeDocuments", HttpMethodEnum.GET),
        };

        var dto = mapper.Map<RbacResourceDto>(permission);

        Assert.Equal("20000000-0000-0000-0000-000000000008", dto.Role);
        Assert.Equal("TypeDocument", dto.Controller);
        Assert.Equal(HttpMethodEnum.GET, dto.Method);
    }
}
