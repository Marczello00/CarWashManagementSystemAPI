using AutoMapper;
using CarWashManagementSystem.Dtos;
using CarWashManagementSystem.Models;

namespace CarWashManagementSystem.Helpers
{
    public class MappingProfiles : Profile
    {
        public MappingProfiles()
        {
            CreateMap<Transaction, TransactionDto>()
                .ForMember(dest => dest.Station, opt => opt.MapFrom(src => src.Station))
                .ForMember(dest => dest.TransactionSource, opt => opt.MapFrom(src => src.TransactionSource));

            CreateMap<Station, StationDto>()
                .ForMember(dest => dest.StationType, opt => opt.MapFrom(src => src.StationType));

            CreateMap<TransactionSource, TransactionSourceDto>();

            CreateMap<StationType, StationTypeDto>();
            CreateMap<Station, StationInfoDto>();
            CreateMap<Station, StationStatusDto>()
                .ForMember(dest => dest.IsFiscOn, opt => opt.MapFrom(src => src.ManualFiscState));
            CreateMap<UpdateStationDto, Station>();
            CreateMap<FiscSchedule, FiscScheduleDto>();
            CreateMap<UpdateFiscScheduleDto, FiscSchedule>();
            CreateMap<StationAllowedIp, StationAllowedIpDto>();
            CreateMap<UpdateStationAllowedIpDto, StationAllowedIp>();
        }
    }
}
