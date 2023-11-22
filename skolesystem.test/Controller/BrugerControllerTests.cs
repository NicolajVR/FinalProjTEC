using FluentAssertions;
using Microsoft.AspNetCore.Mvc;
using Moq;
using skolesystem.Controllers;
using skolesystem.DTOs;
using skolesystem.Models;
using skolesystem.Repository;
using skolesystem.Service;
using System.Collections.Generic;
using System.Threading.Tasks;
using Xunit;

namespace skolesystem.Tests.Controller
{
    public class BrugerControllerTests
    {
        private readonly Mock<IBrugerService> _brugerServiceMock;
        private readonly BrugerController _brugerController;
        private readonly IBrugerService _brugerService;


        public BrugerControllerTests()
        {
            _brugerServiceMock = new Mock<IBrugerService>();
            _brugerController = new BrugerController(_brugerServiceMock.Object);
        }

        [Fact]
        public async Task GetBrugers_ShouldReturnBrugerReadDtos_WhenBrugersExist()
        {
            // Arrange
            var brugers = new List<Bruger>
            {
                new Bruger { user_information_id = 1, name = "John", last_name = "Doe", phone = "123456", date_of_birth = "1995", address = "123 Main St", is_deleted = false, gender_id = 1, city_id = 1, user_id = 1 },
                new Bruger { user_information_id = 2, name = "Jane", last_name = "Doe", phone = "654321", date_of_birth = "1995", address = "456 Oak St", is_deleted = false, gender_id = 2, city_id = 2, user_id = 2 }
            };

            // Create a mock for IBrugerService
            var brugerServiceMock = new Mock<IBrugerService>();
            brugerServiceMock.Setup(repo => repo.GetAllBrugers()).ReturnsAsync(brugers.Select(b => new BrugerReadDto {}));

            // Inject the mock into the controller
            var brugerController = new BrugerController(brugerServiceMock.Object);

            // Act
            var result = await brugerController.GetBrugers();

            // Assert
            result.Should().NotBeNull().And.BeAssignableTo<IEnumerable<BrugerReadDto>>();
            var brugerDtos = (IEnumerable<BrugerReadDto>)result;
            brugerDtos.Should().NotBeNullOrEmpty().And.HaveCount(2);
        }



        [Fact]
        public async Task GetBrugers_ShouldReturnEmptyList_WhenNoBrugersExist()
        {
            // Arrange
            _brugerServiceMock.Setup(repo => repo.GetAllBrugers()).ReturnsAsync(new List<BrugerReadDto>());

            // Act
            var result = await _brugerController.GetBrugers();

            // Assert
            result.Should().NotBeNull().And.BeAssignableTo<IEnumerable<BrugerReadDto>>();
            var brugerDtos = (IEnumerable<BrugerReadDto>)result;
            brugerDtos.Should().BeEmpty();
        }


        [Fact]
        public async Task GetBrugerById_ShouldReturnBrugerReadDto_WhenBrugerExists()
        {
            // Arrange
            int brugerId = 1;
            var existingBruger = new BrugerReadDto { user_information_id = brugerId, name = "John", last_name = "Doe", phone = "123456", date_of_birth = "1998-01-01", address = "123 Main St", is_deleted = false, gender_id = 1, city_id = 1, user_id = 1 };

            _brugerServiceMock.Setup(repo => repo.GetBrugerById(brugerId)).ReturnsAsync(existingBruger);

            // Act
            var result = await _brugerController.GetBrugerById(brugerId);

            // Assert
            result.Should().NotBeNull().And.BeAssignableTo<OkObjectResult>();
            var okObjectResult = (OkObjectResult)result;
            okObjectResult.Value.Should().NotBeNull().And.BeAssignableTo<BrugerReadDto>();
            var brugerDto = (BrugerReadDto)okObjectResult.Value;
            brugerDto.user_information_id.Should().Be(existingBruger.user_information_id);
        }

        [Fact]
        public async Task GetBrugerById_ShouldReturnNotFound_WhenBrugerDoesNotExist()
        {
            // Arrange
            int brugerId = 1;

            _brugerServiceMock.Setup(repo => repo.GetBrugerById(brugerId)).ReturnsAsync((BrugerReadDto)null);

            // Act
            var result = await _brugerController.GetBrugerById(brugerId);

            // Assert
            result.Should().NotBeNull().And.BeAssignableTo<NotFoundResult>();
        }

