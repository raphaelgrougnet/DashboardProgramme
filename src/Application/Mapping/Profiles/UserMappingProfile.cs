using AutoMapper;

using Domain.Entities.Identity;

namespace Application.Mapping.Profiles;

// ReSharper disable once ClassNeverInstantiated.Global
public class UserMappingProfile : Profile
{
    public UserMappingProfile()
    {
        CreateMap<User, User>()
            .ForMember(user => user.UserRoles, opt => opt.Ignore());
    }
}