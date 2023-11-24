using System;
using skolesystem.DTOs.Subject.Request;
using skolesystem.DTOs.Subject.Response;
using skolesystem.Repository.SubjectRepository;
using skolesystem.Models;


namespace skolesystem.Service.SubjectService
{
	public class SubjectService : ISubjectService
	{
        private readonly ISubjectRepository _SubjectRepository;

        public SubjectService(ISubjectRepository SubjectRepository)
        {
            _SubjectRepository = SubjectRepository;
        }

        public async Task<List<SubjectResponse>> GetAll()
        {
            // Kald repository-metode for at hente alle subjects,fra databasen
            List<Subjects> Subject = await _SubjectRepository.SelectAllSubject();
            return Subject.Select(c => new SubjectResponse
            {
                Id = c.subject_id,
                subjectname = c.subject_name,
            }).ToList();
        }


        public async Task<SubjectResponse> GetById(int SubjectId)
        {
            // Kald repository-metode for at hente et specifikt subjects,fra databasen
            Subjects Subject = await _SubjectRepository.SelectSubjectById(SubjectId);
            return Subject == null ? null : new SubjectResponse
            {
                Id = Subject.subject_id,
                subjectname = Subject.subject_name,
            };
        }
        public async Task<SubjectResponse> Create(NewSubject newSubject)
        {
            // Opret et nyt subject-objekt ved hjælp af værdier fra Newsubject
            Subjects subject = new Subjects
            {
                subject_name = newSubject.subjectname
            };
            // Indsæt den nye subject i repository 
            subject = await _SubjectRepository.InsertNewSubject(subject);

            return subject == null ? null : new SubjectResponse
            {
                Id = subject.subject_id,
                subjectname = subject.subject_name
            };


        }
        public async Task<SubjectResponse> Update(int SubjectId, UpdateSubject updateSubject)
        {
            // Opret et nyt subject-objekt med opdaterede oplysninger
            Subjects subject = new Subjects
            {
                subject_name = updateSubject.subjectname
            };
            // Opdater den eksisterende subject i repository'en asynkront
            subject = await _SubjectRepository.UpdateExistingSubject(SubjectId, subject);

            return subject == null ? null : new SubjectResponse
            {
                Id = subject.subject_id,
                subjectname = subject.subject_name
            };
        }
        public async Task<bool> Delete(int SubjectId)
        {
            // Anmod om sletning af subject fra repository
            var result = await _SubjectRepository.DeleteSubject(SubjectId);
            // Returner true, hvis sletningen var vellykket
            if (result != null) return true;
            else return false;
        }
    }
}

