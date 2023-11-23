using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using AutoMapper;
using FluentAssertions;
using global::skolesystem.DTOs;
using global::skolesystem.Models;
using global::skolesystem.Repository;
using global::skolesystem.Service;
using Moq;
using Xunit;

namespace skolesystem.Tests.Services
{
           public class AbsenceServiceTests
        {
            private readonly IMapper _mapper; // Add an instance of IMapper
            private readonly IAbsenceService _absenceService; // Add an instance of IAbsenceService
            private readonly Mock<IAbsenceRepository> _absenceRepositoryMock = new Mock<IAbsenceRepository>();

            // Constructor 
            public AbsenceServiceTests()
            {
                // Initialize IMapper 
                var configuration = new MapperConfiguration(cfg => cfg.AddProfile<MappingProfiles>());
                _mapper = new Mapper(configuration);

                // Provide _mapper to the AbsenceService constructor
                _absenceService = new AbsenceService(_absenceRepositoryMock.Object, _mapper);
            }
            [Fact]
            public async Task GetAbsenceById_ShouldReturnDto_WhenAbsenceExists()
            {
                // Arrange
                int absenceId = 1;
                var existingAbsence = new Absence
                {
                    absence_id = absenceId,
                    user_id = 1,
                    teacher_id = 1,
                    class_id = 1,
                    absence_date = DateTime.Now,
                    reason = "Reason 1",
                    is_deleted = false
                };

                var mapperMock = new Mock<IMapper>();
                mapperMock.Setup(mapper => mapper.Map<AbsenceReadDto>(existingAbsence)).Returns(new AbsenceReadDto
                {
                    absence_id = existingAbsence.absence_id,
                    user_id = existingAbsence.user_id,
                    teacher_id = existingAbsence.teacher_id,
                    class_id = existingAbsence.class_id,
                    absence_date = existingAbsence.absence_date,
                    reason = existingAbsence.reason,
                    is_deleted = existingAbsence.is_deleted
                });

                var repositoryMock = new Mock<IAbsenceRepository>();
                repositoryMock.Setup(repo => repo.GetById(absenceId)).ReturnsAsync(existingAbsence);

                var service = new AbsenceService(repositoryMock.Object, mapperMock.Object);

                // Act
                var result = await service.GetAbsenceById(absenceId);

                // Assert
                result.Should().NotBeNull();
                result.Should().BeOfType<AbsenceReadDto>();
                result.absence_id.Should().Be(existingAbsence.absence_id);
                result.user_id.Should().Be(existingAbsence.user_id);
                result.teacher_id.Should().Be(existingAbsence.teacher_id);
                result.class_id.Should().Be(existingAbsence.class_id);
                result.absence_date.Should().Be(existingAbsence.absence_date);
                result.reason.Should().Be(existingAbsence.reason);
                result.is_deleted.Should().Be(existingAbsence.is_deleted);

                // Additional assertions for mapper interactions (optional)
                mapperMock.Verify(mapper => mapper.Map<AbsenceReadDto>(existingAbsence), Times.Once);
            }

            [Fact]
            public async Task GetAbsenceById_ShouldReturnNull_WhenAbsenceDoesNotExist()
            {
                // Arrange
                int absenceId = 1;
                var mapperMock = new Mock<IMapper>();
                var repositoryMock = new Mock<IAbsenceRepository>();
                repositoryMock.Setup(repo => repo.GetById(absenceId)).ReturnsAsync((Absence)null);

                var service = new AbsenceService(repositoryMock.Object, mapperMock.Object);

                // Act
                var result = await service.GetAbsenceById(absenceId);

                // Assert
                result.Should().BeNull();

                // Additional assertions for mapper interactions (optional)
                mapperMock.Verify(mapper => mapper.Map<AbsenceReadDto>(It.IsAny<Absence>()), Times.Never);
            }

            [Fact]
            public async Task GetAllAbsences_ShouldReturnDtoList_WhenAbsencesExist()
            {
                // Arrange
                var absencesData = new List<Absence>
                    {
                        // Add your Absence data here
                        new Absence
                        {
                            absence_id = 1,
                            user_id = 1,
                            teacher_id = 1,
                            class_id = 1,
                            absence_date = DateTime.Now,
                            reason = "Reason 1",
                            is_deleted = false
                        },
                        
                    };

                _absenceRepositoryMock.Setup(repo => repo.GetAll()).ReturnsAsync(absencesData);

                // Act
                var result = await _absenceService.GetAllAbsences();

                // Assert
                result.Should().NotBeNull().And.BeAssignableTo<IEnumerable<AbsenceReadDto>>();
                var absenceDtos = result.Should().BeAssignableTo<IEnumerable<AbsenceReadDto>>().Subject;

                absenceDtos.Should().HaveCount(absencesData.Count);

                
                foreach (var (absenceDto, absence) in absenceDtos.Zip(absencesData))
                {
                    absenceDto.Should().BeEquivalentTo(absence);
                }
            }


            [Fact]
            public async Task GetAllAbsences_ShouldReturnEmptyList_WhenAbsencesDoNotExist()
            {
                // Arrange
                _absenceRepositoryMock.Setup(repo => repo.GetAll()).ReturnsAsync(new List<Absence>());

                // Act
                var result = await _absenceService.GetAllAbsences();

                // Assert
                result.Should().NotBeNull().And.BeAssignableTo<IEnumerable<AbsenceReadDto>>().And.BeEmpty();
            }

