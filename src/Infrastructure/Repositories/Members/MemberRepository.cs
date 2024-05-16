using Application.Exceptions.IdentityError;
using Application.Exceptions.Members;
using Application.Interfaces;

using Domain.Entities;
using Domain.Entities.Identity;
using Domain.Repositories;

using Microsoft.AspNetCore.Identity;
using Microsoft.EntityFrameworkCore;

using Persistence;

namespace Infrastructure.Repositories.Members;

public class MemberRepository : IMemberRepository
{
    private readonly IDashboardProgrammeDbContext _context;
    private readonly UserManager<User> _userManager;

    public MemberRepository(IDashboardProgrammeDbContext context, UserManager<User> userManager)
    {
        _context = context;
        _userManager = userManager;
    }

    public int GetMemberCount()
    {
        return _context.Members.Count();
    }

    public List<Member> GetAllWithUserEmail(string userEmail)
    {
        return _context.Members
            .Include(x => x.User)
            .Where(x => x.User.Email == userEmail)
            .AsNoTracking()
            .ToList();
    }

    public List<Member> GetAll()
    {
        return _context.Members
            .Include(x => x.User)
            .ThenInclude(x => x.UserRoles)
            .ThenInclude(x => x.Role)
            .ToList();
    }

    public async Task<Member> CreateMember(string firstName, string lastName, string email, string password)
    {
        if (_context.Members.Any(x => x.User.Email != null && x.User.Email.ToUpper().Trim() == email.ToUpper().Trim()))
        {
            throw new MemberWithEmailAlreadyExistsException($"A member with email {email} already exists.");
        }

        string trimmedEmail = email.Trim().ToLower();
        string normalizedEmail = trimmedEmail.Normalize();
        string trimmedFirstName = firstName.Trim();
        string trimmedLastName = lastName.Trim();
        string trimmedPassword = password.Trim();

        User user = new()
        {
            Email = trimmedEmail,
            UserName = trimmedEmail,
            NormalizedEmail = normalizedEmail,
            NormalizedUserName = normalizedEmail,
            EmailConfirmed = true,
            TwoFactorEnabled = false
        };

        IdentityResult? result = await _userManager.CreateAsync(user, trimmedPassword);

        if (!result.Succeeded)
        {
            throw new AggregateException(result.Errors.Select(x => x.ToException()));
        }

        Member? existingMember = _context.Members.IgnoreQueryFilters().FirstOrDefault(x => x.User.Id == user.Id);
        if (existingMember is not null)
        {
            throw new Exception("Le Member existe déjà pour le User.");
        }

        Member newMember = new(trimmedFirstName, trimmedLastName);
        newMember.SetUser(user);
        newMember.Activate();
        _context.Members.Add(newMember);
        await _context.SaveChangesAsync();

        return newMember;
    }

    public async Task UpdateMember(Member member)
    {
        string trimmedEmail = member.Email.Trim().ToLower();
        string normalizedEmail = trimmedEmail.Normalize();
        string trimmedFirstName = member.FirstName.Trim();
        string trimmedLastName = member.LastName.Trim();

        member.User.Email = trimmedEmail;
        member.User.NormalizedEmail = normalizedEmail;
        member.User.UserName = trimmedEmail;
        member.User.NormalizedUserName = normalizedEmail;
        member.SetFirstName(trimmedFirstName);
        member.SetLastName(trimmedLastName);

        if (_context.Members.AsQueryable().Any(x => x.User.Email == member.User.Email && x.Id != member.Id))
        {
            throw new MemberWithEmailAlreadyExistsException($"A member with email {member.Email} already exists.");
        }

        _context.Members.Update(member);
        await _context.SaveChangesAsync();
    }

    public async Task ClearRoles(Member member)
    {
        IList<string> lstRoles = await _userManager.GetRolesAsync(member.User);
        await _userManager.RemoveFromRolesAsync(member.User, lstRoles);
        member.User.ClearRoles();
        await _context.SaveChangesAsync();
    }

    public async Task AddRoleToMember(Member member, string role)
    {
        await _userManager.AddToRoleAsync(member.User, role);
    }

    public Member FindById(Guid id)
    {
        Member? member = _context.Members
            .Include(x => x.User)
            .ThenInclude(x => x.UserRoles)
            .ThenInclude(x => x.Role)
            .Include(x => x.MemberProgrammes)
            .ThenInclude(mp => mp.Programme)
            .FirstOrDefault(x => x.Id == id);
        if (member == null)
        {
            throw new MemberNotFoundException($"No member with id {id} was found.");
        }

        return member;
    }

    public Member? FindByUserId(Guid userId, bool asNoTracking = true)
    {
        IQueryable<Member> query = _context.Members;
        if (asNoTracking)
        {
            query = query.AsNoTracking();
        }

        return query
            .Include(x => x.User)
            .ThenInclude(x => x.UserRoles)
            .ThenInclude(x => x.Role)
            .Include(x => x.MemberProgrammes)
            .ThenInclude(mp => mp.Programme)
            .FirstOrDefault(x => x.User.Id == userId);
    }

    public Member? FindByUserEmail(string userEmail)
    {
        return _context.Members
            .Include(x => x.User)
            .ThenInclude(x => x.UserRoles)
            .ThenInclude(x => x.Role)
            .FirstOrDefault(x => x.User.Email == userEmail);
    }
    
    public async Task DeleteMemberWithId(Guid id)
    {
        Member? member = _context.Members.FirstOrDefault(x => x.Id == id);

        if (member is null)
        {
            throw new MemberNotFoundException($"Could not find member with id {id}");
        }
        _context.Members.Remove(member);
        await _context.SaveChangesAsync();
    }

}