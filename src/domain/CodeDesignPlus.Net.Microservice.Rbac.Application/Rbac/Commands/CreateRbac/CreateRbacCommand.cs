using CodeDesignPlus.Net.Microservice.Rbac.Domain.ValueObjects;

namespace CodeDesignPlus.Net.Microservice.Rbac.Application.Rbac.Commands.CreateRbac;

[DtoGenerator]
public record CreateRbacCommand(Guid Id, string Name, string Description, List<RbacPermissionDto> RbacPermissions) : IRequest;

public class Validator : AbstractValidator<CreateRbacCommand>
{
    public Validator()
    {
        RuleFor(x => x.Id).NotEmpty().NotNull();
        RuleFor(x => x.Name).NotEmpty().NotNull().MaximumLength(128);
        RuleFor(x => x.Description).NotEmpty().NotNull().MaximumLength(512);
        RuleFor(x => x.RbacPermissions).NotEmpty().NotNull();
    }
}
