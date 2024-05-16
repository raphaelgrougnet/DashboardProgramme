﻿namespace Web.Features.Members.Me.GetMe;

public class GetMeResponse
{
    public string FirstName { get; set; } = default!;
    public string LastName { get; set; } = default!;
    public string Email { get; set; } = default!;
    public string? PhoneNumber { get; set; }
    public int? PhoneExtension { get; set; }
    public int? Apartment { get; set; }
    public string? Street { get; set; }
    public string? City { get; set; }
    public string? ZipCode { get; set; }
    public List<string> Roles { get; set; } = default!;
}