using System;
using CodeDesignPlus.Net.Microservice.Rbac.Application.Rbac.Commands.CreateRbac;
using FluentValidation.TestHelper;
using Xunit;

namespace CodeDesignPlus.Net.Microservice.Rbac.Application.Test.Rbac.Commands.CreateRbac;

public class CreateRbacCommandTest
{
    private readonly Validator validator;

    public CreateRbacCommandTest()
    {
        validator = new Validator();
    }

    [Fact]
    public void Should_Have_Error_When_Id_Is_Empty()
    {
        var command = new CreateRbacCommand(Guid.Empty, "ValidName", "ValidDescription");
        var result = validator.TestValidate(command);
        result.ShouldHaveValidationErrorFor(x => x.Id);
    }

    [Fact]
    public void Should_Have_Error_When_Name_Is_Empty()
    {
        var command = new CreateRbacCommand(Guid.NewGuid(), string.Empty, "ValidDescription");
        var result = validator.TestValidate(command);
        result.ShouldHaveValidationErrorFor(x => x.Name);
    }

    [Fact]
    public void Should_Have_Error_When_Name_Exceeds_Max_Length()
    {
        var command = new CreateRbacCommand(Guid.NewGuid(), new string('a', 129), "ValidDescription");
        var result = validator.TestValidate(command);
        result.ShouldHaveValidationErrorFor(x => x.Name);
    }

    [Fact]
    public void Should_Have_Error_When_Description_Is_Empty()
    {
        var command = new CreateRbacCommand(Guid.NewGuid(), "ValidName", string.Empty);
        var result = validator.TestValidate(command);
        result.ShouldHaveValidationErrorFor(x => x.Description);
    }

    [Fact]
    public void Should_Have_Error_When_Description_Exceeds_Max_Length()
    {
        var command = new CreateRbacCommand(Guid.NewGuid(), "ValidName", new string('a', 513));
        var result = validator.TestValidate(command);
        result.ShouldHaveValidationErrorFor(x => x.Description);
    }

    [Fact]
    public void Should_Not_Have_Error_When_Command_Is_Valid()
    {
        var command = new CreateRbacCommand(Guid.NewGuid(), "ValidName", "ValidDescription");
        var result = validator.TestValidate(command);
        result.ShouldNotHaveAnyValidationErrors();
    }
}
