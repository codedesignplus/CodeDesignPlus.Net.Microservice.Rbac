using System.Threading;
using System.Threading.Tasks;
using Moq;
using Xunit;
using CodeDesignPlus.Net.Microservice.Rbac.Application.Rbac.Commands.CreateRbac;
using CodeDesignPlus.Net.Microservice.Rbac.Domain.DomainEvents;
using CodeDesignPlus.Net.Microservice.Rbac.Domain.ValueObjects;

namespace CodeDesignPlus.Net.Microservice.Rbac.Application.Test.Rbac.Commands.CreateRbac;

public class CreateRbacCommandHandlerTest
{
    private readonly Mock<IRbacRepository> repositoryMock;
    private readonly Mock<IUserContext> userContextMock;
    private readonly Mock<IPubSub> pubSubMock;
    private readonly CreateRbacCommandHandler handler;

    public CreateRbacCommandHandlerTest()
    {
        repositoryMock = new Mock<IRbacRepository>();
        userContextMock = new Mock<IUserContext>();
        pubSubMock = new Mock<IPubSub>();
        handler = new CreateRbacCommandHandler(repositoryMock.Object, userContextMock.Object, pubSubMock.Object);
    }

    [Fact]
    public async Task Handle_RequestIsNull_ThrowsCodeDesignPlusException()
    {
        // Arrange
        CreateRbacCommand request = null!;
        var cancellationToken = CancellationToken.None;

        // Act & Assert
        var exception = await Assert.ThrowsAsync<CodeDesignPlusException>(() => handler.Handle(request, cancellationToken));

        Assert.Equal(Errors.InvalidRequest.GetMessage(), exception.Message);
        Assert.Equal(Errors.InvalidRequest.GetCode(), exception.Code);
        Assert.Equal(Layer.Application, exception.Layer);
    }

    [Fact]
    public async Task Handle_RbacAlreadyExists_ThrowsCodeDesignPlusException()
    {
        // Arrange
        var role = Role.Create(Guid.NewGuid(), "Admin");
        var resource = Resource.Create(Guid.NewGuid(), "Custom Module", "Custom Service", "Custom Controller", "Custom Action", Domain.Enums.HttpMethodEnum.PUT);
        var request = new CreateRbacCommand(Guid.NewGuid(), "Test", "Test Description", role, resource);
        var cancellationToken = CancellationToken.None;

        repositoryMock
            .Setup(x => x.ExistsAsync<RbacAggregate>(request.Id, cancellationToken))
            .ReturnsAsync(true);

        // Act & Assert
        var exception = await Assert.ThrowsAsync<CodeDesignPlusException>(() => handler.Handle(request, cancellationToken));

        Assert.Equal(Errors.RbacAlreadyExists.GetMessage(), exception.Message);
        Assert.Equal(Errors.RbacAlreadyExists.GetCode(), exception.Code);
        Assert.Equal(Layer.Application, exception.Layer);
    }

    [Fact]
    public async Task Handle_ValidRequest_CreatesRbacAndPublishesEvents()
    {
        // Arrange
        var role = Role.Create(Guid.NewGuid(), "Admin");
        var resource = Resource.Create(Guid.NewGuid(), "Custom Module", "Custom Service", "Custom Controller", "Custom Action", Domain.Enums.HttpMethodEnum.PUT);
        var request = new CreateRbacCommand(Guid.NewGuid(), "Test", "Test Description", role, resource);
        var cancellationToken = CancellationToken.None;

        repositoryMock
            .Setup(x => x.ExistsAsync<RbacAggregate>(request.Id, cancellationToken))
            .ReturnsAsync(false);

        repositoryMock
            .Setup(x => x.HasActiveRbacAsync(userContextMock.Object.IdUser, cancellationToken))
            .ReturnsAsync(false);

        userContextMock.Setup(x => x.IdUser).Returns(Guid.NewGuid());

        // Act
        await handler.Handle(request, cancellationToken);

        // Assert
        repositoryMock.Verify(x => x.CreateAsync(It.IsAny<RbacAggregate>(), cancellationToken), Times.Once);
        pubSubMock.Verify(x => x.PublishAsync(It.IsAny<List<RbacCreatedDomainEvent>>(), cancellationToken), Times.AtMostOnce);
    }
}
