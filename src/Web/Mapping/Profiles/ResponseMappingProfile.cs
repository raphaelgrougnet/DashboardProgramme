using Application.Common;

using AutoMapper;

using Domain.Entities;
using Domain.Entities.Books;
using Domain.Entities.CoursNs;
using Domain.Entities.Etudiants;
using Domain.Entities.Programmes;
using Domain.Entities.SessionEtudes;

using Microsoft.AspNetCore.Identity;

using Web.Features.Admins.Books;
using Web.Features.Common;
using Web.Features.Cours;
using Web.Features.Etudiants;
using Web.Features.Members;
using Web.Features.Members.Me.GetMe;
using Web.Features.SessionEtudes;

using ProgrammeDto = Web.Features.Programmes.ProgrammeDto;

namespace Web.Mapping.Profiles;

public class ResponseMappingProfile : Profile
{
    public ResponseMappingProfile()
    {
        CreateMap<IdentityResult, SucceededOrNotResponse>();

        CreateMap<IdentityError, Error>()
            .ForMember(error => error.ErrorType, opt => opt.MapFrom(identity => identity.Code))
            .ForMember(error => error.ErrorMessage, opt => opt.MapFrom(identity => identity.Description));

        CreateMap<Member, GetMeResponse>()
            .ForMember(x => x.Roles, opt => opt.MapFrom(x => x.User.RoleNames))
            .ForMember(x => x.PhoneNumber, opt => opt.MapFrom(x => x.GetPhoneNumber()))
            .ForMember(x => x.PhoneExtension, opt => opt.MapFrom(x => x.GetPhoneExtension()));

        CreateMap<Book, BookDto>()
            .ForMember(bookDto => bookDto.Created, opt => opt.MapFrom(book => book.Created.ToDateTimeUtc()))
            .ForMember(bookDto => bookDto.NameFr, opt => opt.MapFrom(book => book.NameFr))
            .ForMember(bookDto => bookDto.NameEn, opt => opt.MapFrom(book => book.NameEn))
            .ForMember(bookDto => bookDto.DescriptionFr, opt => opt.MapFrom(book => book.DescriptionFr))
            .ForMember(bookDto => bookDto.DescriptionEn, opt => opt.MapFrom(book => book.DescriptionEn));

        CreateMap<Programme, ProgrammeDto>()
            .ForMember(programmeDto => programmeDto.Id, opt => opt.MapFrom(programme => programme.Id))
            .ForMember(programmeDto => programmeDto.Nom, opt => opt.MapFrom(programme => programme.Nom))
            .ForMember(programmeDto => programmeDto.Numero, opt => opt.MapFrom(programme => programme.Numero));

        CreateMap<Member, MemberDto>()
            .ForMember(memberDto => memberDto.Roles, opt => opt.MapFrom(member => member.User.RoleNames))
            .ForMember(memberDto => memberDto.Programmes, opt =>
                opt.MapFrom(member => member.MemberProgrammes.Select(mp => mp.Programme.Id).ToList()));

        CreateMap<SessionEtude, SessionEtudeDto>()
            .ForMember(sessionEtudeDto => sessionEtudeDto.Id, opt => opt.MapFrom(sessionEtude => sessionEtude.Id))
            .ForMember(sessionEtudeDto => sessionEtudeDto.Annee, opt => opt.MapFrom(sessionEtude => sessionEtude.Annee))
            .ForMember(sessionEtudeDto => sessionEtudeDto.Saison,
                opt => opt.MapFrom(sessionEtude => sessionEtude.Saison.ToString()))
            .ForMember(sessionEtudeDto => sessionEtudeDto.Slug, opt => opt.MapFrom(sessionEtude => sessionEtude.Slug))
            .ForMember(sessionEtudeDto => sessionEtudeDto.Ordre,
                opt => opt.MapFrom(sessionEtude => sessionEtude.Ordre));

        CreateMap<Cours, CoursDto>()
            .ForMember(coursDto => coursDto.Id, opt => opt.MapFrom(cours => cours.Id))
            .ForMember(coursDto => coursDto.Nom, opt => opt.MapFrom(cours => cours.Nom))
            .ForMember(coursDto => coursDto.Code, opt => opt.MapFrom(cours => cours.Code));


        CreateMap<Etudiant, EtudiantsDto>()
            .ForMember(etudiantDto => etudiantDto.Id, opt => opt.MapFrom(etudiant => etudiant.Id));
    }
}