using System;
using skolesystem.DTOs.Enrollment.Request;
using skolesystem.DTOs.Enrollment.Response;
using skolesystem.Models;
using skolesystem.Repository;
using skolesystem.Repository.AssignmentRepository;
using skolesystem.Repository.EnrollmentRepository;

namespace skolesystem.Service.EnrollmentService
{
	public class EnrollmentService : IEnrollmentService
	{
        private readonly IEnrollmentRepository _EnrollmentRepository;
        private readonly IAssignmentRepository _AssignmentRepository;
        private readonly IUsersRepository _UsersRepository;


        public EnrollmentService(IEnrollmentRepository EnrollmentRepository, IAssignmentRepository AssignmentRepository, IUsersRepository usersRepository)
        {
            _EnrollmentRepository = EnrollmentRepository;
            _AssignmentRepository = AssignmentRepository;
            _UsersRepository = usersRepository;
        }


        public async Task<List<EnrollmentResponse>> GetAllEnrollmentsByClass(int assignmentId)
        {
            // Kald repository-metode for at hente alle enrollments,fra databasen

            List<Enrollments> Enrollment = await _EnrollmentRepository.GetEnrollmentsByClass(assignmentId);
            return Enrollment.Select(a => new EnrollmentResponse
            {
                enrollment_Id = a.enrollment_id,
                enrollmentClassResponse = new EnrollmentClassResponse
                {
                    Id = a.Classe.class_id,
                    className = a.Classe.class_name,
                    location = a.Classe.location
                },
                enrollmentUserResponse = new EnrollmentUserResponse
                {
                    user_id = a.User.user_id,
                    surname = a.User.surname,
                    email = a.User.email
                }
            }).ToList();


        }

        public async Task<List<EnrollmentResponse>> GetAllEnrollmentsByUser(int assignmentId)
        {
            List<Enrollments> Enrollment = await _EnrollmentRepository.GetAllEnrollmentsByUser(assignmentId);
            return Enrollment.Select(a => new EnrollmentResponse
            {
                enrollment_Id = a.enrollment_id,
                enrollmentClassResponse = new EnrollmentClassResponse
                {
                    Id = a.Classe.class_id,
                    className = a.Classe.class_name,
                    location = a.Classe.location
                }
            }).ToList();


        }

        public async Task<EnrollmentResponse?> GetById(int EnrollmentId)
        {
            // Kald repository-metode for at hente et specifikt enrollments,fra databasen
            Enrollments Enrollment = await _EnrollmentRepository.SelectEnrollmentsById(EnrollmentId);
            return Enrollment == null ? null : new EnrollmentResponse
            {
                enrollment_Id = Enrollment.enrollment_id,
                enrollmentClassResponse = new EnrollmentClassResponse
                {
                    Id = Enrollment.Classe.class_id,
                    className = Enrollment.Classe.class_name,
                    location = Enrollment.Classe.location
                },
                enrollmentUserResponse = new EnrollmentUserResponse
                {
                    user_id = Enrollment.User.user_id,
                    surname = Enrollment.User.surname,
                    email = Enrollment.User.email
                }
            };
        }



        public async Task<EnrollmentResponse?> Create(NewEnrollment newEnrollment)
        {
            // Opret et nyt enrollment-objekt ved hjælp af værdier fra Newenrollment
            Enrollments Enrollment = new Enrollments
            {
                user_id = newEnrollment.UserId,
                class_id = newEnrollment.ClasseId
            };

            // Indsæt den nye enrollment i repository 
            Enrollment = await _EnrollmentRepository.InsertNewEnrollments(Enrollment);

            return Enrollment == null ? null : new EnrollmentResponse
            {
              
            };
        }



        public async Task<EnrollmentResponse?> Update(int EnrollmentId, UpdateEnrollment updateEnrollment)
        {
            // Opret et nyt enrollment-objekt med opdaterede oplysninger
            Enrollments Enrollment = new Enrollments
            {
                user_id = updateEnrollment.UserId,
                class_id = updateEnrollment.ClasseId
            };
            // Opdater den eksisterende enrollment i repository'en asynkront
            Enrollment = await _EnrollmentRepository.UpdateExistingEnrollments(EnrollmentId, Enrollment);
            if (Enrollment == null) return null;
            else
            {
                await _AssignmentRepository.SelectAssignmentById(Enrollment.enrollment_id);
                return Enrollment == null ? null : new EnrollmentResponse
                {

                };
            }
        }
        public async Task<bool> Delete(int EnrollmentId)
        {
            // Anmod om sletning af enrollment fra repository
            var result = await _EnrollmentRepository.DeleteEnrollments(EnrollmentId);
            // Returner true, hvis sletningen var vellykket
            return (result != null);
        }

        
    }
}

