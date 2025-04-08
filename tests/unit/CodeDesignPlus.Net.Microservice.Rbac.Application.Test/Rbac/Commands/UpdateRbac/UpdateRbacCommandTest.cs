
using System;
using FluentValidation.TestHelper;
using CodeDesignPlus.Net.Microservice.Rbac.Application.Rbac.Commands.UpdateRbac;
using Xunit;
using CodeDesignPlus.Net.Microservice.Rbac.Domain.ValueObjects;

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
        var role = Role.Create(Guid.NewGuid(), "Admin");
        var resource = Resource.Create(Guid.NewGuid(), "Custom Module", "Custom Service", "Custom Controller", "Custom Action", Domain.Enums.HttpMethodEnum.PUT);
        var rbacPermissions = new List<RbacPermissionDto>
        {
            new () {
                Id = Guid.NewGuid(),
                Role = role,
                Resource = resource
            }
        };
        var command = new UpdateRbacCommand(Guid.Empty, "ValidName", "ValidDescription", true, rbacPermissions);
        var result = validator.TestValidate(command);
        result.ShouldHaveValidationErrorFor(x => x.Id);
    }

    [Fact]
    public void Should_Have_Error_When_Name_Is_Null_Or_Empty()
    {
        var role = Role.Create(Guid.NewGuid(), "Admin");
        var resource = Resource.Create(Guid.NewGuid(), "Custom Module", "Custom Service", "Custom Controller", "Custom Action", Domain.Enums.HttpMethodEnum.PUT);
        var rbacPermissions = new List<RbacPermissionDto>
        {
            new () {
                Id = Guid.NewGuid(),
                Role = role,
                Resource = resource
            }
        };
        var command = new UpdateRbacCommand(Guid.NewGuid(), null!, "ValidDescription", true, rbacPermissions);
        var result = validator.TestValidate(command);
        result.ShouldHaveValidationErrorFor(x => x.Name);

        command = new UpdateRbacCommand(Guid.NewGuid(), "", "ValidDescription", true, rbacPermissions);
        result = validator.TestValidate(command);
        result.ShouldHaveValidationErrorFor(x => x.Name);
    }

    [Fact]
    public void Should_Have_Error_When_Name_Exceeds_Max_Length()
    {
        var role = Role.Create(Guid.NewGuid(), "Admin");
        var resource = Resource.Create(Guid.NewGuid(), "Custom Module", "Custom Service", "Custom Controller", "Custom Action", Domain.Enums.HttpMethodEnum.PUT);
        var rbacPermissions = new List<RbacPermissionDto>
        {
            new () {
                Id = Guid.NewGuid(),
                Role = role,
                Resource = resource
            }
        };
        var command = new UpdateRbacCommand(Guid.NewGuid(), new string('a', 129), "ValidDescription", true, rbacPermissions);
        var result = validator.TestValidate(command);
        result.ShouldHaveValidationErrorFor(x => x.Name);
    }

    [Fact]
    public void Should_Have_Error_When_Description_Is_Null_Or_Empty()
    {
        var role = Role.Create(Guid.NewGuid(), "Admin");
        var resource = Resource.Create(Guid.NewGuid(), "Custom Module", "Custom Service", "Custom Controller", "Custom Action", Domain.Enums.HttpMethodEnum.PUT);
        var rbacPermissions = new List<RbacPermissionDto>
        {
            new () {
                Id = Guid.NewGuid(),
                Role = role,
                Resource = resource
            }
        };
        var command = new UpdateRbacCommand(Guid.NewGuid(), "ValidName", null!, true, rbacPermissions);
        var result = validator.TestValidate(command);
        result.ShouldHaveValidationErrorFor(x => x.Description);

        command = new UpdateRbacCommand(Guid.NewGuid(), "ValidName", "", true, rbacPermissions);
        result = validator.TestValidate(command);
        result.ShouldHaveValidationErrorFor(x => x.Description);
    }

    [Fact]
    public void Should_Have_Error_When_Description_Exceeds_Max_Length()
    {
        var role = Role.Create(Guid.NewGuid(), "Admin");
        var resource = Resource.Create(Guid.NewGuid(), "Custom Module", "Custom Service", "Custom Controller", "Custom Action", Domain.Enums.HttpMethodEnum.PUT);
        var rbacPermissions = new List<RbacPermissionDto>
        {
            new () {
                Id = Guid.NewGuid(),
                Role = role,
                Resource = resource
            }
        };
        var command = new UpdateRbacCommand(Guid.NewGuid(), "ValidName", new string('a', 513), true, rbacPermissions);
        var result = validator.TestValidate(command);
        result.ShouldHaveValidationErrorFor(x => x.Description);
    }

    [Fact]
    public void Should_Have_Error_When_IsActive_Is_Null()
    {
        var role = Role.Create(Guid.NewGuid(), "Admin");
        var resource = Resource.Create(Guid.NewGuid(), "Custom Module", "Custom Service", "Custom Controller", "Custom Action", Domain.Enums.HttpMethodEnum.PUT);
        var rbacPermissions = new List<RbacPermissionDto>
        {
            new () {
                Id = Guid.NewGuid(),
                Role = role,
                Resource = resource
            }
        };
        var command = new UpdateRbacCommand(Guid.NewGuid(), "ValidName", "ValidDescription", false, rbacPermissions);
        var result = validator.TestValidate(command);
        result.ShouldNotHaveValidationErrorFor(x => x.IsActive);
    }
}
