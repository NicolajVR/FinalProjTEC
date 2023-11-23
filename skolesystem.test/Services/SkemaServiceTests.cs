using FluentAssertions;
using Moq;
using skolesystem.DTOs;
using skolesystem.Models;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace skolesystem.Tests.Services
{
    public class ScheduleServiceTests
    {
        [Fact]
        public async Task GetAllSchemata_ShouldReturnAllSchemata_WhenSchemataExist()
        {
            // Arrange
            var schemataData = new List<Schedule>
        {
            new Schedule { schedule_id = 1, /* other properties */ },
            new Schedule { schedule_id = 2, /* other properties */ },
            // Add more Schedule objects as needed
        };

            var scheduleRepositoryMock = new Mock<IScheduleRepository>();
            scheduleRepositoryMock.Setup(repo => repo.GetAll()).ReturnsAsync(schemataData);

            var scheduleService = new ScheduleService(scheduleRepositoryMock.Object);

            // Act
            var result = await scheduleService.GetAllSchemata();

            // Assert
            Assert.NotNull(result);
            Assert.IsAssignableFrom<IEnumerable<Schedule>>(result);
            Assert.Equal(schemataData.Count, result.Count());
            Assert.Equal(schemataData, result);
        }

        [Fact]
        public async Task GetAllSchemata_ShouldReturnEmptyList_WhenNoSchemataExist()
        {
            // Arrange
            var scheduleRepositoryMock = new Mock<IScheduleRepository>();
            scheduleRepositoryMock.Setup(repo => repo.GetAll()).ReturnsAsync(new List<Schedule>());

            var scheduleService = new ScheduleService(scheduleRepositoryMock.Object);

            // Act
            var result = await scheduleService.GetAllSchemata();

            // Assert
            Assert.NotNull(result);
            Assert.IsAssignableFrom<IEnumerable<Schedule>>(result);
            Assert.Empty(result);
        }

        [Fact]
        public async Task GetScheduleById_ShouldReturnSchedule_WhenScheduleExists()
        {
            // Arrange
            int scheduleId = 1;
            var existingSchedule = new Schedule { schedule_id = scheduleId, /* other properties */ };

            var scheduleRepositoryMock = new Mock<IScheduleRepository>();
            scheduleRepositoryMock.Setup(repo => repo.GetById(scheduleId)).ReturnsAsync(existingSchedule);

            var scheduleService = new ScheduleService(scheduleRepositoryMock.Object);

            // Act
            var result = await scheduleService.GetScheduleById(scheduleId);

            // Assert
            Assert.NotNull(result);
            Assert.IsType<Schedule>(result);
            Assert.Equal(scheduleId, result.schedule_id);
            // Additional assertions based on your Schedule model properties
        }

        [Fact]
        public async Task GetScheduleById_ShouldReturnNull_WhenScheduleDoesNotExist()
        {
            // Arrange
            int nonExistingScheduleId = 999; // Assuming this ID doesn't exist

            var scheduleRepositoryMock = new Mock<IScheduleRepository>();
            scheduleRepositoryMock.Setup(repo => repo.GetById(nonExistingScheduleId)).ReturnsAsync((Schedule)null);

            var scheduleService = new ScheduleService(scheduleRepositoryMock.Object);

            // Act
            var result = await scheduleService.GetScheduleById(nonExistingScheduleId);

            // Assert
            Assert.Null(result);
        }
        [Fact]
        public async Task CreateSchedule_ShouldReturnScheduleId_WhenScheduleCreatedSuccessfully()
        {
            // Arrange
            var newSchedule = new Schedule { /* initialize Schedule properties */ };
            int createdScheduleId = 1;

            var scheduleRepositoryMock = new Mock<IScheduleRepository>();
            scheduleRepositoryMock.Setup(repo => repo.Create(newSchedule)).ReturnsAsync(createdScheduleId);

            var scheduleService = new ScheduleService(scheduleRepositoryMock.Object);

            // Act
            var result = await scheduleService.CreateSchedule(newSchedule);

            // Assert
            Assert.Equal(createdScheduleId, result);
        }
        [Fact]
        public async Task CreateSchedule_ShouldThrowException_WhenScheduleCreationFails()
        {
            // Arrange
            var newSchedule = new Schedule {};

            var scheduleRepositoryMock = new Mock<IScheduleRepository>();
            scheduleRepositoryMock.Setup(repo => repo.Create(newSchedule)).ThrowsAsync(new Exception("Failed to create Schedule"));

            var scheduleService = new ScheduleService(scheduleRepositoryMock.Object);

            // Act and Assert
            await Assert.ThrowsAsync<Exception>(() => scheduleService.CreateSchedule(newSchedule));
        }

        [Fact]
        public async Task UpdateSchedule_ShouldNotThrowException_WhenScheduleExists()
        {
            // Arrange
            int existingScheduleId = 1;
            var scheduleUpdateDto = new ScheduleCreateDto { /* initialize ScheduleCreateDto properties */ };

            var scheduleRepositoryMock = new Mock<IScheduleRepository>();
            scheduleRepositoryMock.Setup(repo => repo.GetById(existingScheduleId)).ReturnsAsync(new Schedule { schedule_id = existingScheduleId });

            var scheduleService = new ScheduleService(scheduleRepositoryMock.Object);

            // Act and Assert
            await scheduleService.Invoking(async x => await x.UpdateSchedule(existingScheduleId, scheduleUpdateDto))
                .Should().NotThrowAsync<Exception>();
        }

        [Fact]
        public async Task UpdateSchedule_ShouldThrowException_WhenScheduleDoesNotExist()
        {
            // Arrange
            int nonExistingScheduleId = 999; // Assuming this ID doesn't exist
            var scheduleUpdateDto = new ScheduleCreateDto { /* initialize ScheduleCreateDto properties */ };

            var scheduleRepositoryMock = new Mock<IScheduleRepository>();
            scheduleRepositoryMock.Setup(repo => repo.GetById(nonExistingScheduleId)).ReturnsAsync((Schedule)null);

            var scheduleService = new ScheduleService(scheduleRepositoryMock.Object);

            // Act and Assert
            await Assert.ThrowsAsync<ArgumentException>(() => scheduleService.UpdateSchedule(nonExistingScheduleId, scheduleUpdateDto));
        }

        [Fact]
        public async Task DeleteSchedule_ShouldNotThrowException_WhenScheduleExists()
        {
            // Arrange
            int existingScheduleId = 1;

            var scheduleRepositoryMock = new Mock<IScheduleRepository>();
            scheduleRepositoryMock.Setup(repo => repo.GetById(existingScheduleId)).ReturnsAsync(new Schedule { /* initialize Schedule properties */ });

            var scheduleService = new ScheduleService(scheduleRepositoryMock.Object);

            // Act and Assert
            await scheduleService.Invoking(async x => await x.DeleteSchedule(existingScheduleId))
                .Should().NotThrowAsync<Exception>();
        }

        [Fact]
        public async Task DeleteSchedule_ShouldDeleteSchedule_WhenScheduleExists()
        {
            // Arrange
            int existingScheduleId = 1;

            var scheduleRepositoryMock = new Mock<IScheduleRepository>();
            scheduleRepositoryMock.Setup(repo => repo.GetById(existingScheduleId)).ReturnsAsync(new Schedule { schedule_id = existingScheduleId });

            var scheduleService = new ScheduleService(scheduleRepositoryMock.Object);

            // Act
            await scheduleService.DeleteSchedule(existingScheduleId);

            // Assert
            scheduleRepositoryMock.Verify(repo => repo.Delete(existingScheduleId), Times.Once);
        }






    }
}
