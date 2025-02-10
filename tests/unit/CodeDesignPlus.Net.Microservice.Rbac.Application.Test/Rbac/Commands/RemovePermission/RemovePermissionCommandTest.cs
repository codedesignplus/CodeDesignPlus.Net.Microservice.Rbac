using System;
using FluentValidation.TestHelper;
using CodeDesignPlus.Net.Microservice.Rbac.Application.Rbac.Commands.RemovePermission;

namespace CodeDesignPlus.Net.Microservice.Rbac.Application.Test.Rbac.Commands.RemovePermission;

public class RemovePermissionCommandTest
{
    private readonly Validator validator;

    public RemovePermissionCommandTest()
    {
        validator = new Validator();
    }

    [Fact]
    public void Should_Have_Error_When_Id_Is_Null()
    {
        var command = new RemovePermissionCommand(Guid.Empty, Guid.NewGuid());
        var result = validator.TestValidate(command);
        result.ShouldHaveValidationErrorFor(x => x.Id);
    }

    [Fact]
    public void Should_Have_Error_When_IdRbacPermission_Is_Null()
    {
        var command = new RemovePermissionCommand(Guid.NewGuid(), Guid.Empty);
        var result = validator.TestValidate(command);
        result.ShouldHaveValidationErrorFor(x => x.IdRbacPermission);
    }

    [Fact]
    public void Should_Not_Have_Error_When_Ids_Are_Valid()
    {
        var command = new RemovePermissionCommand(Guid.NewGuid(), Guid.NewGuid());
        var result = validator.TestValidate(command);
        result.ShouldNotHaveValidationErrorFor(x => x.Id);
        result.ShouldNotHaveValidationErrorFor(x => x.IdRbacPermission);
    }
}
