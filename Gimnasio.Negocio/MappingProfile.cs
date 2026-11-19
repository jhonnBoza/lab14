using AutoMapper;
using Gimnasio.Entidades;
using Gimnasio.Entidades.Dtos;

namespace Gimnasio.Negocio;

public class MappingProfile : Profile
{
    public MappingProfile()
    {
        CreateMap<Plan, PlanDto>().ReverseMap();

        CreateMap<Miembro, MiembroDto>()
            .ForMember(d => d.NombrePlan, o => o.MapFrom(s => s.Plan != null ? s.Plan.Nombre : null));

        CreateMap<MiembroDto, Miembro>()
            .ForMember(d => d.Plan, o => o.Ignore());
    }
}
