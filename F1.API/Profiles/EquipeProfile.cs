using AutoMapper;
using F1.Lib.Modelos;
using F1.API.Data.Dtos.EquipeDTO;

namespace F1.API.Profiles;

public class EquipeProfile : Profile
{
    public EquipeProfile()
    {
        CreateMap<Equipe, CreateEquipeDTO>();
        CreateMap<Equipe, UpdateEquipeDTO>();
        CreateMap<ReadEquipeDTO, UpdateEquipeDTO>();

        CreateMap<CreateEquipeDTO, Equipe>()
                .ForMember(dest => dest.Id, opt => opt.Ignore())      
                .ForMember(dest => dest.Pilotos, opt => opt.Ignore());

        CreateMap<Equipe, ReadEquipeDTO>()
            .ForMember(dest => dest.Pilotos, opt => opt.MapFrom(src =>
                src.Pilotos.Select(p => p.Nome).ToList()));

        CreateMap<UpdateEquipeDTO, Equipe>()
                .ForMember(dest => dest.Id, opt => opt.Ignore())
                .ForMember(dest => dest.Pilotos, opt => opt.Ignore());
    }
}