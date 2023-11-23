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
            Classe deleteClasse = await _context.Classe
                .FirstOrDefaultAsync(Classe => ClasseId == Classe.class_id);

            if (deleteClasse != null)
            {
                _context.Classe.Remove(deleteClasse);
                await _context.SaveChangesAsync();
            }
            return deleteClasse;
        }


        public async Task<Classe> InsertNewClasse(Classe Classe)
        {
            _context.Classe.Add(Classe);
            await _context.SaveChangesAsync();
            return Classe;
        }

        public async Task<Classe> UpdateExistingClasse(int ClasseId, Classe Classe)
        {
            Classe updateClasse = await _context.Classe
                .FirstOrDefaultAsync(Classe => Classe.class_id == ClasseId);
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
            return await _context.Classe.ToListAsync();
        }

        public async Task<Classe> SelectClasseById(int ClasseId)
        {
            return await _context.Classe.FirstOrDefaultAsync(a => a.class_id == ClasseId);
        }
    }
}

