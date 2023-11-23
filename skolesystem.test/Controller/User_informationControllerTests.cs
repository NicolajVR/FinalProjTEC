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
    public class User_informationControllerTests
    {
        private readonly Mock<IUser_informationService> _user_informationServiceMock;
        private readonly User_informationController _user_informationController;
        private readonly IUser_informationService _user_informationService;



        public User_informationControllerTests()
        {
            _user_informationServiceMock = new Mock<IUser_informationService>();
            _user_informationController = new User_informationController(_user_informationServiceMock.Object);
        }

        [Fact]
        public async Task GetUser_informations_ShouldReturnUser_informationReadDtos_WhenUser_informationsExist()
        {
            // Arrange
            var user_informations = new List<User_information>
            {
                new User_information { user_information_id = 1, name = "John", last_name = "Doe", phone = "123456", date_of_birth = "1995", address = "123 Main St", is_deleted = false, gender_id = 1, user_id = 1 },
                new User_information { user_information_id = 2, name = "Jane", last_name = "Doe", phone = "654321", date_of_birth = "1995", address = "456 Oak St", is_deleted = false, gender_id = 2, user_id = 2 }
            };

            // Create a mock for IUser_informationService
            var user_informationServiceMock = new Mock<IUser_informationService>();
            user_informationServiceMock.Setup(repo => repo.GetAllUser_informations()).ReturnsAsync(user_informations.Select(b => new User_informationReadDto { }));

            // Inject the mock into the controller
            var user_informationController = new User_informationController(user_informationServiceMock.Object);

            // Act
            var result = await user_informationController.Get();

            // Assert
            result.Should().NotBeNull().And.BeAssignableTo<IEnumerable<User_informationReadDto>>();
            var user_informationDtos = (IEnumerable<User_informationReadDto>)result;
            user_informationDtos.Should().NotBeNullOrEmpty().And.HaveCount(2);
        }



        [Fact]
        public async Task GetUser_informations_ShouldReturnEmptyList_WhenNoUser_informationsExist()
        {
            // Arrange
            _user_informationServiceMock.Setup(repo => repo.GetAllUser_informations()).ReturnsAsync(new List<User_informationReadDto>());

            // Act
            var result = await _user_informationController.Get();

            // Assert
            result.Should().NotBeNull().And.BeAssignableTo<IEnumerable<User_informationReadDto>>();
            var user_informationDtos = (IEnumerable<User_informationReadDto>)result;
            user_informationDtos.Should().BeEmpty();
        }


        [Fact]
        public async Task GetUser_informationById_ShouldReturnUser_informationReadDto_WhenUser_informationExists()
        {
            // Arrange
            int user_informationId = 1;
            var existingUser_information = new User_informationReadDto { user_information_id = user_informationId, name = "John", last_name = "Doe", phone = "123456", date_of_birth = "1998-01-01", address = "123 Main St", is_deleted = false, gender_id = 1, user_id = 1 };

            _user_informationServiceMock.Setup(repo => repo.GetUser_informationById(user_informationId)).ReturnsAsync(existingUser_information);

            // Act
            var result = await _user_informationController.GetById(user_informationId);

            // Assert
            result.Should().NotBeNull().And.BeAssignableTo<OkObjectResult>();
            var okObjectResult = (OkObjectResult)result;
            okObjectResult.Value.Should().NotBeNull().And.BeAssignableTo<User_informationReadDto>();
            var user_informationDto = (User_informationReadDto)okObjectResult.Value;
            user_informationDto.user_information_id.Should().Be(existingUser_information.user_information_id);
        }

        [Fact]
        public async Task GetUser_informationById_ShouldReturnNotFound_WhenUser_informationDoesNotExist()
        {
            // Arrange
            int user_informationId = 1;

            _user_informationServiceMock.Setup(repo => repo.GetUser_informationById(user_informationId)).ReturnsAsync((User_informationReadDto)null);

            // Act
            var result = await _user_informationController.GetById(user_informationId);

            // Assert
            result.Should().NotBeNull().And.BeAssignableTo<NotFoundResult>();
        }

        [Fact]
        public async Task CreateUser_information_ShouldReturnCreatedAtAction_WhenUser_informationIsCreated()
        {
            // Arrange
            var user_informationDto = new User_informationCreateDto
            {
                name = "John",
                last_name = "Doe",
                phone = "123456",
                date_of_birth = "1998-01-01",
                address = "123 Main St",
                is_deleted = false,
                gender_id = 1,
                user_id = 1
            };

            var expectedUser_informationId = 1;

            _user_informationServiceMock.Setup(repo => repo.AddUser_information(It.IsAny<User_informationCreateDto>()))
                .Callback<User_informationCreateDto>(input =>
                {
                    // Simulate setting the user_information_id when adding to the repository
                    // This assumes your AddUser_information method returns the ID
                    input.user_information_id = expectedUser_informationId;
                })
                .ReturnsAsync((User_informationCreateDto input) =>
                {
                    // Create a User_informationReadDto based on the input or your logic
                    var createdUser_informationDto = new User_informationReadDto
                    {
                        user_information_id = expectedUser_informationId,
                        name = input.name,
                        last_name = input.last_name,
                    };

                    return createdUser_informationDto;
                });

            // Act
            var result = await _user_informationController.Create(user_informationDto);

            // Assert
            result.Should().NotBeNull().And.BeAssignableTo<CreatedAtActionResult>();
            var createdAtActionResult = (CreatedAtActionResult)result;
            createdAtActionResult.Value.Should().BeAssignableTo<User_informationReadDto>();
            var resultDto = (User_informationReadDto)createdAtActionResult.Value;
            resultDto.user_information_id.Should().Be(expectedUser_informationId);
            resultDto.name.Should().Be(user_informationDto.name);
            resultDto.last_name.Should().Be(user_informationDto.last_name);
        }





        [Fact]
        public async Task CreateUser_information_ShouldReturnConflict_WhenUser_informationAlreadyExists()
        {
            // Arrange
            var user_informationDto = new User_informationCreateDto
            {
                name = "John",
                last_name = "Doe",
                phone = "123456",
                date_of_birth = "1998-01-01",
                address = "123 Main St",
                is_deleted = false,
                gender_id = 1,
                user_id = 1
            };

            _user_informationServiceMock.Setup(repo => repo.AddUser_information(It.IsAny<User_informationCreateDto>())).ThrowsAsync(new ArgumentException("User with the specified ID already exists"));

            // Act
            var result = await _user_informationController.Create(user_informationDto);

            // Assert
            result.Should().BeAssignableTo<ConflictObjectResult>().And.Match<ConflictObjectResult>(r => r.Value.Equals("User with the specified ID already exists"));
        }

        [Fact]
        public async Task UpdateUser_information_ShouldReturnNoContent_WhenUser_informationExists()
        {
            // Arrange
            int user_informationId = 1;
            var updatedUser_informationDto = new User_informationUpdateDto
            {
                name = "Updated Name",
                last_name = "Updated Last Name",
                phone = "987654",
                date_of_birth = "1990-02-15",
                address = "456 Oak St",
                is_deleted = true
            };

            _user_informationServiceMock.Setup(repo => repo.GetUser_informationById(user_informationId)).ReturnsAsync(new User_informationReadDto { user_information_id = user_informationId });

            // Act
            var result = await _user_informationController.UpdateUser_information(user_informationId, updatedUser_informationDto);

            // Assert
            result.Should().BeOfType<NoContentResult>();
        }





        [Fact]
        public async Task DeleteUser_information_ShouldReturnNoContent_WhenUser_informationExists()
        {
            // Arrange
            int user_informationId = 1;
            var existingUser_information = new User_informationReadDto
            {
                user_information_id = user_informationId,
                name = "John",
                last_name = "Doe",
                phone = "123456",
                date_of_birth = "1998-01-01",
                address = "123 Main St",
                is_deleted = false,
                gender_id = 1,
                user_id = 1
            };

            _user_informationServiceMock.Setup(repo => repo.GetUser_informationById(user_informationId)).ReturnsAsync(existingUser_information);

            // Act
            var result = await _user_informationController.DeleteUser_information(user_informationId);

            // Assert
            result.Should().BeOfType<NoContentResult>();
            _user_informationServiceMock.Verify(repo => repo.SoftDeleteUser_information(It.IsAny<int>()), Times.Once);
        }









    }
}