using System;
using Microsoft.EntityFrameworkCore;
using skolesystem.Data;
using skolesystem.Models;

namespace skolesystem.Repository.SubjectRepository
{
	public class SubjectRepository : ISubjectRepository
	{
        private readonly SubjectsDbContext _context;


        public SubjectRepository(SubjectsDbContext context)
        {
            _context = context;
        }

        public async Task<Subjects> DeleteSubject(int SubjectId)
        {
            // Find subject i databasen baseret på subjectId
            Subjects deleteSubject = await _context.Subjects
                .FirstOrDefaultAsync(Subject => SubjectId == Subject.subject_id);
            // Hvis subject findes, marker den som slettet og gem ændringerne i databasen
            if (deleteSubject != null)
            {
                _context.Subjects.Remove(deleteSubject);
                await _context.SaveChangesAsync();
            }
            return deleteSubject;
        }

        public async Task<Subjects> InsertNewSubject(Subjects Subject)
        {
            // Tilføj den nye subject til konteksten
            _context.Subjects.Add(Subject);
            // Gem ændringerne i databasen
            await _context.SaveChangesAsync();
            return Subject;
        }

        public async Task<Subjects> UpdateExistingSubject(int SubjectId, Subjects Subject)
        {
            // Find den eksisterende subject i databasen baseret på subjectId
            Subjects updateSubject = await _context.Subjects
                .FirstOrDefaultAsync(Subject => Subject.subject_id == SubjectId);
            // Hvis subject findes, opdater dens attributter og gem ændringerne i databasen
            if (updateSubject != null)
            {
                updateSubject.subject_name = Subject.subject_name;
                await _context.SaveChangesAsync();
            }
            return updateSubject;
        }

        public async Task<List<Subjects>> SelectAllSubject()
        {
            // Hent alle subjecter fra databasen inklusiv hvem der har afleveret
            return await _context.Subjects.ToListAsync();
        }

        public async Task<Subjects> SelectSubjectById(int SubjectId)
        {
            // Hent en enkelt subject fra databasen baseret på subjectId
            return await _context.Subjects
                .FirstOrDefaultAsync(a => a.subject_id == SubjectId);
        }
    }
}

