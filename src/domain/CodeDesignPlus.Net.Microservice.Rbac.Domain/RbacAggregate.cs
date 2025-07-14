using CodeDesignPlus.Net.Microservice.Rbac.Domain.Entities;
using CodeDesignPlus.Net.Microservice.Rbac.Domain.ValueObjects;

namespace CodeDesignPlus.Net.Microservice.Rbac.Domain;

public class RbacAggregate(Guid id) : AggregateRootBase(id)
{

    public string Name { get; private set; } = null!;

    public string Description { get; private set; } = null!;

    public List<RbacPermissionEntity> Permissions { get; private set; } = [];

    private RbacAggregate(Guid id, string name, string description, Guid createdBy) : this(id)
    {
        this.Name = name;
        this.Description = description;
        this.IsActive = true;
        this.CreatedAt = SystemClock.Instance.GetCurrentInstant();
        this.CreatedBy = createdBy;

        AddEvent(RbacCreatedDomainEvent.Create(Id, Name, Description, Permissions, IsActive));
    }

    public static RbacAggregate Create(Guid id, string name, string description, Guid createdBy)
    {
        DomainGuard.GuidIsEmpty(id, Errors.RbacIdIsInvalid);
        DomainGuard.IsNullOrEmpty(name, Errors.RbacNameIsInvalid);
        DomainGuard.IsNullOrEmpty(description, Errors.DescriptionRoleIsInvalid);
        DomainGuard.GuidIsEmpty(createdBy, Errors.CreatedByIsInvalid);

        return new RbacAggregate(id, name, description, createdBy);
    }

    public void Update(string name, string description, bool isActive, Guid updatedBy)
    {
        DomainGuard.IsNullOrEmpty(name, Errors.RbacNameIsInvalid);
        DomainGuard.IsNullOrEmpty(description, Errors.DescriptionRoleIsInvalid);
        DomainGuard.GuidIsEmpty(updatedBy, Errors.UpdatedByIsInvalid);

        this.Name = name;
        this.Description = description;
        this.IsActive = isActive;
        this.UpdatedAt = SystemClock.Instance.GetCurrentInstant();
        this.UpdatedBy = updatedBy;

        AddEvent(RbacUpdatedDomainEvent.Create(Id, Name, Description, Permissions, IsActive));
    }

    public void Delete(Guid deletedBy)
    {
        DomainGuard.GuidIsEmpty(deletedBy, Errors.DeletedByIsInvalid);

        this.IsDeleted = true;
        this.IsActive = false;
        this.DeletedAt = SystemClock.Instance.GetCurrentInstant();
        this.DeletedBy = deletedBy;

        AddEvent(RbacDeletedDomainEvent.Create(Id, Name, Description, Permissions, IsActive));
    }

    public void AddPermission(Guid id, Role role, Resource resource, Guid updatedBy)
    {
        DomainGuard.GuidIsEmpty(id, Errors.PermissionIdIsInvalid);
        DomainGuard.IsNull(role, Errors.RoleIsInvalid);
        DomainGuard.IsNull(resource, Errors.ResourceIsInvalid);

        var permission = new RbacPermissionEntity
        {
            Id = id,
            Role = role,
            Resource = resource
        };

        this.Permissions.Add(permission);
        this.UpdatedAt = SystemClock.Instance.GetCurrentInstant();
        this.UpdatedBy = updatedBy;

        AddEvent(PermissionAddedDomainEvent.Create(Id, permission.Id, permission.Role, permission.Resource));
    }

    public void UpdatePermission(Guid idPermission, Role role, Resource resource, Guid updatedBy)
    {
        DomainGuard.GuidIsEmpty(idPermission, Errors.PermissionIdIsInvalid);
        DomainGuard.IsNull(role, Errors.RoleIsInvalid);
        DomainGuard.IsNull(resource, Errors.ResourceIsInvalid);
        DomainGuard.GuidIsEmpty(updatedBy, Errors.UpdatedByIsInvalid);

        var permission = this.Permissions.FirstOrDefault(x => x.Id == idPermission);

        DomainGuard.IsNull(permission, Errors.PermissionNotFound);

        permission.Role = role;
        permission.Resource = resource;

        this.UpdatedAt = SystemClock.Instance.GetCurrentInstant();
        this.UpdatedBy = updatedBy;

        AddEvent(PermissionUpdatedDomainEvent.Create(Id, permission.Id, permission.Role, permission.Resource));
    }

    public void RemovePermission(Guid idPermission, Guid updatedBy)
    {
        DomainGuard.GuidIsEmpty(idPermission, Errors.PermissionIdIsInvalid);

        var permission = this.Permissions.FirstOrDefault(x => x.Id == idPermission);

        DomainGuard.IsNull(permission, Errors.PermissionNotFound);

        this.Permissions.Remove(permission);

        this.UpdatedAt = SystemClock.Instance.GetCurrentInstant();
        this.UpdatedBy = updatedBy;

        AddEvent(PermissionRemovedDomainEvent.Create(Id, permission.Id, permission.Role, permission.Resource));
    }
}
