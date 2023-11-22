using Microsoft.AspNetCore.Http;
using Microsoft.AspNetCore.Mvc;
using Moq;
using skolesystem.Controllers;
using skolesystem.DTOs;
using skolesystem.Models;

namespace skolesystem.Tests.Controller

{
    public class SkemaControllerTests
    {
        private readonly Mock<ISkemaRepository> _skemaRepositoryMock;
        private readonly SkemaController _skemaController;

        public SkemaControllerTests()
        {
            _skemaRepositoryMock = new Mock<ISkemaRepository>();
            _skemaController = new SkemaController(_skemaRepositoryMock.Object);
        }

        [Fact]
        public async Task Get_ShouldReturnOkResult_WhenSchemataExist()
        {
            // Arrange
            var existingSchemata = new List<Skema>
        {
            new Skema { schedule_id = 1, user_subject_id = 1, day_of_week = "Monday", start_time = 8, end_time = 10, class_id = 1 },
            new Skema { schedule_id = 2, user_subject_id = 2, day_of_week = "Tuesday", start_time = 9, end_time = 11, class_id = 2 }
        };

            _skemaRepositoryMock.Setup(repo => repo.GetAll()).ReturnsAsync(existingSchemata);

            // Act
            var result = await _skemaController.Get();

            // Assert
            var okResult = Assert.IsType<OkObjectResult>(result);
            var model = Assert.IsAssignableFrom<IEnumerable<Skema>>(okResult.Value);
            Assert.Equal(existingSchemata.Count, model.Count());
        }

        [Fact]
        public async Task Get_ShouldReturnNotFound_WhenNoSchemataExist()
        {
            // Arrange
            _skemaRepositoryMock.Setup(repo => repo.GetAll()).ReturnsAsync(new List<Skema>());

            // Act
            var result = await _skemaController.Get();

            // Assert
            Assert.IsType<OkObjectResult>(result);
        }

        [Fact]
        public async Task GetById_ShouldReturnOkResult_WhenSkemaExists()
        {
            // Arrange
            int skemaId = 1;
            var skema = new Skema { schedule_id = skemaId, user_subject_id = 1, day_of_week = "Monday", start_time = 9, end_time = 12, class_id = 1 };
            _skemaRepositoryMock.Setup(repo => repo.GetById(skemaId)).ReturnsAsync(skema);

            // Act
            var result = await _skemaController.GetById(skemaId);

            // Assert
            Assert.IsType<OkObjectResult>(result);
            var okResult = result as OkObjectResult;
            Assert.NotNull(okResult);
            Assert.Equal(StatusCodes.Status200OK, okResult.StatusCode);

            var skemaResult = okResult.Value as Skema;
            Assert.NotNull(skemaResult);
            Assert.Equal(skemaId, skemaResult.schedule_id);
        }

        [Fact]
        public async Task GetById_ShouldReturnNotFound_WhenSkemaDoesNotExist()
        {
            // Arrange
            int skemaId = 1;
            _skemaRepositoryMock.Setup(repo => repo.GetById(skemaId)).ReturnsAsync((Skema)null);

            // Act
            var result = await _skemaController.GetById(skemaId);

            // Assert
            Assert.IsType<NotFoundResult>(result);
        }
        [Fact]
        public async Task Update_ShouldReturnNoContent_WhenSkemaExists()
        {
            // Arrange
            int skemaId = 1;
            var skemaDto = new SkemaCreateDto { user_subject_id = 2, day_of_week = "Tuesday", start_time = 10, end_time = 13, class_id = 2 };

            _skemaRepositoryMock.Setup(repo => repo.Update(skemaId, skemaDto));

            // Act
            var result = await _skemaController.Update(skemaId, skemaDto);

            // Assert
            Assert.IsType<NoContentResult>(result);
        }

        [Fact]
        public async Task Update_ShouldReturnNotFound_WhenSkemaDoesNotExist()
        {
            // Arrange
            int skemaId = 1;
            var skemaDto = new SkemaCreateDto { user_subject_id = 2, day_of_week = "Tuesday", start_time = 10, end_time = 13, class_id = 2 };

            _skemaRepositoryMock.Setup(repo => repo.Update(skemaId, skemaDto)).Throws(new ArgumentException("Skema not found"));

            // Act
            var result = await _skemaController.Update(skemaId, skemaDto);

            // Assert
            Assert.IsType<NotFoundObjectResult>(result);
            var notFoundResult = result as NotFoundObjectResult;
            Assert.NotNull(notFoundResult);
            Assert.Equal("Skema not found", notFoundResult.Value);
        }
        [Fact]
        public async Task Delete_ShouldReturnNoContent_WhenSkemaExists()
        {
            // Arrange
            int skemaId = 1;

            _skemaRepositoryMock.Setup(repo => repo.Delete(skemaId));

            // Act
            var result = await _skemaController.Delete(skemaId);

            // Assert
            Assert.IsType<NoContentResult>(result);
        }

        [Fact]
        public async Task Delete_ShouldReturnNotFound_WhenSkemaDoesNotExist()
        {
            // Arrange
            int skemaId = 1;

            _skemaRepositoryMock.Setup(repo => repo.Delete(skemaId)).Throws(new ArgumentException("Skema not found"));

            // Act
            var result = await _skemaController.Delete(skemaId);

            // Assert
            Assert.IsType<NotFoundObjectResult>(result);
            var notFoundResult = result as NotFoundObjectResult;
            Assert.NotNull(notFoundResult);
            Assert.Equal("Skema not found", notFoundResult.Value);
        }




    }

}