        [Fact]
        public async Task GetDeletedAbsences_ShouldReturnListOfDeletedAbsenceReadDto_WhenDeletedAbsencesExist()
        {
            // Arrange
            var deletedAbsences = new List<Absence>
            {
            new Absence { absence_id = 1, user_id = 1, teacher_id = 1, class_id = 1, absence_date = DateTime.Now, reason = "Reason 1", is_deleted = true },
            new Absence { absence_id = 2, user_id = 2, teacher_id = 2, class_id = 2, absence_date = DateTime.Now, reason = "Reason 2", is_deleted = true },
            };

            _absenceRepositoryMock.Setup(repo => repo.GetDeletedAbsences()).ReturnsAsync(deletedAbsences);

            // Act
            var result = await _absenceService.GetDeletedAbsences();

            // Assert
            result.Should().NotBeNull().And.BeAssignableTo<IEnumerable<AbsenceReadDto>>();
            var deletedAbsenceList = result.ToList();
            deletedAbsenceList.Should().HaveCount(2);
        }

        [Fact]
        public async Task GetDeletedAbsences_ShouldReturnEmptyList_WhenNoDeletedAbsencesExist()
        {
            // Arrange
            _absenceRepositoryMock.Setup(repo => repo.GetDeletedAbsences()).ReturnsAsync(new List<Absence>());

            // Act
            var result = await _absenceService.GetDeletedAbsences();

            // Assert
            result.Should().NotBeNull().And.BeAssignableTo<IEnumerable<AbsenceReadDto>>().And.BeEmpty();
        }

        [Fact]
        public async Task CreateAbsence_ShouldReturnAbsenceReadDto_WhenAbsenceCreatedSuccessfully()
        {
            // Arrange
            var absenceCreateDto = new AbsenceCreateDto
            {
                user_id = 1,
                teacher_id = 1,
                class_id = 1,
                absence_date = DateTime.Now,
                reason = "Reason 1"
            };

            var createdAbsenceId = 1;

            _absenceRepositoryMock.Setup(repo => repo.AddAbsence(It.IsAny<Absence>()))
                .Callback<Absence>(absence =>
                {
                    // Simulate setting the absence ID when adding to the repository
                    absence.absence_id = createdAbsenceId;
                });

            // Act
            var result = await _absenceService.CreateAbsence(absenceCreateDto);

            // Assert
            result.Should().NotBeNull().And.BeAssignableTo<AbsenceReadDto>();
            result.absence_id.Should().Be(createdAbsenceId);
        }
        [Fact]
        public async Task CreateAbsence_ShouldThrowException_WhenAbsenceCreationFails()
        {
            // Arrange
            var absenceCreateDto = new AbsenceCreateDto
            {
                user_id = 1,
                teacher_id = 1,
                class_id = 1,
                absence_date = DateTime.Now,
                reason = "Reason 1"
            };

            _absenceRepositoryMock.Setup(repo => repo.AddAbsence(It.IsAny<Absence>()))
                .ThrowsAsync(new Exception("Failed to create absence"));

            // Act and Assert
            await Assert.ThrowsAsync<Exception>(() => _absenceService.CreateAbsence(absenceCreateDto));
        }

        [Fact]
        public async Task UpdateAbsence_ShouldNotThrowException_WhenAbsenceExists()
        {
            // Arrange
            var absenceUpdateDto = new AbsenceUpdateDto
            {
                user_id = 1,
                teacher_id = 1,
                class_id = 1,
                absence_date = DateTime.Now,
                reason = "Updated Reason"
            };

            var existingAbsenceId = 1;

            _absenceRepositoryMock.Setup(repo => repo.GetById(existingAbsenceId))
                .ReturnsAsync(new Absence { absence_id = existingAbsenceId });

            // Act and Assert
            await _absenceService.Invoking(async x => await x.UpdateAbsence(existingAbsenceId, absenceUpdateDto))
                .Should().NotThrowAsync<Exception>();
        }


        [Fact]
        public async Task UpdateAbsence_ShouldThrowException_WhenAbsenceDoesNotExist()
        {
            // Arrange
            var absenceUpdateDto = new AbsenceUpdateDto
            {
                user_id = 1,
                teacher_id = 1,
                class_id = 1,
                absence_date = DateTime.Now,
                reason = "Updated Reason"
            };

            var nonExistingAbsenceId = 999; // Assuming this ID doesn't exist

            _absenceRepositoryMock.Setup(repo => repo.GetById(nonExistingAbsenceId))
                .ReturnsAsync((Absence)null);

            // Act and Assert
            await Assert.ThrowsAsync<ArgumentException>(() => _absenceService.UpdateAbsence(nonExistingAbsenceId, absenceUpdateDto));
        }

        [Fact]
        public async Task SoftDeleteAbsence_ShouldNotThrowException_WhenAbsenceExists()
        {
            // Arrange
            var existingAbsenceId = 1;

            _absenceRepositoryMock.Setup(repo => repo.GetById(existingAbsenceId))
                .ReturnsAsync(new Absence { absence_id = existingAbsenceId });

            // Act and Assert
            await _absenceService.Invoking(async x => await x.SoftDeleteAbsence(existingAbsenceId))
                .Should().NotThrowAsync<Exception>();
        }

        [Fact]
        public async Task SoftDeleteAbsence_ShouldThrowException_WhenAbsenceDoesNotExist()
        {
            // Arrange
            var nonExistingAbsenceId = 999; // Assuming this ID doesn't exist

            _absenceRepositoryMock.Setup(repo => repo.GetById(nonExistingAbsenceId))
                .ReturnsAsync((Absence)null);

            // Act and Assert
            await Assert.ThrowsAsync<ArgumentException>(() => _absenceService.SoftDeleteAbsence(nonExistingAbsenceId));
        }



    }

}
    
