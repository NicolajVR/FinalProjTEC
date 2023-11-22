using AutoMapper;
using skolesystem.DTOs;
using skolesystem.Models;
using skolesystem.Repository;
using System;
using System.Collections.Generic;
using System.Threading.Tasks;

namespace skolesystem.Service
{
    // Service for business logic
    public interface IAbsenceService
    {
        Task<AbsenceReadDto> GetAbsenceById(int id);
        Task<IEnumerable<AbsenceReadDto>> GetAllAbsences();
        Task<IEnumerable<AbsenceReadDto>> GetDeletedAbsences();
        Task<AbsenceReadDto> CreateAbsence(AbsenceCreateDto absenceDto);
        Task UpdateAbsence(int id, AbsenceUpdateDto absenceDto);
        Task SoftDeleteAbsence(int id);
    }

    public class AbsenceService : IAbsenceService
    {
        private readonly IAbsenceRepository _absenceRepository;
        private readonly IMapper _mapper;

        public AbsenceService(IAbsenceRepository absenceRepository, IMapper mapper)
        {
            _absenceRepository = absenceRepository;
            _mapper = mapper;
        }

        public async Task<AbsenceReadDto> GetAbsenceById(int id)
        {
            var absence = await _absenceRepository.GetById(id);

            if (absence == null)
            {
                return null; // or throw an exception or handle accordingly
            }

            // Use your mapping logic here to convert Absence to AbsenceReadDto
            var absenceDto = _mapper.Map<AbsenceReadDto>(absence);

            return absenceDto;
        }

        public async Task<IEnumerable<AbsenceReadDto>> GetAllAbsences()
        {
            var absences = await _absenceRepository.GetAll();
            return _mapper.Map<IEnumerable<AbsenceReadDto>>(absences);
        }

        public async Task<IEnumerable<AbsenceReadDto>> GetDeletedAbsences()
        {
            var deletedAbsences = await _absenceRepository.GetDeletedAbsences();
            return _mapper.Map<IEnumerable<AbsenceReadDto>>(deletedAbsences);
        }

        public async Task<AbsenceReadDto> CreateAbsence(AbsenceCreateDto absenceDto)
        {
            var absence = new Absence
            {
                user_id = absenceDto.user_id,
                teacher_id = absenceDto.teacher_id,
                class_id = absenceDto.class_id,
                absence_date = absenceDto.absence_date,
                reason = absenceDto.reason
            };

            await _absenceRepository.AddAbsence(absence);

            // Map the created Absence entity to AbsenceReadDto before returning
            var createdAbsenceDto = _mapper.Map<AbsenceReadDto>(absence);

            return createdAbsenceDto;
        }

        public async Task UpdateAbsence(int id, AbsenceUpdateDto absenceDto)
        {
            var existingAbsence = await _absenceRepository.GetById(id);

            if (existingAbsence == null)
            {
                throw new ArgumentException("Absence not found");
            }

            _mapper.Map(absenceDto, existingAbsence);

            await _absenceRepository.UpdateAbsence(id, existingAbsence);
        }

        public async Task SoftDeleteAbsence(int id)
        {
            var absenceToDelete = await _absenceRepository.GetById(id);

            if (absenceToDelete == null)
            {
                throw new ArgumentException("Absence not found");
            }

            await _absenceRepository.SoftDeleteAbsence(id);
        }
    }
}