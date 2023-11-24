using System;
using Microsoft.EntityFrameworkCore;
using skolesystem.Data;
using skolesystem.Models;

namespace skolesystem.Repository.AssignmentRepository
{
	public class AssignmentRepository : IAssignmentRepository
	{
        private readonly AssignmentDbContext _context;


        public AssignmentRepository(AssignmentDbContext context)
        {
            _context = context;
        }

        public async Task<Assignment> DeleteAssignment(int assignmentId)
        {
            // Find assignment i databasen baseret på assignmentId
            Assignment deleteAssignment = await _context.Assignments
.FirstOrDefaultAsync(Assignment => assignmentId == Assignment.assignment_id);
            // Hvis assignment findes, marker den som slettet og gem ændringerne i databasen
            if (deleteAssignment != null)
            {
                
                    deleteAssignment.is_Deleted = true;
                    _context.Entry(deleteAssignment).State = EntityState.Modified;
      
                await _context.SaveChangesAsync();
            }
            return deleteAssignment;
        }

        public async Task<Assignment> InsertNewAssignment(Assignment assignment)
        {
            // Tilføj den nye assignment til konteksten
            _context.Assignments.Add(assignment);
            // Gem ændringerne i databasen
            await _context.SaveChangesAsync();
            return assignment;
        }

        public async Task<Assignment> UpdateExistingAssignment(int assignmentId, Assignment assignment)
        {
            // Find den eksisterende assignment i databasen baseret på assignmentId
            Assignment updateAssignment = await _context.Assignments
                .FirstOrDefaultAsync(assignment => assignment.assignment_id == assignmentId);
            // Hvis assignment findes, opdater dens attributter og gem ændringerne i databasen
            if (updateAssignment != null)
            {
                updateAssignment.assignment_description = assignment.assignment_description;
                updateAssignment.assignment_deadline = assignment.assignment_deadline;
                await _context.SaveChangesAsync();
            }
            return updateAssignment;
        }

        public async Task<List<Assignment>> SelectAllAssignment()
        {
            // Hent alle assignmenter fra databasen
            return await _context.Assignments.Include(p => p.Classe).Include(a => a.Subjects).ToListAsync();

        }

        public async Task<Assignment> SelectAssignmentById(int assignmentId)
        {
            // Hent en enkelt assignment fra databasen baseret på assignmentId
            return await _context.Assignments
            .Include(p => p.Classe).Include(b => b.Subjects).
                FirstOrDefaultAsync(a => a.assignment_id == assignmentId);
        }
    }
}