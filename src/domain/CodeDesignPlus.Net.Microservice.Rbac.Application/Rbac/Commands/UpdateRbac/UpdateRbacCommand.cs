using CodeDesignPlus.Net.Microservice.Rbac.Domain.ValueObjects;

namespace CodeDesignPlus.Net.Microservice.Rbac.Application.Rbac.Commands.UpdateRbac;

[DtoGenerator]
public record UpdateRbacCommand(Guid Id, string Name, string Description, bool IsActive, List<RbacPermissionDto> RbacPermissions) : IRequest;

public class Validator : AbstractValidator<UpdateRbacCommand>
{
    public Validator()
    {
        RuleFor(x => x.Id).NotEmpty().NotNull();
        RuleFor(x => x.Name).NotEmpty().NotNull().MaximumLength(FieldLength.Name);
        RuleFor(x => x.Description).NotEmpty().NotNull().MaximumLength(FieldLength.Description);
        RuleFor(x => x.IsActive).NotNull();

        RuleFor(x => x.RbacPermissions).NotEmpty().NotNull();
    }
}
