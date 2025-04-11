using CodeDesignPlus.Net.Microservice.Rbac.Domain.ValueObjects;
using CodeDesignPlus.Microservice.Api.Dtos;
using CodeDesignPlus.Net.Microservice.Rbac.Application.Rbac.Commands.CreateRbac;
using CodeDesignPlus.Net.Microservice.Rbac.Application.Rbac.Commands.UpdateRbac;
using CodeDesignPlus.Net.Microservice.Rbac.Application.Rbac.Commands.AddPermission;
using CodeDesignPlus.Net.Microservice.Rbac.Domain.Entities;

namespace CodeDesignPlus.Net.Microservice.Rbac.Application.Setup;

public static class MapsterConfigRbac
{
    public static void Configure()
    {
        //Rbac
        TypeAdapterConfig<CreateRbacDto, CreateRbacCommand>.NewConfig();
        TypeAdapterConfig<UpdateRbacDto, UpdateRbacCommand>.NewConfig();
        TypeAdapterConfig<RbacAggregate, RbacDto>
            .NewConfig()
            .MapWith(x => new RbacDto()
            {
                Id = x.Id,
                Name = x.Name,
                Description = x.Description,
                Permissions = x.Permissions.Select(p => new RbacPermissionDto
                {
                    Id = p.Id,
                    Role = Role.Create(p.Role.Id, p.Role.Name),
                    Resource = Resource.Create(p.Id, p.Resource.Module, p.Resource.Service, p.Resource.Controller, p.Resource.Action, p.Resource.Method)
                }).ToList(),
                IsActive = x.IsActive
            });

        //Permission
        TypeAdapterConfig<AddPermissionDto, AddPermissionCommand>
            .NewConfig()
            .ConstructUsing(dto => new AddPermissionCommand(
                dto.Id,
                dto.IdRbacPermission,
                Role.Create(dto.Role.Id, dto.Role.Name),
                Resource.Create(dto.Id, dto.Resource.Module, dto.Resource.Service, dto.Resource.Controller, dto.Resource.Action, dto.Resource.Method)
            ));

        //RbacPermissionEntity
        TypeAdapterConfig<RbacPermissionEntity, RbacPermissionDto>
            .NewConfig()
            .ConstructUsing(entity => new RbacPermissionDto
            {
                Id = entity.Id,
                Role = Role.Create(entity.Role.Id, entity.Role.Name),
                Resource = Resource.Create(entity.Id, entity.Resource.Module, entity.Resource.Service, entity.Resource.Controller, entity.Resource.Action, entity.Resource.Method)
            });

        TypeAdapterConfig<RbacPermissionEntity, RbacResourceDto>
            .NewConfig()
            .ConstructUsing(entity => new RbacResourceDto
            {
                Id = entity.Id,
                Role = entity.Role.Name,
                Module = entity.Resource.Module,                
                Service = entity.Resource.Service,
                Controller = entity.Resource.Controller,
                Action = entity.Resource.Action,
                Method = entity.Resource.Method
            });

    }
}
