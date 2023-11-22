using System;
using skolesystem.DTOs.Classe.Request;
using skolesystem.DTOs.Classe.Response;
using skolesystem.Models;
using skolesystem.Repository.ClasseRepository;

namespace skolesystem.Service.ClasseService
{
	public class ClasseService : IClasseService
	{
        private readonly IClasseRepository _ClasseRepository;

        public ClasseService(IClasseRepository ClasseRepository)
        {
            _ClasseRepository = ClasseRepository;
        }

        public async Task<List<ClasseResponse>> GetAll()
        {
            List<Classe> Classe = await _ClasseRepository.SelectAllClasse();
            return Classe.Select(c => new ClasseResponse
            {
                Id = c.class_id,
                className = c.class_name,
                location = c.location,
                
            }).ToList();
        }


        public async Task<ClasseResponse> GetById(int ClasseId)
        {
            Classe Classe = await _ClasseRepository.SelectClasseById(ClasseId);
            return Classe == null ? null : new ClasseResponse
            {
                Id = Classe.class_id,
                className = Classe.class_name,
                location = Classe.location,
                
            };
        }
        public async Task<ClasseResponse> Create(NewClasse newClasse)
        {
            Classe Classe = new Classe
            {
                class_name = newClasse.className,
                location = newClasse.location
            };

            Classe = await _ClasseRepository.InsertNewClasse(Classe);

            return Classe == null ? null : new ClasseResponse
            {
                Id = Classe.class_id,
                className = Classe.class_name,
                location = Classe.location
            };


        }
        public async Task<ClasseResponse> Update(int ClasseId, UpdateClasse updateClasse)
        {
            Classe Classe = new Classe
            {
                class_name = updateClasse.className,
                location = updateClasse.location
            };

            Classe = await _ClasseRepository.UpdateExistingClasse(ClasseId, Classe);

            return Classe == null ? null : new ClasseResponse
            {
                Id = Classe.class_id,
                className = Classe.class_name,
                location = Classe.location
            };
        }
        public async Task<bool> Delete(int ClasseId)
        {
            var result = await _ClasseRepository.DeleteClasse(ClasseId);
            if (result != null) return true;
            else return false;
        }
    }
}

