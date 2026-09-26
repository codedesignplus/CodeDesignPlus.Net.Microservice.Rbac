using System.Collections.Generic;
using System.Linq;
using System.Threading;
using System.Threading.Tasks;
using CodeDesignPlus.Net.Microservice.Rbac.Application.Rbac.Commands.AddPermission;
using CodeDesignPlus.Net.Microservice.Rbac.Application.Rbac.Commands.CreateRbac;
using CodeDesignPlus.Net.Microservice.Rbac.Application.Rbac.Commands.DeleteRbac;
using CodeDesignPlus.Net.Microservice.Rbac.Application.Rbac.Commands.RemovePermission;
using CodeDesignPlus.Net.Microservice.Rbac.Application.Rbac.Commands.UpdateRbac;
using CodeDesignPlus.Net.Microservice.Rbac.Domain.Enums;
using CodeDesignPlus.Net.Microservice.Rbac.Domain.ValueObjects;
using Xunit;

namespace CodeDesignPlus.Net.Microservice.Rbac.Application.Test.Rbac.Commands;

/// <summary>
/// Toda escritura de un RBAC borra la cache del detalle (su id) y la de cada microservicio de sus permisos, antes y
/// despues del cambio (plan 030 de pendings). Y editar reemplaza el conjunto de permisos (plan 029).
/// </summary>
public class RbacCacheInvalidationTest
{
    private readonly Mock<IRbacRepository> repository = new();
    private readonly Mock<IUserContext> user = new();
    private readonly Mock<IPubSub> pubsub = new();
    private readonly Mock<ICacheManager> cache = new();
    private readonly List<string> removedKeys = [];

    private static readonly Role Auditor = Role.Create(Guid.NewGuid(), "Revisor Fiscal");

    public RbacCacheInvalidationTest()
    {
        user.SetupGet(x => x.IdUser).Returns(Guid.NewGuid());
        cache.Setup(x => x.RemoveAsync(It.IsAny<string>())).Callback<string>(removedKeys.Add).Returns(Task.CompletedTask);
    }

    private static Resource Endpoint(string service, string action)
        => Resource.Create(Guid.NewGuid(), "PruebaRbac", service, "Rbac", action, HttpMethodEnum.GET);

    private RbacAggregate ExistingRbac(params (Guid Id, string Service, string Action)[] permissions)
    {
        var rbac = RbacAggregate.Create(Guid.NewGuid(), "PruebaGuia", "Prueba", Guid.NewGuid());
        foreach (var (id, service, action) in permissions)
            rbac.AddPermission(id, Auditor, Endpoint(service, action), Guid.NewGuid());
        rbac.GetAndClearEvents();

        repository.Setup(x => x.FindAsync<RbacAggregate>(rbac.Id, It.IsAny<CancellationToken>())).ReturnsAsync(rbac);
        return rbac;
    }

    [Fact]
    public async Task Update_ReemplazaLosPermisosYBorraLaCacheDeAntesYDeDespues()
    {
        var kept = Guid.NewGuid();
        var removed = Guid.NewGuid();
        var added = Guid.NewGuid();
        var rbac = ExistingRbac((kept, "ms-rbac", "GetRbac"), (removed, "ms-units", "GetUnits"));

        var request = new UpdateRbacCommand(rbac.Id, "PruebaGuia", "Editado", false,
        [
            new() { Id = kept, Role = Auditor, Resource = Endpoint("ms-rbac", "GetRbac") },
            new() { Id = added, Role = Auditor, Resource = Endpoint("ms-invoicing", "GetDocuments") },
        ]);

        await new UpdateRbacCommandHandler(repository.Object, user.Object, pubsub.Object, cache.Object)
            .Handle(request, CancellationToken.None);

        // 029: un permiso con id nuevo se agrega (antes fallaba con 117) y el que no vino se quita.
        Assert.Equal(new[] { added, kept }.OrderBy(x => x), rbac.Permissions.Select(x => x.Id).OrderBy(x => x));

        // 030: el detalle y los tres micros, incluido ms-units, que perdio su permiso.
        Assert.Equal(
            new[] { rbac.Id.ToString(), "ms-invoicing", "ms-rbac", "ms-units" }.OrderBy(x => x),
            removedKeys.OrderBy(x => x));
    }

    [Fact]
    public async Task Create_BorraLaListaCacheadaDeCadaMicro()
    {
        repository.Setup(x => x.ExistsAsync<RbacAggregate>(It.IsAny<Guid>(), It.IsAny<CancellationToken>())).ReturnsAsync(false);
        repository.Setup(x => x.HasActiveRbacAsync(It.IsAny<CancellationToken>())).ReturnsAsync(false);

        var id = Guid.NewGuid();
        var request = new CreateRbacCommand(id, "PruebaGuia", "Prueba", true,
        [
            new() { Id = Guid.NewGuid(), Role = Auditor, Resource = Endpoint("ms-rbac", "GetRbac") },
        ]);

        await new CreateRbacCommandHandler(repository.Object, user.Object, pubsub.Object, cache.Object)
            .Handle(request, CancellationToken.None);

        Assert.Contains("ms-rbac", removedKeys);
        Assert.Contains(id.ToString(), removedKeys);
    }

    [Fact]
    public async Task Delete_BorraElDetalleYLosMicros()
    {
        var rbac = ExistingRbac((Guid.NewGuid(), "ms-rbac", "GetRbac"));

        await new DeleteRbacCommandHandler(repository.Object, user.Object, pubsub.Object, cache.Object)
            .Handle(new DeleteRbacCommand(rbac.Id), CancellationToken.None);

        Assert.Equal(new[] { rbac.Id.ToString(), "ms-rbac" }.OrderBy(x => x), removedKeys.OrderBy(x => x));
    }

    [Fact]
    public async Task RemovePermission_BorraElMicroDelPermisoQuitado()
    {
        var removed = Guid.NewGuid();
        var rbac = ExistingRbac((Guid.NewGuid(), "ms-rbac", "GetRbac"), (removed, "ms-units", "GetUnits"));

        await new RemovePermissionCommandHandler(repository.Object, user.Object, pubsub.Object, cache.Object)
            .Handle(new RemovePermissionCommand(rbac.Id, removed), CancellationToken.None);

        Assert.Contains("ms-units", removedKeys);
        Assert.Contains(rbac.Id.ToString(), removedKeys);
    }

    [Fact]
    public async Task AddPermission_BorraElMicroDelPermisoNuevo()
    {
        var rbac = ExistingRbac((Guid.NewGuid(), "ms-rbac", "GetRbac"));

        await new AddPermissionCommandHandler(repository.Object, user.Object, pubsub.Object, cache.Object)
            .Handle(new AddPermissionCommand(rbac.Id, Guid.NewGuid(), Auditor, Endpoint("ms-units", "GetUnits")), CancellationToken.None);

        Assert.Contains("ms-units", removedKeys);
        Assert.Contains(rbac.Id.ToString(), removedKeys);
    }
}
