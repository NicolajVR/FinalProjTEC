using System;
using Microsoft.EntityFrameworkCore;
using skolesystem.Data;
using skolesystem.Models;

namespace skolesystem.Repository.ClasseRepository
{
    public class ClasseRepository : IClasseRepository
    {
        private readonly ClasseDbContext _context;


        public ClasseRepository(ClasseDbContext context)
        {
            _context = context;
        }

        public async Task<Classe> DeleteClasse(int ClasseId)
        {
            // Find Class i databasen baseret på ClassId
            Classe deleteClasse = await _context.Classe
                .FirstOrDefaultAsync(Classe => ClasseId == Classe.class_id);
            // Hvis Class findes, marker den som slettet og gem ændringerne i databasen
            if (deleteClasse != null)
            {
                _context.Classe.Remove(deleteClasse);
                await _context.SaveChangesAsync();
            }
            return deleteClasse;
        }

        public async Task<Classe> InsertNewClasse(Classe Classe)
        {
            // Tilføj den nye Class til konteksten
            _context.Classe.Add(Classe);
            // Gem ændringerne i databasen
            await _context.SaveChangesAsync();
            return Classe;
        }

        public async Task<Classe> UpdateExistingClasse(int ClasseId, Classe Classe)
        {
            // Find den eksisterende Class i databasen baseret på ClassId
            Classe updateClasse = await _context.Classe
                .FirstOrDefaultAsync(Classe => Classe.class_id == ClasseId);
            // Hvis Class findes, opdater dens attributter og gem ændringerne i databasen
            if (updateClasse != null)
            {
                updateClasse.class_name = Classe.class_name;
                updateClasse.location = Classe.location;

                await _context.SaveChangesAsync();
            }
            return updateClasse;
        }

        public async Task<List<Classe>> SelectAllClasse()
        {
            // Hent alle Classer fra databasen inklusiv hvem der har afleveret
            return await _context.Classe.ToListAsync();
        }

        public async Task<Classe> SelectClasseById(int ClasseId)
        {
            // Hent en enkelt Class fra databasen baseret på ClassId
            return await _context.Classe.FirstOrDefaultAsync(a => a.class_id == ClasseId);
        }
    }
}
