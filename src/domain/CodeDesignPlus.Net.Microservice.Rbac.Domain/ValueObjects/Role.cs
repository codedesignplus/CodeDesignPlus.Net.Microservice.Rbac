using System.Text.Json.Serialization;

namespace CodeDesignPlus.Net.Microservice.Rbac.Domain.ValueObjects;

public sealed partial class Role
{
    [GeneratedRegex(@"^0x[0-9]{32}$")]
    private static partial Regex Regex();

    public Guid Id { get; private set; }

    public string Name { get; private set; }

    [JsonConstructor]
    private Role(Guid id, string name)
    {
        DomainGuard.GuidIsEmpty(id, Errors.IdRoleIsInvalid);
        DomainGuard.IsNullOrEmpty(name, Errors.NameRoleIsInvalid);

        this.Id = id;
        this.Name = name;
    }

    public static Role Create(Guid id, string name)
    {
        return new Role(id, name);
    }
}
