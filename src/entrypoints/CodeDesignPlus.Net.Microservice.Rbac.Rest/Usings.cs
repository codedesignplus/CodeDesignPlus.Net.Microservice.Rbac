global using CodeDesignPlus.Microservice.Api.Dtos;
global using CodeDesignPlus.Net.Logger.Extensions;
global using CodeDesignPlus.Net.Mongo.Extensions;
global using CodeDesignPlus.Net.Observability.Extensions;
global using CodeDesignPlus.Net.RabbitMQ.Extensions;
global using CodeDesignPlus.Net.Redis.Extensions;
global using CodeDesignPlus.Net.Security.Extensions;
global using Mapster;
global using MapsterMapper;
global using MediatR;
global using Microsoft.AspNetCore.Mvc;
global using C = CodeDesignPlus.Net.Core.Abstractions.Models.Criteria;
global using CodeDesignPlus.Net.Serializers;
global using NodaTime;









global using CodeDesignPlus.Net.Microservice.Rbac.Application.Rbac.Commands.CreateRbac;
global using CodeDesignPlus.Net.Microservice.Rbac.Application.Rbac.Commands.UpdateRbac;
global using CodeDesignPlus.Net.Microservice.Rbac.Application.Rbac.Commands.DeleteRbac;
global using CodeDesignPlus.Net.Microservice.Rbac.Application.Rbac.Queries.GetRbacById;
global using CodeDesignPlus.Net.Microservice.Rbac.Application.Rbac.Queries.GetAllRbac;