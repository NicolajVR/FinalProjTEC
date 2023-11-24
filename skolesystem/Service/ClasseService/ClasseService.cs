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
            // Kald repository-metode for at hente alle Classs,fra databasen
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
            // Kald repository-metode for at hente et specifikt Classs,fra databasen
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
            // Opret et nyt Class-objekt ved hjælp af værdier fra NewClass
            Classe Classe = new Classe
            {
                class_name = newClasse.className,
                location = newClasse.location
            };
            // Indsæt den nye Class i repository 
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
            // Opret et nyt Class-objekt med opdaterede oplysninger
            Classe Classe = new Classe
            {
                class_name = updateClasse.className,
                location = updateClasse.location
            };
            // Opdater den eksisterende Class i repository'en asynkront
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
            // Anmod om sletning af Class fra repository
            var result = await _ClasseRepository.DeleteClasse(ClasseId);
            if (result != null) return true;
            // Returner true, hvis sletningen var vellykket
            else return false;
        }
    }
}

