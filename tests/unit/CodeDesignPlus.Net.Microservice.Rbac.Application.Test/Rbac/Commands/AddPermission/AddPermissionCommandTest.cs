using System;
using CodeDesignPlus.Net.Microservice.Rbac.Application.Rbac.Commands.AddPermission;
using CodeDesignPlus.Net.Microservice.Rbac.Domain.ValueObjects;
using FluentValidation.TestHelper;
using Xunit;

namespace CodeDesignPlus.Net.Microservice.Rbac.Application.Test.Rbac.Commands.AddPermission;

public class AddPermissionCommandTest
{
    private readonly Validator validator;

    private readonly Role _role = Role.Create(Guid.NewGuid(), "Admin");

    private readonly Resource _resource = Resource.Create(Guid.NewGuid(), "TestModule", "TestService", "TestController", "TestAction", Domain.Enums.HttpMethodEnum.GET);

    public AddPermissionCommandTest()
    {
        validator = new Validator();
    }

    [Fact]
    public void Should_Have_Error_When_Id_Is_Empty()
    {
        var command = new AddPermissionCommand(Guid.Empty, Guid.NewGuid(), _role, _resource);
        var result = validator.TestValidate(command);
        result.ShouldHaveValidationErrorFor(x => x.Id);
    }

    [Fact]
    public void Should_Have_Error_When_IdRbacPermission_Is_Empty()
    {
        var command = new AddPermissionCommand(Guid.NewGuid(), Guid.Empty, _role, _resource);
        var result = validator.TestValidate(command);
        result.ShouldHaveValidationErrorFor(x => x.IdRbacPermission);
    }

    [Fact]
    public void Should_Have_Error_When_Role_Is_Null()
    {
        var command = new AddPermissionCommand(Guid.NewGuid(), Guid.NewGuid(), null!, _resource);
        var result = validator.TestValidate(command);
        result.ShouldHaveValidationErrorFor(x => x.Role);
    }

    [Fact]
    public void Should_Have_Error_When_Resource_Is_Null()
    {
        var command = new AddPermissionCommand(Guid.NewGuid(), Guid.NewGuid(), _role, null!);
        var result = validator.TestValidate(command);
        result.ShouldHaveValidationErrorFor(x => x.Resource);
    }

    [Fact]
    public void Should_Not_Have_Error_When_Command_Is_Valid()
    {
        var command = new AddPermissionCommand(Guid.NewGuid(), Guid.NewGuid(), _role, _resource);
        var result = validator.TestValidate(command);
        result.ShouldNotHaveAnyValidationErrors();
    }
}
