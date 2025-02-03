using CodeDesignPlus.Net.Microservice.Rbac.Domain.ValueObjects;
using CodeDesignPlus.Microservice.Api.Dtos;
using CodeDesignPlus.Net.Microservice.Rbac.Application.Rbac.Commands.CreateRbac;
using CodeDesignPlus.Net.Microservice.Rbac.Application.Rbac.Commands.UpdateRbac;
using CodeDesignPlus.Net.Microservice.Rbac.Application.Rbac.Commands.AddPermission;

namespace CodeDesignPlus.Net.Microservice.Rbac.Application.Setup;

public static class MapsterConfigRbac
{
    public static void Configure()
    {
        //Rbac
        TypeAdapterConfig<CreateRbacDto, CreateRbacCommand>.NewConfig();
        TypeAdapterConfig<UpdateRbacDto, UpdateRbacCommand>.NewConfig();
        TypeAdapterConfig<RbacAggregate, RbacDto>.NewConfig();

        //Permission
        TypeAdapterConfig<AddPermissionDto, AddPermissionCommand>
            .NewConfig()
            .ConstructUsing(dto => new AddPermissionCommand(
                dto.Id,
                dto.IdRbacPermission,
                Role.Create(dto.Role.Id, dto.Role.Name),
                Resource.Create(dto.Resource.Module, dto.Resource.Service, dto.Resource.Controller, dto.Resource.Action, dto.Resource.Method)
            ));

    }
}
