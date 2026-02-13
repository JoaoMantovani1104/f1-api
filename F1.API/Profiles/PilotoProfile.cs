using AutoMapper;
using F1.Lib.Modelos;
using F1.API.Data.Dtos.PilotoDTO;

namespace F1.API.Profiles;

public class PilotoProfile : Profile
{
    public PilotoProfile()
    {
        CreateMap<CreatePilotoDTO, Piloto>();
        CreateMap<UpdatePilotoDTO, Piloto>();

        CreateMap<Piloto, CreatePilotoDTO>();
        CreateMap<Piloto, UpdatePilotoDTO>();

        CreateMap<Piloto, ReadPilotoDTO>()
            .ForMember(dest => dest.NomeEquipe, opt => opt.MapFrom(src => src.Equipe.Nome))
            .ForMember(dest => dest.GpsVencidos, opt => opt.MapFrom(src => 
                src.GpsVencidos.Select(gp => gp.Nome).ToList()));
    }
}