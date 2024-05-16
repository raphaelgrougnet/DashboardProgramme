using Domain.Common;
using Domain.Helpers;

using Microsoft.AspNetCore.Identity;

using NodaTime;

namespace Domain.Entities.Identity;

public class User : IdentityUser<Guid>, ISoftDeletable
{
    public Instant Created { get; private set; }
    public string? CreatedBy { get; private set; }

    public Instant? LastModified { get; private set; }
    public string? LastModifiedBy { get; private set; }

    public int? PhoneExtension { get; set; }

    public List<UserRole> UserRoles { get; } = [];
    public List<string> RoleNames => UserRoles.Select(x => x.Role.Name).ToList();

    public Instant? Deleted { get; private set; }
    public string? DeletedBy { get; private set; }

    public void Restore()
    {
        Deleted = null;
        DeletedBy = null;
    }

    public void SoftDelete(string? deletedBy = null)
    {
        Deleted = InstantHelper.GetLocalNow();
        DeletedBy = deletedBy;
    }

    public void Activate(string firstName)
    {
        Restore();
    }

    public void AddRole(Role role)
    {
        if (UserRoles.Any(x => x.Role.Id == role.Id))
        {
            return;
        }

        UserRoles.Add(new UserRole { User = this, Role = role });
    }

    public void ClearRoles()
    {
        UserRoles.Clear();
    }

    public string? GetPhoneNumberWithExtension()
    {
        return PhoneNumberHelper.AddExtensionToPhoneNumber(PhoneNumber, PhoneExtension);
    }

    public bool HasRole(string role)
    {
        return RoleNames.Contains(role);
    }

    public bool IsActive()
    {
        return Deleted == null && DeletedBy == null;
    }

    public void SetDeletedBy(string deletedBy)
    {
        DeletedBy = deletedBy;
    }

    public void SetPhoneExtension(int? phoneExtension)
    {
        PhoneExtension = phoneExtension;
    }

    public void UpdateCreated(Instant created, string createdBy)
    {
        Created = created;
        CreatedBy = createdBy;
    }

    public void UpdateLastModified(Instant lastModified, string lastModifiedBy)
    {
        LastModified = lastModified;
        LastModifiedBy = lastModifiedBy;
    }
}