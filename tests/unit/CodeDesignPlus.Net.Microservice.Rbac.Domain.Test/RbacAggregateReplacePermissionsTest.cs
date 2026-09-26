using System.Collections.Generic;
using System.Linq;
using CodeDesignPlus.Net.Microservice.Rbac.Domain;
using CodeDesignPlus.Net.Microservice.Rbac.Domain.Enums;
using CodeDesignPlus.Net.Microservice.Rbac.Domain.ValueObjects;
using Xunit;

namespace CodeDesignPlus.Net.Microservice.Rbac.Domain.Test;

/// <summary>
/// Editar un RBAC envia la lista completa de permisos marcados, con un id nuevo para cada permiso recien marcado
/// (plan 029 de pendings). El agregado tiene que dejar su conjunto igual a esa lista.
/// </summary>
public class RbacAggregateReplacePermissionsTest
{
    private static readonly Role Auditor = Role.Create(Guid.NewGuid(), "Revisor Fiscal");

    private static Resource Endpoint(string service, string action, HttpMethodEnum method = HttpMethodEnum.GET)
        => Resource.Create(Guid.NewGuid(), "PruebaRbac", service, "Rbac", action, method);

    [Fact]
    public void ReplacePermissions_AgregaLosNuevosActualizaLosQueQuedanYQuitaLosQueFaltan()
    {
        var user = Guid.NewGuid();
        var rbac = RbacAggregate.Create(Guid.NewGuid(), "PruebaGuia", "Prueba", user);
        var kept = Guid.NewGuid();
        var removed = Guid.NewGuid();
        rbac.AddPermission(kept, Auditor, Endpoint("ms-rbac", "GetRbac"), user);
        rbac.AddPermission(removed, Auditor, Endpoint("ms-rbac", "UpdateRbac", HttpMethodEnum.PUT), user);
        rbac.GetAndClearEvents();

        var added = Guid.NewGuid();

        rbac.ReplacePermissions(
            [
                (kept, Auditor, Endpoint("ms-rbac", "GetRbac")),
                (added, Auditor, Endpoint("ms-rbac", "GetRbacById")),
            ],
            user);

        Assert.Equal([kept, added], rbac.Permissions.Select(x => x.Id).OrderBy(x => x == added).ToList());
        Assert.DoesNotContain(rbac.Permissions, x => x.Id == removed);

        var events = rbac.GetAndClearEvents();
        Assert.Contains(events, x => x is PermissionRemovedDomainEvent e && e.IdRbacPermission == removed);
        Assert.Contains(events, x => x is PermissionAddedDomainEvent e && e.IdRbacPermission == added);
        Assert.Contains(events, x => x is PermissionUpdatedDomainEvent e && e.IdRbacPermission == kept);
    }

    [Fact]
    public void ReplacePermissions_ConLaMismaListaNoCambiaElConjunto()
    {
        var user = Guid.NewGuid();
        var rbac = RbacAggregate.Create(Guid.NewGuid(), "PruebaGuia", "Prueba", user);
        var id = Guid.NewGuid();
        var resource = Endpoint("ms-rbac", "GetRbac");
        rbac.AddPermission(id, Auditor, resource, user);

        rbac.ReplacePermissions([(id, Auditor, resource)], user);

        var permission = Assert.Single(rbac.Permissions);
        Assert.Equal(id, permission.Id);
    }

    [Fact]
    public void ReplacePermissions_ConListaVaciaQuitaTodos()
    {
        var user = Guid.NewGuid();
        var rbac = RbacAggregate.Create(Guid.NewGuid(), "PruebaGuia", "Prueba", user);
        rbac.AddPermission(Guid.NewGuid(), Auditor, Endpoint("ms-rbac", "GetRbac"), user);
        rbac.AddPermission(Guid.NewGuid(), Auditor, Endpoint("ms-units", "GetUnits"), user);

        rbac.ReplacePermissions(new List<(Guid, Role, Resource)>(), user);

        Assert.Empty(rbac.Permissions);
    }
}
