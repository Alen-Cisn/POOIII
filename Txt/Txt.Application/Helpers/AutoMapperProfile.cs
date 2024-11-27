using AutoMapper;

using Txt.Domain.Entities;
using Txt.Shared.Dtos;

namespace Txt.Application.Helpers;

public class AutoMapperProfile : Profile
{
    public AutoMapperProfile()
    {
        CreateMap<Note, NoteDto>()
            .ForMember(dest => dest.Id, opt => opt.MapFrom(src => src.Id))
            .ForMember(dest => dest.Name, opt => opt.MapFrom(src => src.Name))
            .ForMember(dest => dest.Lines, opt => opt.MapFrom(src => src.Lines))
            .ForMember(dest => dest.Parent, opt => opt.MapFrom(src => src.Parent))
            .ForMember(dest => dest.ParentId, opt => opt.MapFrom(src => src.ParentId));
        CreateMap<Folder, FolderDto>()
            .ForMember(dest => dest.Id, opt => opt.MapFrom(src => src.Id))
            .ForMember(dest => dest.Name, opt => opt.MapFrom(src => src.Name))
            .ForMember(dest => dest.Parent, opt => opt.MapFrom(src => src.Parent))
            .ForMember(dest => dest.ParentId, opt => opt.MapFrom(src => src.ParentId));
        CreateMap<NoteLine, NoteLineDto>()
            .ForMember(dest => dest.Id, opt => opt.MapFrom(src => src.Id))
            .ForMember(dest => dest.Note, opt => opt.Ignore())
            .ForMember(dest => dest.NoteId, opt => opt.MapFrom(src => src.NoteId))
            .ForMember(dest => dest.Content, opt => opt.MapFrom(src => src.Content))
            .ForMember(dest => dest.OrderIndex, opt => opt.MapFrom(src => src.OrderIndex));
    }
}