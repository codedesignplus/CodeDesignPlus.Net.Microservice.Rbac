namespace CodeDesignPlus.Net.Microservice.Rbac.Application.Rbac.Commands.UpdateRbac;

[DtoGenerator]
public record UpdateRbacCommand(Guid Id, string Name, string Description, bool IsActive) : IRequest;

public class Validator : AbstractValidator<UpdateRbacCommand>
{
    public Validator()
    {
        RuleFor(x => x.Id).NotEmpty().NotNull();
        RuleFor(x => x.Name).NotEmpty().NotNull().MaximumLength(100);
        RuleFor(x => x.Description).NotEmpty().NotNull().MaximumLength(500);
        RuleFor(x => x.IsActive).NotNull();
    }
}
