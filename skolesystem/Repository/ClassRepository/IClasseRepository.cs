using System;
using skolesystem.Models;

namespace skolesystem.Repository.ClasseRepository
{
    public interface IClasseRepository
    {
        Task<List<Classe>> SelectAllClasse();
        Task<Classe> SelectClasseById(int classId);
        Task<Classe> InsertNewClasse(Classe classe);
        Task<Classe> UpdateExistingClasse(int classId, Classe classe);
        Task<Classe> DeleteClasse(int classId);
    }
}
