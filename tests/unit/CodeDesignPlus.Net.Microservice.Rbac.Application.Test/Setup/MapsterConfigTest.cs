using CodeDesignPlus.Net.Microservice.Rbac.Application.Setup;

namespace CodeDesignPlus.Net.Microservice.Rbac.Application.Test.Setup;

public class MapsterConfigTest
{
    [Fact]
    public void Configure_ShouldMapProperties_Success()
    {
        // Arrange
        var config = TypeAdapterConfig.GlobalSettings;
        config.Scan(typeof(MapsterConfigRbac).Assembly);

        // Act
        var mapper = new Mapper(config);

        // Assert
        Assert.NotNull(mapper);
    }
}
