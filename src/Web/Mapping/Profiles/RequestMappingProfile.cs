using AutoMapper;

using Domain.Common;
using Domain.Entities.Books;
using Domain.Entities.Programmes;

using Web.Dtos;
using Web.Features.Admins.Books.CreateBook;
using Web.Features.Admins.Books.EditBook;
using Web.Features.Admins.Programmes.CreateProgramme;
using Web.Features.Admins.Programmes.EditProgramme;

namespace Web.Mapping.Profiles;

public class RequestMappingProfile : Profile
{
    public RequestMappingProfile()
    {
        CreateMap<TranslatableStringDto, TranslatableString>().ReverseMap();

        CreateMap<CreateBookRequest, Book>();

        CreateMap<EditBookRequest, Book>();

        CreateMap<CreateProgrammeRequest, Programme>();

        CreateMap<EditProgrammeRequest, Programme>();
    }
}