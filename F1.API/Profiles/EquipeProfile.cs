using AutoMapper;
using F1.Lib.Modelos;
using F1.API.Data.Dtos.EquipeDTO;

namespace F1.API.Profiles;

public class EquipeProfile : Profile
{
    public EquipeProfile()
    {
        CreateMap<CreateEquipeDTO, Equipe>();
        CreateMap<UpdateEquipeDTO, Equipe>();
        CreateMap<Equipe, CreateEquipeDTO>();
        CreateMap<Equipe, UpdateEquipeDTO>();

        CreateMap<Equipe, ReadEquipeDTO>()
            .ForMember(dest => dest.Pilotos, opt => opt.MapFrom(src =>
                src.Pilotos.Select(p => p.Nome).ToList()));
    }
}