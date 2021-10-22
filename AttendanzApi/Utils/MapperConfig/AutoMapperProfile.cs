using System;
using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;
using AutoMapper;
using AttendanzApi.Models;
using AttendanzApi.Dtos;

namespace AttendanzApi.Utils.MapperConfig
{
    public class AutoMapperProfile : Profile
    {
        public AutoMapperProfile()
        {
            CreateMap<SubjectDto, SubjectModel>()
                .ForMember(dest => dest.Id, opt => opt.Ignore())
                .ReverseMap();

            CreateMap<GroupDto, GroupModel>()
                .ForMember(dest => dest.Id, opt => opt.Ignore())
                //.ForMember(dest => dest.StartTime, opt => opt.MapFrom(src => src.StartTime))
                //.ForMember(dest => dest.EndTime, opt => opt.MapFrom(src => src.EndTime))
                .ReverseMap();
                //.ForMember(dest => dest.StartTime, opt => opt.MapFrom(src => src.StartTime))
                //.ForMember(dest => dest.EndTime, opt => opt.MapFrom(src => src.EndTime));

            //CreateMap<TimeSpan, TimeSpan>();

            CreateMap<StudentDto, StudentModel>()
                .ForMember(dest => dest.Id, opt => opt.Ignore())
                .ReverseMap();

            CreateMap<ClassDto, ClassModel>()
                .ForMember(dest => dest.Id, opt => opt.Ignore())
                .ForMember(dest => dest.Date, opt => opt.Ignore())
                .ReverseMap()
                .ForMember(dest => dest.Date, opt => opt.MapFrom(src => src.Date.ToString("dd.MM.yyyy")));

            CreateMap<GroupStudentModel, StudentDto>()
                .ForMember(dest => dest.Id, opt => opt.MapFrom(src => src.Student.Id))
                .ForMember(dest => dest.Firstname, opt => opt.MapFrom(src => src.Student.Firstname))
                .ForMember(dest => dest.Lastname, opt => opt.MapFrom(src => src.Student.LastName))
                .ForMember(dest => dest.IndexNumber, opt => opt.MapFrom(src => src.Student.IndexNumber));

            CreateMap<ScannerDto, ScannerModel>()
                .ForMember(dest => dest.Id, opt => opt.Ignore())
                .ForMember(dest => dest.Key, opt => opt.Ignore())
                .ForMember(dest => dest.IsConfirmed, opt => opt.Ignore())
                .ReverseMap();

            CreateMap<PresenceDto, PresenceModel>()
                .ForMember(dest => dest.Id, opt => opt.Ignore())
                .ReverseMap();
            //.ForMember(dest => dest.Class, opt => opt.MapFrom(src => src.Class));
            /*
            .ForMember(dest => dest.Student.Firstname, opt => opt.MapFrom(src => src.GroupStudent.Student.Firstname))
            .ForMember(dest => dest.Student.Lastname, opt => opt.MapFrom(src => src.GroupStudent.Student.LastName))
            .ForMember(dest => dest.Student.IndexNumber, opt => opt.MapFrom(src => src.GroupStudent.Student.IndexNumber));
            */

            CreateMap<PresenceModel, StudentPresenceDto>();

            CreateMap<PresenceModel, ClassPresenceDto>()
                .ForMember(dest => dest.Student, opt => opt.MapFrom(src => src.GroupStudent));

            CreateMap<ControlProcessDto, ControlProcessModel>()
                //.ForMember(dest => dest.ScannerId, opt => opt.Ignore())
                .ReverseMap();

            CreateMap<AccountDto, AccountModel>()
                .ForMember(dest => dest.Id, opt => opt.Ignore())
                .ReverseMap()
                .ForMember(dest => dest.Password, opt => opt.Ignore());
        }
    }
}
