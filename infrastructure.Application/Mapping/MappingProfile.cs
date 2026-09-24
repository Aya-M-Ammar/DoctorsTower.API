
using AutoMapper;
using DoctorsTower.Application.DTOs;
using DoctorsTower.Domain.Entities;
using System;
using System.Collections.Generic;
using System.Text;

namespace DoctorsTower.Application.Mapping
{
    public class MappingProfile : Profile
    {
        public MappingProfile()
        {
            CreateMap<Doctor, DoctorDTO>().ReverseMap();
            CreateMap<Doctor, CreateDoctorDTO>().ReverseMap();

            CreateMap<CreatePatientDTO, Patient>().ReverseMap();
            CreateMap<PatientDTO, Patient>().ReverseMap();
            CreateMap<ScheduleDTO, Schedule>().ReverseMap();
            CreateMap<CreateScheduleDTO, Schedule>().ReverseMap();

            CreateMap<AppointmentDTO, Appointment>().ReverseMap();
            

        }
    }
}
