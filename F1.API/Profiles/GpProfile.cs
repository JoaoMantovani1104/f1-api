using AutoMapper;
using F1.Lib.Modelos;
using F1.API.Data.Dtos.GrandePremioDTO;

namespace F1.API.Profiles;

public class GpProfile : Profile
{
    public GpProfile()
    {
        CreateMap<CreateGpDTO, GrandePremio>();
        CreateMap<UpdateGpDTO, GrandePremio>();

        CreateMap<GrandePremio, CreateGpDTO>();
        CreateMap<GrandePremio, UpdateGpDTO>();

        CreateMap<GrandePremio, ReadGpDTO>()
            .ForMember(dest => dest.Vencedor, opt => opt.MapFrom(src => src.Vencedor.Nome));
    }
}