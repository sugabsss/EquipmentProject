using AutoMapper;
using EquipmentManagement.Application.Dtos;
using EquipmentManagement.Domain.Entities;

namespace EquipmentManagement.Application.Mappers
{
    public class EquipmentProfile : Profile
    {
        public EquipmentProfile()
        {

            CreateMap<Equipment, EquipmentDto>();
            // If property names differ, configure them like this:
            // CreateMap<Equipment, EquipmentDto>()
            //     .ForMember(dest => dest.EquipmentName, opt => opt.MapFrom(src => src.Name));
        }
    }
}
