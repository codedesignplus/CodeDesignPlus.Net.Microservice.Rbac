namespace CodeDesignPlus.Net.Microservice.Rbac.Application.Rbac.Commands.CreateRbac;

[DtoGenerator]
public record CreateRbacCommand(Guid Id, string Name, string Description) : IRequest;

public class Validator : AbstractValidator<CreateRbacCommand>
{
    public Validator()
    {
        RuleFor(x => x.Id).NotEmpty().NotNull();
        RuleFor(x => x.Name).NotEmpty().NotNull().MaximumLength(100);
        RuleFor(x => x.Description).NotEmpty().NotNull().MaximumLength(500);
    }
}
