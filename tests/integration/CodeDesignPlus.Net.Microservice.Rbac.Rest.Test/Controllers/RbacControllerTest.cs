using CodeDesignPlus.Net.Microservice.Rbac.Domain.ValueObjects;
using NodaTime.Serialization.SystemTextJson;

namespace CodeDesignPlus.Net.Microservice.Rbac.Rest.Test.Controllers;

public class RbacControllerTest : ServerBase<Program>, IClassFixture<Server<Program>>
{

    public readonly static System.Text.Json.JsonSerializerOptions options = new System.Text.Json.JsonSerializerOptions()
    {
        PropertyNamingPolicy = System.Text.Json.JsonNamingPolicy.CamelCase,
    }.ConfigureForNodaTime(DateTimeZoneProviders.Tzdb);

    public RbacControllerTest(Server<Program> server) : base(server)
    {
        server.InMemoryCollection = (x) =>
        {
            x.Add("Vault:Enable", "false");
            x.Add("Vault:Address", "http://localhost:8200");
            x.Add("Vault:Token", "root");
            x.Add("Solution", "CodeDesignPlus");
            x.Add("AppName", "my-test");
            x.Add("RabbitMQ:UserName", "guest");
            x.Add("RabbitMQ:Password", "guest");
            x.Add("Security:ValidAudiences:0", Guid.NewGuid().ToString());
        };
    }

    [Fact]
    public async Task GetRbac_ReturnOk()
    {
        var data = await this.CreateRbacAsync(false);

        var response = await this.RequestAsync("http://localhost/api/Rbac", null, HttpMethod.Get);

        Assert.NotNull(response);
        Assert.Equal(HttpStatusCode.OK, response.StatusCode);

        var json = await response.Content.ReadAsStringAsync();

        var rbac = System.Text.Json.JsonSerializer.Deserialize<IEnumerable<RbacDto>>(json, options);

        Assert.NotNull(rbac);
        Assert.NotEmpty(rbac);
        Assert.Contains(rbac, x => x.Id == data.Id);
    }

    [Fact]
    public async Task GetRbacById_ReturnOk()
    {
        var rbacCreated = await this.CreateRbacAsync(false);

        var response = await this.RequestAsync($"http://localhost/api/Rbac/{rbacCreated.Id}", null, HttpMethod.Get);

        Assert.NotNull(response);
        Assert.Equal(HttpStatusCode.OK, response.StatusCode);

        var json = await response.Content.ReadAsStringAsync();

        var rbac = System.Text.Json.JsonSerializer.Deserialize<RbacDto>(json, options);

        Assert.NotNull(rbac);
        Assert.Equal(rbacCreated.Id, rbac.Id);
        Assert.Equal(rbacCreated.Name, rbac.Name);
        Assert.Equal(rbacCreated.Description, rbac.Description);
    }

    [Fact]
    public async Task CreateRbac_ReturnNoContent()
    {
        var data = new CreateRbacDto()
        {
            Id = Guid.NewGuid(),
            Name = "Rbac Test",
            Description = "Rbac Test",
        };

        var json = System.Text.Json.JsonSerializer.Serialize(data, options);

        var content = new StringContent(json, Encoding.UTF8, "application/json");

        var response = await this.RequestAsync("http://localhost/api/Rbac", content, HttpMethod.Post);

        await InactiveRbac(data);

        var rbac = await this.GetRecordAsync(data.Id);

        Assert.NotNull(response);
        Assert.Equal(HttpStatusCode.NoContent, response.StatusCode);

        Assert.Equal(data.Id, rbac.Id);
        Assert.Equal(data.Name, rbac.Name);
        Assert.Equal(data.Description, rbac.Description);
    }

    [Fact]
    public async Task UpdateRbac_ReturnNoContent()
    {
        var rbacCreated = await this.CreateRbacAsync(false);

        var data = new UpdateRbacDto()
        {
            Id = rbacCreated.Id,
            Name = "Rbac Test Updated",
            Description = "Rbac Test Updated",
        };

        var json = System.Text.Json.JsonSerializer.Serialize(data, options);

        var content = new StringContent(json, Encoding.UTF8, "application/json");

        var response = await this.RequestAsync($"http://localhost/api/Rbac/{rbacCreated.Id}", content, HttpMethod.Put);

        var rbac = await this.GetRecordAsync(data.Id);

        Assert.NotNull(response);
        Assert.Equal(HttpStatusCode.NoContent, response.StatusCode);

        Assert.Equal(data.Id, rbac.Id);
        Assert.Equal(data.Name, rbac.Name);
        Assert.Equal(data.Description, rbac.Description);
    }

    [Fact]
    public async Task DeleteRbac_ReturnNoContent()
    {
        var rbacCreated = await this.CreateRbacAsync(false);

        var response = await this.RequestAsync($"http://localhost/api/Rbac/{rbacCreated.Id}", null, HttpMethod.Delete);

        Assert.NotNull(response);
        Assert.True(response.IsSuccessStatusCode);
        Assert.Equal(HttpStatusCode.NoContent, response.StatusCode);
    }