        [Fact]
        public async Task CreateBruger_ShouldReturnCreatedAtAction_WhenBrugerIsCreated()
        {
            // Arrange
            var brugerDto = new BrugerCreateDto
            {
                name = "John",
                last_name = "Doe",
                phone = "123456",
                date_of_birth = "1998-01-01",
                address = "123 Main St",
                is_deleted = false,
                gender_id = 1,
                city_id = 1,
                user_id = 1
            };

            var expectedBrugerId = 1;

            _brugerServiceMock.Setup(repo => repo.AddBruger(It.IsAny<BrugerCreateDto>()))
                .Callback<BrugerCreateDto>(input =>
                {
                    // Simulate setting the user_information_id when adding to the repository
                    // This assumes your AddBruger method returns the ID
                    input.user_information_id = expectedBrugerId;
                })
                .ReturnsAsync((BrugerCreateDto input) =>
                {
                    // Create a BrugerReadDto based on the input or your logic
                    var createdBrugerDto = new BrugerReadDto
                    {
                        user_information_id = expectedBrugerId,
                        name = input.name,
                        last_name = input.last_name,
                    };

                    return createdBrugerDto;
                });

            // Act
            var result = await _brugerController.CreateBruger(brugerDto);

            // Assert
            result.Should().NotBeNull().And.BeAssignableTo<CreatedAtActionResult>();
            var createdAtActionResult = (CreatedAtActionResult)result;
            createdAtActionResult.Value.Should().BeAssignableTo<BrugerReadDto>();
            var resultDto = (BrugerReadDto)createdAtActionResult.Value;
            resultDto.user_information_id.Should().Be(expectedBrugerId);
            resultDto.name.Should().Be(brugerDto.name);
            resultDto.last_name.Should().Be(brugerDto.last_name);
        }





        [Fact]
        public async Task CreateBruger_ShouldReturnConflict_WhenBrugerAlreadyExists()
        {
            // Arrange
            var brugerDto = new BrugerCreateDto
            {
                name = "John",
                last_name = "Doe",
                phone = "123456",
                date_of_birth = "1998-01-01",
                address = "123 Main St",
                is_deleted = false,
                gender_id = 1,
                city_id = 1,
                user_id = 1
            };

            _brugerServiceMock.Setup(repo => repo.AddBruger(It.IsAny<BrugerCreateDto>())).ThrowsAsync(new ArgumentException("User with the specified ID already exists"));

            // Act
            var result = await _brugerController.CreateBruger(brugerDto);

            // Assert
            result.Should().BeAssignableTo<ConflictObjectResult>().And.Match<ConflictObjectResult>(r => r.Value.Equals("User with the specified ID already exists"));
        }

        [Fact]
        public async Task UpdateBruger_ShouldReturnNoContent_WhenBrugerExists()
        {
            // Arrange
            int brugerId = 1;
            var updatedBrugerDto = new BrugerUpdateDto
            {
                name = "Updated Name",
                last_name = "Updated Last Name",
                phone = "987654",
                date_of_birth = "1990-02-15",
                address = "456 Oak St",
                is_deleted = true
            };

            _brugerServiceMock.Setup(repo => repo.GetBrugerById(brugerId)).ReturnsAsync(new BrugerReadDto { user_information_id = brugerId });

            // Act
            var result = await _brugerController.UpdateBruger(brugerId, updatedBrugerDto);

            // Assert
            result.Should().BeOfType<NoContentResult>();
        }





        [Fact]
        public async Task DeleteBruger_ShouldReturnNoContent_WhenBrugerExists()
        {
            // Arrange
            int brugerId = 1;
            var existingBruger = new BrugerReadDto
            {
                user_information_id = brugerId,
                name = "John",
                last_name = "Doe",
                phone = "123456",
                date_of_birth = "1998-01-01",
                address = "123 Main St",
                is_deleted = false,
                gender_id = 1,
                city_id = 1,
                user_id = 1
            };

            _brugerServiceMock.Setup(repo => repo.GetBrugerById(brugerId)).ReturnsAsync(existingBruger);

            // Act
            var result = await _brugerController.DeleteBruger(brugerId);

            // Assert
            result.Should().BeOfType<NoContentResult>();
            _brugerServiceMock.Verify(repo => repo.SoftDeleteBruger(It.IsAny<int>()), Times.Once);
        }









    }
}
