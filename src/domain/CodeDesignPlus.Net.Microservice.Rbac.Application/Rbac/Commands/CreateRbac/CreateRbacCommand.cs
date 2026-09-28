using CodeDesignPlus.Net.Microservice.Rbac.Domain.ValueObjects;

namespace CodeDesignPlus.Net.Microservice.Rbac.Application.Rbac.Commands.CreateRbac;

[DtoGenerator]
public record CreateRbacCommand(Guid Id, string Name, string Description, bool IsActive, List<RbacPermissionDto> RbacPermissions) : IRequest;

public class Validator : AbstractValidator<CreateRbacCommand>
{
    public Validator()
    {
        RuleFor(x => x.Id).NotEmpty().NotNull();
        RuleFor(x => x.Name).NotEmpty().NotNull().MaximumLength(FieldLength.Name);
        RuleFor(x => x.Description).NotEmpty().NotNull().MaximumLength(FieldLength.Description);
        RuleFor(x => x.RbacPermissions).NotEmpty().NotNull();
    }
}
