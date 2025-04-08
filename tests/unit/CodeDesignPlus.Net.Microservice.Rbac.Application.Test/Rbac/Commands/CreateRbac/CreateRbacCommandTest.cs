using CodeDesignPlus.Net.Microservice.Rbac.Application.Rbac.Commands.CreateRbac;
using CodeDesignPlus.Net.Microservice.Rbac.Domain.ValueObjects;
using FluentValidation.TestHelper;

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
        var command = new CreateRbacCommand(Guid.Empty, "ValidName", "ValidDescription", rbacPermissions);
        var result = validator.TestValidate(command);
        result.ShouldHaveValidationErrorFor(x => x.Id);
    }

    [Fact]
    public void Should_Have_Error_When_Name_Is_Empty()
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
        var command = new CreateRbacCommand(Guid.NewGuid(), string.Empty, "ValidDescription", rbacPermissions);
        var result = validator.TestValidate(command);
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
        var command = new CreateRbacCommand(Guid.NewGuid(), new string('a', 129), "ValidDescription", rbacPermissions);
        var result = validator.TestValidate(command);
        result.ShouldHaveValidationErrorFor(x => x.Name);
    }

    [Fact]
    public void Should_Have_Error_When_Description_Is_Empty()
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
        var command = new CreateRbacCommand(Guid.NewGuid(), "ValidName", string.Empty, rbacPermissions);
        var result = validator.TestValidate(command);
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
        var command = new CreateRbacCommand(Guid.NewGuid(), "ValidName", new string('a', 513), rbacPermissions);
        var result = validator.TestValidate(command);
        result.ShouldHaveValidationErrorFor(x => x.Description);
    }

    [Fact]
    public void Should_Not_Have_Error_When_Command_Is_Valid()
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
        var command = new CreateRbacCommand(Guid.NewGuid(), "ValidName", "ValidDescription", rbacPermissions);
        var result = validator.TestValidate(command);
        result.ShouldNotHaveAnyValidationErrors();
    }
}
