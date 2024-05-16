using System.Security.Claims;

using Application.Interfaces.Services;

using Microsoft.AspNetCore.Http;

namespace Infrastructure.Services;

public class HttpContextUserService(IHttpContextAccessor httpContextAccessor) : IHttpContextUserService
{
    public string? UserEmail => httpContextAccessor.HttpContext?.User?.FindFirstValue(ClaimTypes.Email);
}