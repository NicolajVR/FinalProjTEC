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
            Enrollments Enrollment = new Enrollments
            {
                user_id = newEnrollment.UserId,
                class_id = newEnrollment.ClasseId
            };

            Enrollment = await _EnrollmentRepository.InsertNewEnrollments(Enrollment);
            //await _UsersRepository.GetEnrollmentByAssignment(user.CategoryId);

            return Enrollment == null ? null : new EnrollmentResponse
            {
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



        public async Task<EnrollmentResponse?> Update(int EnrollmentId, UpdateEnrollment updateEnrollment)
        {
            Enrollments Enrollment = new Enrollments
            {
                user_id = updateEnrollment.UserId,
                class_id = updateEnrollment.ClasseId
            };

            Enrollment = await _EnrollmentRepository.UpdateExistingEnrollments(EnrollmentId, Enrollment);
            if (Enrollment == null) return null;
            else
            {
                await _AssignmentRepository.SelectAssignmentById(Enrollment.enrollment_id);
                return Enrollment == null ? null : new EnrollmentResponse
                {

                    enrollmentClassResponse = new EnrollmentClassResponse
                    {
                        Id = Enrollment.Classe.class_id,
                    },
                    enrollmentUserResponse = new EnrollmentUserResponse
                    {
                        user_id = Enrollment.User.user_id,
                    }
                };
            }
        }
        public async Task<bool> Delete(int EnrollmentId)
        {
            var result = await _EnrollmentRepository.DeleteEnrollments(EnrollmentId);
            return (result != null);
        }

        
    }
}

