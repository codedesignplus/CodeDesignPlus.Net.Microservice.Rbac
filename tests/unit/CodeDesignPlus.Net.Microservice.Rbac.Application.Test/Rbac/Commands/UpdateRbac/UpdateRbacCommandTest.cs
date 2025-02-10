
using System;
using FluentValidation.TestHelper;
using CodeDesignPlus.Net.Microservice.Rbac.Application.Rbac.Commands.UpdateRbac;
using Xunit;

namespace CodeDesignPlus.Net.Microservice.Rbac.Application.Test.Rbac.Commands.UpdateRbac;

public class UpdateRbacCommandTest
{
    private readonly Validator validator;

    public UpdateRbacCommandTest()
    {
        validator = new Validator();
    }

    [Fact]
    public void Should_Have_Error_When_Id_Is_Empty()
    {
        var command = new UpdateRbacCommand(Guid.Empty, "ValidName", "ValidDescription", true);
        var result = validator.TestValidate(command);
        result.ShouldHaveValidationErrorFor(x => x.Id);
    }

    [Fact]
    public void Should_Have_Error_When_Name_Is_Null_Or_Empty()
    {
        var command = new UpdateRbacCommand(Guid.NewGuid(), null!, "ValidDescription", true);
        var result = validator.TestValidate(command);
        result.ShouldHaveValidationErrorFor(x => x.Name);

        command = new UpdateRbacCommand(Guid.NewGuid(), "", "ValidDescription", true);
        result = validator.TestValidate(command);
        result.ShouldHaveValidationErrorFor(x => x.Name);
    }

    [Fact]
    public void Should_Have_Error_When_Name_Exceeds_Max_Length()
    {
        var command = new UpdateRbacCommand(Guid.NewGuid(), new string('a', 129), "ValidDescription", true);
        var result = validator.TestValidate(command);
        result.ShouldHaveValidationErrorFor(x => x.Name);
    }

    [Fact]
    public void Should_Have_Error_When_Description_Is_Null_Or_Empty()
    {
        var command = new UpdateRbacCommand(Guid.NewGuid(), "ValidName", null!, true);
        var result = validator.TestValidate(command);
        result.ShouldHaveValidationErrorFor(x => x.Description);

        command = new UpdateRbacCommand(Guid.NewGuid(), "ValidName", "", true);
        result = validator.TestValidate(command);
        result.ShouldHaveValidationErrorFor(x => x.Description);
    }

    [Fact]
    public void Should_Have_Error_When_Description_Exceeds_Max_Length()
    {
        var command = new UpdateRbacCommand(Guid.NewGuid(), "ValidName", new string('a', 513), true);
        var result = validator.TestValidate(command);
        result.ShouldHaveValidationErrorFor(x => x.Description);
    }

    [Fact]
    public void Should_Have_Error_When_IsActive_Is_Null()
    {
        var command = new UpdateRbacCommand(Guid.NewGuid(), "ValidName", "ValidDescription", false);
        var result = validator.TestValidate(command);
        result.ShouldNotHaveValidationErrorFor(x => x.IsActive);
    }
}
