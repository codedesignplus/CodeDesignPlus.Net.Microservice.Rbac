using System.Threading;
using System.Threading.Tasks;
using CodeDesignPlus.Net.Microservice.Rbac.Application.Rbac.Commands.UpdateRbac;
using CodeDesignPlus.Net.Microservice.Rbac.Domain.DomainEvents;
using CodeDesignPlus.Net.Microservice.Rbac.Domain.ValueObjects;
using Moq;
using Xunit;

namespace CodeDesignPlus.Net.Microservice.Rbac.Application.Test.Rbac.Commands.UpdateRbac;

public class UpdateRbacCommandHandlerTest
{
    private readonly Mock<IRbacRepository> repositoryMock;
    private readonly Mock<IUserContext> userContextMock;
    private readonly Mock<IPubSub> pubSubMock;
    private readonly UpdateRbacCommandHandler handler;

    public UpdateRbacCommandHandlerTest()
    {
        repositoryMock = new Mock<IRbacRepository>();
        userContextMock = new Mock<IUserContext>();
        pubSubMock = new Mock<IPubSub>();
        handler = new UpdateRbacCommandHandler(repositoryMock.Object, userContextMock.Object, pubSubMock.Object);
    }

    [Fact]
    public async Task Handle_RequestIsNull_ThrowsCodeDesignPlusException()
    {
        // Arrange
        UpdateRbacCommand request = null!;
        var cancellationToken = CancellationToken.None;

        // Act & Assert
        var exception = await Assert.ThrowsAsync<CodeDesignPlusException>(() => handler.Handle(request, cancellationToken));

        Assert.Equal(Errors.InvalidRequest.GetMessage(), exception.Message);
        Assert.Equal(Errors.InvalidRequest.GetCode(), exception.Code);
        Assert.Equal(Layer.Application, exception.Layer);
    }

    [Fact]
    public async Task Handle_RbacNotFound_ThrowsCodeDesignPlusException()
    {
        // Arrange
        var role = Role.Create(Guid.NewGuid(), "Admin");
        var resource = Resource.Create(Guid.NewGuid(), "Custom Module", "Custom Service", "Custom Controller", "Custom Action", Domain.Enums.HttpMethodEnum.PUT);
        var rbacPermissions = new List<RbacPermissionDto>
        {
            new () {
                Id = Guid.NewGuid(),
                Role = role,
                Resource = resource
            }
        };
        var request = new UpdateRbacCommand(Guid.NewGuid(), "Test Name", "Test Description", true, rbacPermissions);
        var cancellationToken = CancellationToken.None;

        repositoryMock
            .Setup(repo => repo.FindAsync<RbacAggregate>(request.Id, cancellationToken))
            .ReturnsAsync((RbacAggregate)null!);

        // Act & Assert
        var exception = await Assert.ThrowsAsync<CodeDesignPlusException>(() => handler.Handle(request, cancellationToken));

        Assert.Equal(Errors.RbacNotFound.GetMessage(), exception.Message);
        Assert.Equal(Errors.RbacNotFound.GetCode(), exception.Code);
        Assert.Equal(Layer.Application, exception.Layer);
    }

    [Fact]
    public async Task Handle_ValidRequest_UpdatesRbacAndPublishesEvents()
    {
        // Arrange
        var idRbacPermission = Guid.NewGuid();
        var role = Role.Create(Guid.NewGuid(), "Admin");
        var resource = Resource.Create(Guid.NewGuid(), "Custom Module", "Custom Service", "Custom Controller", "Custom Action", Domain.Enums.HttpMethodEnum.PUT);
        var rbacPermissions = new List<RbacPermissionDto>
        {
            new () {
                Id = idRbacPermission,
                Role = role,
                Resource = resource
            }
        };
        var request = new UpdateRbacCommand(Guid.NewGuid(), "Test Name", "Test Description", true, rbacPermissions);
        var cancellationToken = CancellationToken.None;
        var rbac = RbacAggregate.Create(request.Id, "Name", "Description", Guid.NewGuid());

        rbac.AddPermission(idRbacPermission, role, resource, Guid.NewGuid());

        repositoryMock
            .Setup(repo => repo.FindAsync<RbacAggregate>(request.Id, cancellationToken))
            .ReturnsAsync(rbac);

        repositoryMock
            .Setup(x => x.HasActiveRbacAsync(userContextMock.Object.IdUser, cancellationToken))
            .ReturnsAsync(false);

        userContextMock.SetupGet(user => user.IdUser).Returns(Guid.NewGuid());

        // Act
        await handler.Handle(request, cancellationToken);

        // Assert
        repositoryMock.Verify(repo => repo.UpdateAsync(rbac, cancellationToken), Times.Once);
        pubSubMock.Verify(pubsub => pubsub.PublishAsync(It.IsAny<List<RbacUpdatedDomainEvent>>(), cancellationToken), Times.AtMostOnce);
    }
}
