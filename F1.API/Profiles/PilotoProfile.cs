using AutoMapper;
using F1.Lib.Modelos;
using F1.API.Data.Dtos.PilotoDTO;

namespace F1.API.Profiles;

public class PilotoProfile : Profile
{
    public PilotoProfile()
    {
        CreateMap<Piloto, CreatePilotoDTO>();
        CreateMap<Piloto, UpdatePilotoDTO>();
        CreateMap<ReadPilotoDTO, UpdatePilotoDTO>();

        CreateMap<Piloto, ReadPilotoDTO>()
            .ForMember(dest => dest.NomeEquipe, opt => opt.MapFrom(src => src.Equipe.Nome))
            .ForMember(dest => dest.GpsVencidos, opt => opt.MapFrom(src => 
                src.GpsVencidos.Select(gp => gp.Nome).ToList()));

        CreateMap<CreatePilotoDTO, Piloto>()
                .ForMember(dest => dest.Id, opt => opt.Ignore())
                .ForMember(dest => dest.Equipe, opt => opt.Ignore())      
                .ForMember(dest => dest.GpsVencidos, opt => opt.Ignore());

        CreateMap<UpdatePilotoDTO, Piloto>()
            .ForMember(dest => dest.Id, opt => opt.Ignore())
            .ForMember(dest => dest.Equipe, opt => opt.Ignore())
            .ForMember(dest => dest.GpsVencidos, opt => opt.Ignore());
        }
}