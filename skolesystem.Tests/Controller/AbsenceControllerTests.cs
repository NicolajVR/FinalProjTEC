using AutoMapper;
using FluentAssertions;
using Moq;
using skolesystem.DTOs;
using skolesystem.Models;
using skolesystem.Repository;
using skolesystem.Service;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace skolesystem.Tests.Controller
{
    // Assuming you're using xUnit and FluentAssertions for assertions

    public class AbsenceServiceTests
    {
        private readonly Mock<IAbsenceRepository> _absenceRepositoryMock;
        private readonly IAbsenceService _absenceService;

        public AbsenceServiceTests()
        {
            _absenceRepositoryMock = new Mock<IAbsenceRepository>();

            // Set up AutoMapper configuration
            var config = new MapperConfiguration(cfg => cfg.AddProfile<MappingProfiles>());
            var mapper = new Mapper(config);

            _absenceService = new AbsenceService(_absenceRepositoryMock.Object, mapper);
        }

        [Fact]
        public async Task GetAbsences_ShouldReturnListOfAbsenceReadDto()
        {
            // Arrange
            var absences = new List<Absence>
    {
        new Absence { absence_id = 1, user_id = 1, teacher_id = 1, class_id = 1, absence_date = DateTime.Now, reason = "Reason 1", is_deleted = false },
        new Absence { absence_id = 2, user_id = 2, teacher_id = 2, class_id = 2, absence_date = DateTime.Now, reason = "Reason 2", is_deleted = false },
    };

            _absenceRepositoryMock.Setup(repo => repo.GetAll()).ReturnsAsync(absences);

            // Act
            var result = await _absenceService.GetAllAbsences();

            // Assert
            result.Should().NotBeNull().And.BeAssignableTo<IEnumerable<AbsenceReadDto>>();
            var absenceList = result.ToList();
            absenceList.Should().HaveCount(2);
            absenceList.Should().OnlyContain(a => a is AbsenceReadDto);
        }

        [Fact]
        public async Task GetAbsences_ShouldReturnEmptyList_WhenNoAbsencesExist()
        {
            // Arrange
            _absenceRepositoryMock.Setup(repo => repo.GetAll()).ReturnsAsync(new List<Absence>());

            // Act
            var result = await _absenceService.GetAllAbsences();

            // Assert
            result.Should().NotBeNull().And.BeAssignableTo<IEnumerable<AbsenceReadDto>>().And.BeEmpty();
        }


        [Fact]
        public async Task GetAbsenceById_ShouldReturnAbsenceReadDto_WhenAbsenceExists()
        {
            // Arrange
            int absenceId = 1;
            var existingAbsence = new Absence { absence_id = absenceId, user_id = 1, teacher_id = 1, class_id = 1, absence_date = DateTime.Now, reason = "Reason 1", is_deleted = false };

            _absenceRepositoryMock.Setup(repo => repo.GetById(absenceId)).ReturnsAsync(existingAbsence);

            // Act
            var result = await _absenceService.GetAbsenceById(absenceId);

            // Assert
            result.Should().NotBeNull().And.BeOfType<AbsenceReadDto>();
            var absenceDto = (AbsenceReadDto)result; 
            absenceDto.Should().NotBeNull();
            absenceDto.absence_id.Should().Be(existingAbsence.absence_id);
            
        }


        [Fact]
        public async Task GetAbsenceById_ShouldReturnNull_WhenAbsenceDoesNotExist()
        {
            // Arrange
            int absenceId = 1;
            _absenceRepositoryMock.Setup(repo => repo.GetById(absenceId)).ReturnsAsync((Absence)null);

            // Act
            var result = await _absenceService.GetAbsenceById(absenceId);

            // Assert
            result.Should().BeNull();
        }


        [Fact]
        public async Task CreateAbsence_ShouldReturnAbsenceReadDto_WhenCreateAbsenceExists()
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

            var expectedAbsenceId = 1;

            _absenceRepositoryMock.Setup(repo => repo.AddAbsence(It.IsAny<Absence>()))
                .Callback<Absence>(absence =>
                {
                    // Simulate setting the absence ID when adding to the repository
                    absence.absence_id = expectedAbsenceId;
                });

            // Act
            var result = await _absenceService.CreateAbsence(absenceCreateDto);

            // Assert
            result.Should().NotBeNull().And.BeAssignableTo<AbsenceReadDto>();
            var resultDto = (AbsenceReadDto)result;
            resultDto.absence_id.Should().Be(expectedAbsenceId);
            resultDto.user_id.Should().Be(absenceCreateDto.user_id);
            
        }



        [Fact]
        public async Task CreateAbsence_ShouldThrowException_WhenNoCreateAbsenceExist()
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
        public async Task UpdateAbsence_ShouldReturnNoContent_WhenUpdateAbsenceExists()
        {
            // Arrange
            int absenceId = 1;
            var existingAbsence = new Absence { absence_id = absenceId, user_id = 1, teacher_id = 1, class_id = 1, absence_date = DateTime.Now, reason = "Reason 1", is_deleted = false };
            var updatedAbsenceDto = new AbsenceUpdateDto { user_id = 2, teacher_id = 2, class_id = 2, absence_date = DateTime.Now.AddDays(1), reason = "Updated Reason" };

            _absenceRepositoryMock.Setup(repo => repo.GetById(absenceId)).ReturnsAsync(existingAbsence);

            // Act
            Func<Task> act = async () => await _absenceService.UpdateAbsence(absenceId, updatedAbsenceDto);

            // Assert
            await act.Should().NotThrowAsync<Exception>();
            _absenceRepositoryMock.Verify(repo => repo.UpdateAbsence(absenceId, It.IsAny<Absence>()), Times.Once);
        }

        [Fact]
        public async Task UpdateAbsence_ShouldThrowException_WhenNoUpdateAbsenceExist()
        {
            // Arrange
            int absenceId = 1;
            var updatedAbsenceDto = new AbsenceUpdateDto { user_id = 2, teacher_id = 2, class_id = 2, absence_date = DateTime.Now.AddDays(1), reason = "Updated Reason" };

            _absenceRepositoryMock.Setup(repo => repo.GetById(absenceId)).ReturnsAsync((Absence)null);

            // Act and Assert
            await Assert.ThrowsAsync<ArgumentException>(() => _absenceService.UpdateAbsence(absenceId, updatedAbsenceDto));
            _absenceRepositoryMock.Verify(repo => repo.UpdateAbsence(absenceId, It.IsAny<Absence>()), Times.Never);
        }

        [Fact]
        public async Task SoftDeleteAbsence_ShouldReturnNoContent_WhenDeleteAbsenceExists()
        {
            // Arrange
            int absenceId = 1;
            var existingAbsence = new Absence { absence_id = absenceId, user_id = 1, teacher_id = 1, class_id = 1, absence_date = DateTime.Now, reason = "Reason 1", is_deleted = false };

            _absenceRepositoryMock.Setup(repo => repo.GetById(absenceId)).ReturnsAsync(existingAbsence);

            // Act
            Func<Task> act = async () => await _absenceService.SoftDeleteAbsence(absenceId);

            // Assert
            await act.Should().NotThrowAsync<Exception>();
            _absenceRepositoryMock.Verify(repo => repo.SoftDeleteAbsence(absenceId), Times.Once);
        }

        [Fact]
        public async Task SoftDeleteAbsence_ShouldThrowException_WhenNoDeleteAbsenceExist()
        {
            // Arrange
            int absenceId = 1;

            _absenceRepositoryMock.Setup(repo => repo.GetById(absenceId)).ReturnsAsync((Absence)null);

            // Act and Assert
            await Assert.ThrowsAsync<ArgumentException>(() => _absenceService.SoftDeleteAbsence(absenceId));
            _absenceRepositoryMock.Verify(repo => repo.SoftDeleteAbsence(absenceId), Times.Never);
        }

    }

}
