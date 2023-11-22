using AutoMapper;
using skolesystem.Data;
using skolesystem.Models;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;
using Microsoft.EntityFrameworkCore;

namespace skolesystem.Repository
{
    // Repository for data access
    public interface IAbsenceRepository
    {
        Task<Absence> GetById(int id);
        Task<IEnumerable<Absence>> GetAll();
        Task<IEnumerable<Absence>> GetDeletedAbsences();
        Task AddAbsence(Absence absence);
        Task UpdateAbsence(int id, Absence absence);
        Task SoftDeleteAbsence(int id);
    }

    public class AbsenceRepository : IAbsenceRepository
    {
        private readonly AbsenceDbContext _context;
        private readonly IMapper _mapper;

        public AbsenceRepository(AbsenceDbContext context, IMapper mapper)
        {
            _context = context;
            _mapper = mapper;
        }

        public async Task<Absence> GetById(int id)
        {
            return await _context.Absence.FindAsync(id);
        }

        public async Task<IEnumerable<Absence>> GetAll()
        {
            return await _context.Absence.ToListAsync();
        }

        public async Task<IEnumerable<Absence>> GetDeletedAbsences()
        {
            return await _context.Absence.Where(a => a.is_deleted).ToListAsync();
        }

        public async Task AddAbsence(Absence absence)
        {
            _context.Absence.Add(absence);
            await _context.SaveChangesAsync();
        }

        public async Task UpdateAbsence(int id, Absence absence)
        {
            var existingAbsence = await _context.Absence.FindAsync(id);

            if (existingAbsence == null)
            {
                throw new ArgumentException("Absence not found");
            }

            existingAbsence.user_id = absence.user_id;
            existingAbsence.teacher_id = absence.teacher_id;
            existingAbsence.class_id = absence.class_id;
            existingAbsence.absence_date = absence.absence_date;
            existingAbsence.reason = absence.reason;

            await _context.SaveChangesAsync();
        }

        public async Task SoftDeleteAbsence(int id)
        {
            var absenceToDelete = await _context.Absence.FindAsync(id);

            if (absenceToDelete != null)
            {
                absenceToDelete.is_deleted = true;
                _context.Entry(absenceToDelete).State = EntityState.Modified;
                await _context.SaveChangesAsync();
            }
        }
    }
}