namespace CodeDesignPlus.Net.Microservice.Rbac.Application.Rbac.Commands.RemovePermission;

[DtoGenerator]
public record RemovePermissionCommand(Guid Id, Guid IdRbacPermission) : IRequest;

public class Validator : AbstractValidator<RemovePermissionCommand>
{
    public Validator()
    {
        RuleFor(x => x.Id).NotEmpty().NotNull();
        RuleFor(x => x.IdRbacPermission).NotEmpty().NotNull();
    }
}