    [Fact]
    public async Task AddPermission_ReturnNoContent()
    {
        var rbacCreated = await this.CreateRbacAsync(false);

        var permission = new AddPermissionDto()
        {
            Id = rbacCreated.Id,
            IdRbacPermission = Guid.NewGuid(),
            Resource = Resource.Create(Guid.NewGuid(), "ModuleTest", "ServiceTest", "ControllerTest", "ActionTest", Domain.Enums.HttpMethodEnum.GET),
            Role = Role.Create(Guid.NewGuid(), "Role Test")
        };

        var json = System.Text.Json.JsonSerializer.Serialize(permission, options);

        var content = new StringContent(json, Encoding.UTF8, "application/json");

        var response = await this.RequestAsync($"http://localhost/api/Rbac/{rbacCreated.Id}/permissions", content, HttpMethod.Post);

        var rbac = await this.GetRecordAsync(rbacCreated.Id);

        Assert.NotNull(response);
        Assert.True(response.IsSuccessStatusCode);
        Assert.Equal(HttpStatusCode.NoContent, response.StatusCode);

        Assert.NotNull(rbac.Permissions);
        Assert.NotEmpty(rbac.Permissions);
        Assert.Contains(rbac.Permissions, x =>
            x.Id == permission.IdRbacPermission
            && x.Resource.Module == permission.Resource.Module
            && x.Resource.Service == permission.Resource.Service
            && x.Resource.Controller == permission.Resource.Controller
            && x.Resource.Action == permission.Resource.Action
        );
    }

    [Fact]
    public async Task RemovePermission_ReturnNoContent()
    {
        var rbacCreated = await this.CreateRbacAsync(false);

        var permission = new AddPermissionDto()
        {
            Id = rbacCreated.Id,
            IdRbacPermission = Guid.NewGuid(),
            Resource = Resource.Create(Guid.NewGuid(), "ModuleTest", "ServiceTest", "ControllerTest", "ActionTest", Domain.Enums.HttpMethodEnum.GET),
            Role = Role.Create(Guid.NewGuid(), "Role Test")
        };

        var json = System.Text.Json.JsonSerializer.Serialize(permission, options);

        var content = new StringContent(json, Encoding.UTF8, "application/json");

        await this.RequestAsync($"http://localhost/api/Rbac/{rbacCreated.Id}/permissions", content, HttpMethod.Post);

        var response = await this.RequestAsync($"http://localhost/api/Rbac/{rbacCreated.Id}/permissions/{permission.IdRbacPermission}", null, HttpMethod.Delete);

        var rbac = await this.GetRecordAsync(rbacCreated.Id);

        Assert.NotNull(response);
        Assert.True(response.IsSuccessStatusCode);
        Assert.Equal(HttpStatusCode.NoContent, response.StatusCode);

        Assert.NotNull(rbac.Permissions);
        Assert.DoesNotContain(rbac.Permissions, x => x.Id == permission.IdRbacPermission);
    }


    private static StringContent BuildBody(object data)
    {
        var json = System.Text.Json.JsonSerializer.Serialize(data, options);

        var content = new StringContent(json, Encoding.UTF8, "application/json");

        return content;
    }

    private async Task<CreateRbacDto> CreateRbacAsync(bool isActive)
    {
        var data = new CreateRbacDto()
        {
            Id = Guid.NewGuid(),
            Name = "Rbac Test",
            Description = "Rbac Test"
        };

        var json = System.Text.Json.JsonSerializer.Serialize(data, options);

        var content = new StringContent(json, Encoding.UTF8, "application/json");

        await this.RequestAsync("http://localhost/api/Rbac", content, HttpMethod.Post);

        if (!isActive)
            await this.InactiveRbac(data);

        return data;
    }

    private async Task InactiveRbac(CreateRbacDto rbac)
    {
        var data = new UpdateRbacDto()
        {
            Id = rbac.Id,
            Name = rbac.Name,
            Description = rbac.Description,
            IsActive = false
        };

        var json = System.Text.Json.JsonSerializer.Serialize(data, options);

        var content = new StringContent(json, Encoding.UTF8, "application/json");

        await this.RequestAsync($"http://localhost/api/Rbac/{rbac.Id}", content, HttpMethod.Put);
    }

    private async Task<RbacDto> GetRecordAsync(Guid id)
    {
        var response = await this.RequestAsync($"http://localhost/api/Rbac/{id}", null, HttpMethod.Get);

        var json = await response.Content.ReadAsStringAsync();

        return System.Text.Json.JsonSerializer.Deserialize<RbacDto>(json, options)!;
    }

    private async Task<HttpResponseMessage> RequestAsync(string uri, HttpContent? content, HttpMethod method)
    {
        var httpRequestMessage = new HttpRequestMessage()
        {
            RequestUri = new Uri(uri),
            Content = content,
            Method = method
        };
        httpRequestMessage.Headers.Authorization = new AuthenticationHeaderValue("TestAuth");

        var response = await Client.SendAsync(httpRequestMessage);

        return response;
    }

}