using CodeDesignPlus.Net.Microservice.Rbac.Application.Rbac.Commands.AddPermission;
using CodeDesignPlus.Net.Microservice.Rbac.Domain.DomainEvents;
using CodeDesignPlus.Net.Microservice.Rbac.Domain.ValueObjects;
using Microsoft.VisualStudio.TestPlatform.ObjectModel;

namespace CodeDesignPlus.Net.Microservice.Rbac.Application.Test.Rbac.Commands.AddPermission;

public class AddPermissionCommandHandlerTest
{
    private readonly Mock<IRbacRepository> repositoryMock;
    private readonly Mock<IUserContext> userContextMock;
    private readonly Mock<IPubSub> pubSubMock;
    private readonly AddPermissionCommandHandler handler;

    public AddPermissionCommandHandlerTest()
    {
        repositoryMock = new Mock<IRbacRepository>();
        userContextMock = new Mock<IUserContext>();
        pubSubMock = new Mock<IPubSub>();
        handler = new AddPermissionCommandHandler(repositoryMock.Object, userContextMock.Object, pubSubMock.Object);
    }

    [Fact]
    public async Task Handle_RequestIsNull_ThrowsCodeDesignPlusException()
    {
        // Arrange
        AddPermissionCommand request = null!;
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
        var request = new AddPermissionCommand(Guid.NewGuid(), Guid.NewGuid(), Role.Create(Guid.NewGuid(), "Admin"), Resource.Create(Guid.NewGuid(), "TestModule", "TestService", "TestController", "TestAction", Domain.Enums.HttpMethodEnum.GET));
        var cancellationToken = CancellationToken.None;

        repositoryMock
            .Setup(repo => repo.FindAsync<RbacAggregate>(request.Id, cancellationToken))
            .ReturnsAsync((RbacAggregate)null!);

        // Act & Assert
        var exception =await Assert.ThrowsAsync<CodeDesignPlusException>(() => handler.Handle(request, cancellationToken));

        
        Assert.Equal(Errors.RbacNotFound.GetMessage(), exception.Message);
        Assert.Equal(Errors.RbacNotFound.GetCode(), exception.Code);
        Assert.Equal(Layer.Application, exception.Layer);        
    }

    [Fact]
    public async Task Handle_ValidRequest_AddsPermissionAndPublishesEvents()
    {
        // Arrange
        var idUser = Guid.NewGuid();
        var request = new AddPermissionCommand(Guid.NewGuid(), Guid.NewGuid(), Role.Create(Guid.NewGuid(), "Admin"), Resource.Create(Guid.NewGuid(), "TestModule", "TestService", "TestController", "TestAction", Domain.Enums.HttpMethodEnum.GET));
        var cancellationToken = CancellationToken.None;
        var rbacAggregate = RbacAggregate.Create(request.Id, "Name", "Description", idUser);

        repositoryMock
            .Setup(repo => repo.FindAsync<RbacAggregate>(request.Id, cancellationToken))
            .ReturnsAsync(rbacAggregate);
        userContextMock.Setup(user => user.IdUser).Returns(idUser);

        // Act
        await handler.Handle(request, cancellationToken);

        // Assert
        repositoryMock.Verify(repo => repo.UpdateAsync(rbacAggregate, cancellationToken), Times.Once);
        pubSubMock.Verify(pubsub => pubsub.PublishAsync(It.IsAny<List<PermissionAddedDomainEvent>>(), cancellationToken), Times.AtMostOnce);
    }
}
