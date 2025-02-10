using System;
using FluentValidation.TestHelper;
using Xunit;
using CodeDesignPlus.Net.Microservice.Rbac.Application.Rbac.Commands.DeleteRbac;

namespace CodeDesignPlus.Net.Microservice.Rbac.Application.Test.Rbac.Commands.DeleteRbac;

public class DeleteRbacCommandTest
{
    private readonly Validator validator;

    public DeleteRbacCommandTest()
    {
        validator = new Validator();
    }

    [Fact]
    public void Should_Have_Error_When_Id_Is_Empty()
    {
        var command = new DeleteRbacCommand(Guid.Empty);
        var result = validator.TestValidate(command);
        result.ShouldHaveValidationErrorFor(x => x.Id);
    }

    [Fact]
    public void Should_Not_Have_Error_When_Id_Is_Valid()
    {
        var command = new DeleteRbacCommand(Guid.NewGuid());
        var result = validator.TestValidate(command);
        result.ShouldNotHaveValidationErrorFor(x => x.Id);
    }
}
