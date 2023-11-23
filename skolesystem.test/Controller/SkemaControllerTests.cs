using Microsoft.AspNetCore.Http;
using Microsoft.AspNetCore.Mvc;
using Moq;
using skolesystem.Controllers;
using skolesystem.DTOs;
using skolesystem.Models;

namespace skolesystem.Tests.Controller

{
    public class ScheduleControllerTests
    {
        private readonly Mock<IScheduleRepository> _scheduleRepositoryMock;
        private readonly ScheduleController _scheduleController;

        public ScheduleControllerTests()
        {
            _scheduleRepositoryMock = new Mock<IScheduleRepository>();
            _scheduleController = new ScheduleController(_scheduleRepositoryMock.Object);
        }

        [Fact]
        public async Task Get_ShouldReturnOkResult_WhenSchemataExist()
        {
            // Arrange
            var existingSchemata = new List<Schedule>
        {
            new Schedule { schedule_id = 1, subject_id = 1, day_of_week = "Monday", start_time = "10/12/2013", end_time = "10/12/2013", class_id = 1 },
            new Schedule { schedule_id = 2, subject_id = 2, day_of_week = "Tuesday", start_time = "10/12/2013", end_time = "10/12/2013", class_id = 2 }
        };

            _scheduleRepositoryMock.Setup(repo => repo.GetAll()).ReturnsAsync(existingSchemata);

            // Act
            var result = await _scheduleController.Get();

            // Assert
            var okResult = Assert.IsType<OkObjectResult>(result);
            var model = Assert.IsAssignableFrom<IEnumerable<Schedule>>(okResult.Value);
            Assert.Equal(existingSchemata.Count, model.Count());
        }

        [Fact]
        public async Task Get_ShouldReturnNotFound_WhenNoSchemataExist()
        {
            // Arrange
            _scheduleRepositoryMock.Setup(repo => repo.GetAll()).ReturnsAsync(new List<Schedule>());

            // Act
            var result = await _scheduleController.Get();

            // Assert
            Assert.IsType<OkObjectResult>(result);
        }

        [Fact]
        public async Task GetById_ShouldReturnOkResult_WhenScheduleExists()
        {
            // Arrange
            int scheduleId = 1;
            var schedule = new Schedule { schedule_id = scheduleId, subject_id = 1, day_of_week = "Monday", start_time = "10/12/2013", end_time = "10/12/2013", class_id = 1 };
            _scheduleRepositoryMock.Setup(repo => repo.GetById(scheduleId)).ReturnsAsync(schedule);

            // Act
            var result = await _scheduleController.GetById(scheduleId);

            // Assert
            Assert.IsType<OkObjectResult>(result);
            var okResult = result as OkObjectResult;
            Assert.NotNull(okResult);
            Assert.Equal(StatusCodes.Status200OK, okResult.StatusCode);

            var scheduleResult = okResult.Value as Schedule;
            Assert.NotNull(scheduleResult);
            Assert.Equal(scheduleId, scheduleResult.schedule_id);
        }

        [Fact]
        public async Task GetById_ShouldReturnNotFound_WhenScheduleDoesNotExist()
        {
            // Arrange
            int scheduleId = 1;
            _scheduleRepositoryMock.Setup(repo => repo.GetById(scheduleId)).ReturnsAsync((Schedule)null);

            // Act
            var result = await _scheduleController.GetById(scheduleId);

            // Assert
            Assert.IsType<NotFoundResult>(result);
        }
        [Fact]
        public async Task Update_ShouldReturnNoContent_WhenScheduleExists()
        {
            // Arrange
            int scheduleId = 1;
            var scheduleDto = new ScheduleCreateDto { subject_id = 2, day_of_week = "Tuesday", start_time = "10/12/2013", end_time = "10/12/2013", class_id = 2 };

            _scheduleRepositoryMock.Setup(repo => repo.Update(scheduleId, scheduleDto));

            // Act
            var result = await _scheduleController.Update(scheduleId, scheduleDto);

            // Assert
            Assert.IsType<NoContentResult>(result);
        }

        [Fact]
        public async Task Update_ShouldReturnNotFound_WhenScheduleDoesNotExist()
        {
            // Arrange
            int scheduleId = 1;
            var scheduleDto = new ScheduleCreateDto { subject_id = 2, day_of_week = "Tuesday", start_time = "10/12/2013", end_time = "10/12/2013", class_id = 2 };

            _scheduleRepositoryMock.Setup(repo => repo.Update(scheduleId, scheduleDto)).Throws(new ArgumentException("Schedule not found"));

            // Act
            var result = await _scheduleController.Update(scheduleId, scheduleDto);

            // Assert
            Assert.IsType<NotFoundObjectResult>(result);
            var notFoundResult = result as NotFoundObjectResult;
            Assert.NotNull(notFoundResult);
            Assert.Equal("Schedule not found", notFoundResult.Value);
        }
        [Fact]
        public async Task Delete_ShouldReturnNoContent_WhenScheduleExists()
        {
            // Arrange
            int scheduleId = 1;

            _scheduleRepositoryMock.Setup(repo => repo.Delete(scheduleId));

            // Act
            var result = await _scheduleController.Delete(scheduleId);

            // Assert
            Assert.IsType<NoContentResult>(result);
        }

        [Fact]
        public async Task Delete_ShouldReturnNotFound_WhenScheduleDoesNotExist()
        {
            // Arrange
            int scheduleId = 1;

            _scheduleRepositoryMock.Setup(repo => repo.Delete(scheduleId)).Throws(new ArgumentException("Schedule not found"));

            // Act
            var result = await _scheduleController.Delete(scheduleId);

            // Assert
            Assert.IsType<NotFoundObjectResult>(result);
            var notFoundResult = result as NotFoundObjectResult;
            Assert.NotNull(notFoundResult);
            Assert.Equal("Schedule not found", notFoundResult.Value);
        }




    }

}