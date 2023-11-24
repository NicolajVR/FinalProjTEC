using AutoMapper;
using skolesystem.DTOs;
using skolesystem.Models;

public class MappingProfiles : Profile
{
    // Konstruktør til konfiguration af AutoMapper-profiler, der definerer mapping mellem DTO'er og domænemodeller.
    public MappingProfiles()
    {
        CreateMap<UserCreateDto, Users>();
        CreateMap<Users, UserReadDto>();
        CreateMap<UserUpdateDto, Users>();
        CreateMap<Users, UserUpdateDto>();
        CreateMap<UserReadDto, Users>();


        CreateMap<Absence, AbsenceReadDto>();
        CreateMap<AbsenceCreateDto, Absence>();
        CreateMap<AbsenceUpdateDto, Absence>();
        CreateMap<AbsenceReadDto, Absence>();

        CreateMap<User_information, User_informationReadDto>();
        CreateMap<User_information, User_informationUpdateDto>();
        CreateMap<User_information, User_informationCreateDto>();
        CreateMap<User_informationUpdateDto, User_information>();

    }
}