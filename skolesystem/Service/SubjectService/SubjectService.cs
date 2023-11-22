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
            List<Subjects> Subject = await _SubjectRepository.SelectAllSubject();
            return Subject.Select(c => new SubjectResponse
            {
                Id = c.subject_id,
                subjectname = c.subject_name,
            }).ToList();
        }


        public async Task<SubjectResponse> GetById(int SubjectId)
        {
            Subjects Subject = await _SubjectRepository.SelectSubjectById(SubjectId);
            return Subject == null ? null : new SubjectResponse
            {
                Id = Subject.subject_id,
                subjectname = Subject.subject_name,
            };
        }
        public async Task<SubjectResponse> Create(NewSubject newSubject)
        {
            Subjects subject = new Subjects
            {
                subject_name = newSubject.subjectname
            };

            subject = await _SubjectRepository.InsertNewSubject(subject);

            return subject == null ? null : new SubjectResponse
            {
                Id = subject.subject_id,
                subjectname = subject.subject_name
            };


        }
        public async Task<SubjectResponse> Update(int SubjectId, UpdateSubject updateSubject)
        {
            Subjects subject = new Subjects
            {
                subject_name = updateSubject.subjectname
            };

            subject = await _SubjectRepository.UpdateExistingSubject(SubjectId, subject);

            return subject == null ? null : new SubjectResponse
            {
                Id = subject.subject_id,
                subjectname = subject.subject_name
            };
        }
        public async Task<bool> Delete(int SubjectId)
        {
            var result = await _SubjectRepository.DeleteSubject(SubjectId);
            if (result != null) return true;
            else return false;
        }
    }
}

