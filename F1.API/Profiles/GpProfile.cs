using AutoMapper;
using F1.Lib.Modelos;
using F1.API.Data.Dtos.GrandePremioDTO;

namespace F1.API.Profiles;

public class GpProfile : Profile
{
    public GpProfile()
    {
        CreateMap<GrandePremio, CreateGpDTO>();
        CreateMap<GrandePremio, UpdateGpDTO>();
        CreateMap<ReadGpDTO, UpdateGpDTO>();

        CreateMap<GrandePremio, ReadGpDTO>()
            .ForMember(dest => dest.Vencedor, opt => opt.MapFrom(src => src.Vencedor.Nome));

        CreateMap<CreateGpDTO, GrandePremio>()
                .ForMember(dest => dest.Id, opt => opt.Ignore())
                .ForMember(dest => dest.Vencedor, opt => opt.Ignore()); 

        CreateMap<UpdateGpDTO, GrandePremio>()
            .ForMember(dest => dest.Id, opt => opt.Ignore())
            .ForMember(dest => dest.Vencedor, opt => opt.Ignore());
    }
}