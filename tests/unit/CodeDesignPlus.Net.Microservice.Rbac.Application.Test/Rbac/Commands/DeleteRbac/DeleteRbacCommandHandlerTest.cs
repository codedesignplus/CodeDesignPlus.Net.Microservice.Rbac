using CodeDesignPlus.Net.Microservice.Rbac.Application.Rbac.Commands.DeleteRbac;
using CodeDesignPlus.Net.Microservice.Rbac.Domain.DomainEvents;

namespace CodeDesignPlus.Net.Microservice.Rbac.Application.Test.Rbac.Commands.DeleteRbac;

public class DeleteRbacCommandHandlerTest
{
    private readonly Mock<IRbacRepository> repositoryMock;
    private readonly Mock<IUserContext> userContextMock;
    private readonly Mock<IPubSub> pubSubMock;
    private readonly DeleteRbacCommandHandler handler;

    public DeleteRbacCommandHandlerTest()
    {
        repositoryMock = new Mock<IRbacRepository>();
        userContextMock = new Mock<IUserContext>();
        pubSubMock = new Mock<IPubSub>();
        handler = new DeleteRbacCommandHandler(repositoryMock.Object, userContextMock.Object, pubSubMock.Object);
    }

    [Fact]
    public async Task Handle_RequestIsNull_ThrowsCodeDesignPlusException()
    {
        // Arrange
        DeleteRbacCommand request = null!;
        var cancellationToken = CancellationToken.None;

        // Act & Assert
        var exception = await Assert.ThrowsAsync<CodeDesignPlusException>(() => handler.Handle(request, cancellationToken));

        Assert.Equal(Errors.InvalidRequest.GetMessage(), exception.Message);
        Assert.Equal(Errors.InvalidRequest.GetCode(), exception.Code);
        Assert.Equal(Layer.Application, exception.Layer);
    }

    [Fact]
    public async Task Handle_AggregateNotFound_ThrowsCodeDesignPlusException()
    {
        // Arrange
        var request = new DeleteRbacCommand(Guid.NewGuid());
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
    public async Task Handle_ValidRequest_DeletesAggregateAndPublishesEvents()
    {
        // Arrange
        var request = new DeleteRbacCommand(Guid.NewGuid());
        var cancellationToken = CancellationToken.None;
        var aggregate = RbacAggregate.Create(Guid.NewGuid(), "Test", "Test Description", Guid.NewGuid());

        repositoryMock
            .Setup(repo => repo.FindAsync<RbacAggregate>(request.Id, cancellationToken))
            .ReturnsAsync(aggregate);

        userContextMock.Setup(user => user.IdUser).Returns(Guid.NewGuid());

        // Act
        await handler.Handle(request, cancellationToken);

        // Assert
        repositoryMock.Verify(repo => repo.DeleteAsync<RbacAggregate>(aggregate.Id, cancellationToken), Times.Once);
        pubSubMock.Verify(pubsub => pubsub.PublishAsync(It.IsAny<List<RbacDeletedDomainEvent>>(), cancellationToken), Times.AtMostOnce);
    }
}
