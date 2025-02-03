using CodeDesignPlus.Net.Microservice.Rbac.Domain.ValueObjects;

namespace CodeDesignPlus.Net.Microservice.Rbac.Application.Rbac.Commands.AddPermission;

[DtoGenerator]
public record AddPermissionCommand(Guid Id, Guid IdRbacPermission, Role Role, Resource Resource) : IRequest;

public class Validator : AbstractValidator<AddPermissionCommand>
{
    public Validator()
    {
        RuleFor(x => x.Id).NotEmpty().NotNull();
        RuleFor(x => x.IdRbacPermission).NotEmpty().NotNull();
        RuleFor(x => x.Role).NotEmpty().NotNull();
        RuleFor(x => x.Resource).NotEmpty().NotNull();
    }
}
