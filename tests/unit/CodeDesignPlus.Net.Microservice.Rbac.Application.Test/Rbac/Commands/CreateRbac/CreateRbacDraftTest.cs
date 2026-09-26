using System.Collections.Generic;
using System.Linq;
using System.Threading;
using System.Threading.Tasks;
using CodeDesignPlus.Net.Microservice.Rbac.Application.Rbac.Commands.CreateRbac;
using CodeDesignPlus.Net.Microservice.Rbac.Domain.DomainEvents;
using CodeDesignPlus.Net.Microservice.Rbac.Domain.Enums;
using CodeDesignPlus.Net.Microservice.Rbac.Domain.ValueObjects;
using Xunit;

namespace CodeDesignPlus.Net.Microservice.Rbac.Application.Test.Rbac.Commands.CreateRbac;

/// <summary>
/// Solo una configuracion puede estar activa, pero una inactiva es un borrador y se crea aunque haya otra activa
/// (plan 032 de pendings). Antes, el estado del alta se ignoraba y todo RBAC nacia activo.
/// </summary>
public class CreateRbacDraftTest
{
    private readonly Mock<IRbacRepository> repository = new();
    private readonly Mock<IUserContext> user = new();
    private readonly Mock<IPubSub> pubsub = new();
    private readonly Mock<ICacheManager> cache = new();
    private readonly CreateRbacCommandHandler handler;

    public CreateRbacDraftTest()
    {
        user.SetupGet(x => x.IdUser).Returns(Guid.NewGuid());
        repository.Setup(x => x.ExistsAsync<RbacAggregate>(It.IsAny<Guid>(), It.IsAny<CancellationToken>())).ReturnsAsync(false);
        // Ya hay una configuracion activa.
        repository.Setup(x => x.HasActiveRbacAsync(It.IsAny<CancellationToken>())).ReturnsAsync(true);
        handler = new CreateRbacCommandHandler(repository.Object, user.Object, pubsub.Object, cache.Object);
    }

    private static List<RbacPermissionDto> OnePermission() =>
    [
        new()
        {
            Id = Guid.NewGuid(),
            Role = Role.Create(Guid.NewGuid(), "Revisor Fiscal"),
            Resource = Resource.Create(Guid.NewGuid(), "PruebaRbac", "ms-rbac", "Rbac", "GetRbac", HttpMethodEnum.GET),
        },
    ];

    [Fact]
    public async Task UnBorradorSeCreaInactivoAunqueHayaOtraActiva()
    {
        RbacAggregate? saved = null;
        repository.Setup(x => x.CreateAsync(It.IsAny<RbacAggregate>(), It.IsAny<CancellationToken>()))
            .Callback<RbacAggregate, CancellationToken>((rbac, _) => saved = rbac)
            .Returns(Task.CompletedTask);

        await handler.Handle(new CreateRbacCommand(Guid.NewGuid(), "Borrador", "Prueba", false, OnePermission()), CancellationToken.None);

        Assert.NotNull(saved);
        Assert.False(saved!.IsActive);
        repository.Verify(x => x.HasActiveRbacAsync(It.IsAny<CancellationToken>()), Times.Never);
    }

    [Fact]
    public async Task ElEventoDeCreacionLlevaElEstadoReal()
    {
        RbacAggregate? saved = null;
        repository.Setup(x => x.CreateAsync(It.IsAny<RbacAggregate>(), It.IsAny<CancellationToken>()))
            .Callback<RbacAggregate, CancellationToken>((rbac, _) => saved = rbac)
            .Returns(Task.CompletedTask);
        IReadOnlyList<IDomainEvent>? published = null;
        pubsub.Setup(x => x.PublishAsync(It.IsAny<IReadOnlyList<IDomainEvent>>(), It.IsAny<CancellationToken>()))
            .Callback<IReadOnlyList<IDomainEvent>, CancellationToken>((events, _) => published = events)
            .Returns(Task.CompletedTask);

        await handler.Handle(new CreateRbacCommand(Guid.NewGuid(), "Borrador", "Prueba", false, OnePermission()), CancellationToken.None);

        var created = Assert.Single(published!.OfType<RbacCreatedDomainEvent>());
        Assert.False(created.IsActive);
    }

    [Fact]
    public async Task UnaActivaConOtraActivaSigueRechazandose()
    {
        var exception = await Assert.ThrowsAsync<CodeDesignPlusException>(() =>
            handler.Handle(new CreateRbacCommand(Guid.NewGuid(), "Otra", "Prueba", true, OnePermission()), CancellationToken.None));

        Assert.Equal(Errors.RbacActive.GetCode(), exception.Code);
    }
}
