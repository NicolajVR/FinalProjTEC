using FakeItEasy;
using skolesystem.DTOs;
using skolesystem.Service;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using skolesystem.Controllers;
using FluentAssertions;
using Microsoft.AspNetCore.Mvc;
using Microsoft.AspNetCore.Mvc.Infrastructure;
using Moq;
using Microsoft.AspNetCore.Http;

namespace skolesystem.Tests.Controller
{
    public class UsersControllerTests
    {
        private readonly UserController _userController;
        private readonly Mock<IUsersService> _usersService = new();


        public UsersControllerTests()
        {
            _userController = new UserController(context: null, _usersService.Object);
        }

        [Fact]
        public async Task GetUsers_ShouldReturnOk_WhenDataExists()
        {
            // Arrange
            List<UserReadDto> users = new List<UserReadDto>
    {
        new UserReadDto
        {
            user_id = 1,
            surname = "Doe",
            email = "john.doe@example.com",
        },
        new UserReadDto
        {
            user_id = 2,
            surname = "Smith",
            email = "jane.smith@example.com",
        }
    };

            _usersService.Setup(s => s.GetAllUsers()).ReturnsAsync(users);

            // Act
            var result = await _userController.GetUsers();

            // Assert
            result.Should().NotBeNull().And.BeAssignableTo<IEnumerable<UserReadDto>>();
            var userDtos = result.Should().BeAssignableTo<List<UserReadDto>>().Subject;

            // Assert
            userDtos.Should().HaveCount(2);
            userDtos.Should().ContainSingle(u => u.user_id == 1);
            userDtos.Should().ContainSingle(u => u.user_id == 2);
        }


        [Fact]
        public async Task GetUsers_ShouldReturnOk_WhenNoDataExists()
        {
            // Arrange
            //Laver en tom liste
            List<UserReadDto> users = new List<UserReadDto>();
            _usersService.Setup(s => s.GetAllUsers()).Returns(Task.FromResult<IEnumerable<UserReadDto>>(users));

            // Act
            var result = await _userController.GetUsers();

            // Assert
            result.Should().NotBeNull().And.BeAssignableTo<IEnumerable<UserReadDto>>();
            var userDtos = result.Should().BeAssignableTo<IEnumerable<UserReadDto>>().Subject;
        }
        [Fact]
        public async Task GetUserById_ShouldReturnOk_WhenUserExists()
        {
            // Arrange
            int userId = 1;
            var user = new UserReadDto
            {
                user_id = userId,
            };

            _usersService.Setup(s => s.GetUserById(userId)).ReturnsAsync(user);

            // Act
            var result = await _userController.GetUserById(userId);

            // Assert
            result.Should().NotBeNull().And.BeAssignableTo<OkObjectResult>();
            var okResult = result.Should().BeOfType<OkObjectResult>().Subject;

            okResult.Value.Should().BeOfType<UserReadDto>().Which.user_id.Should().Be(userId);
            
         
        }

        [Fact]
        public async Task GetUserById_ShouldReturnNotFound_WhenUserDoesNotExist()
        {
            // Arrange
            int userId = 1;
            _usersService.Setup(s => s.GetUserById(userId)).ReturnsAsync((UserReadDto)null);

            // Act
            var result = await _userController.GetUserById(userId);

            // Assert
            result.Should().NotBeNull().And.BeAssignableTo<NotFoundResult>();
        }

        [Fact]
        public async Task CreateUser_ShouldReturnStatusCode201_WhenDataIsCreated()
        {
            // Arrange
            var userDto = new UserCreateDto
            {
                surname = "Doe",
                email = "john.doe@example.com",
                password_hash = "passwordHash",
                user_id = 0,
                email_confirmed = 1,
                lockout_enabled = 1,
                phone_confirmed = 1,
                twofactor_enabled = 1,
                try_failed_count = 1,
                lockout_end = 1,
                user_information_id = 1,
            };

            _usersService
                .Setup(s => s.AddUser(It.IsAny<UserCreateDto>()))
                .Returns(Task.FromResult(userDto));


            // Act
            var result = await _userController.CreateUser(userDto);


            // Assert
            var statusCodeResult = (IStatusCodeActionResult)result;
            Assert.Equal(201, statusCodeResult.StatusCode);

        }

        [Fact]
        public async Task Create_ShouldReturnStatusCode500_WhenExceptionIsRaised()
        {
            // Arrange
            var userDto = new UserCreateDto
            {
                surname = "Doe",
                email = "john.doe@example.com",
                password_hash = "passwordHash",
                user_id = 0,
                email_confirmed = 1,
                lockout_enabled = 1,
                phone_confirmed = 1,
                twofactor_enabled = 1,
                try_failed_count = 1,
                lockout_end = 1,
                user_information_id = 1,
            };

            _usersService
                .Setup(s => s.AddUser(It.IsAny<UserCreateDto>()))
                .Returns(() => throw new System.Exception("This is an exception"));

            // Act
            var result = await _userController.CreateUser(userDto);

            // Assert
            var statusCodeResult = (IStatusCodeActionResult)result;
            Assert.Equal(500, statusCodeResult.StatusCode);

        }

        [Fact]
        public async Task Update_ShouldReturnStatusCode200_WhenDataIsSaved()
        {
            // Arrange
            int userId = 1;
            UserUpdateDto updateUser = new UserUpdateDto
            {
                surname = "Doe",
                email = "john.doe@example.com",
                password_hash = "passwordHash",
                user_id = 0,
                email_confirmed = 1,
                lockout_enabled = 1,
                phone_confirmed = 1,
                twofactor_enabled = 1,
                try_failed_count = 1,
                lockout_end = 1,
                user_information_id = 1,
                is_deleted = false,
            };

            _usersService
                .Setup(s => s.UpdateUser(It.IsAny<int>(), It.IsAny<UserUpdateDto>()))
                .Returns(Task.CompletedTask); // Assuming your UpdateUser method returns Task

            // Act
            var result = await _userController.UpdateUser(userId, updateUser);

            // Assert
            var statusCodeResult = (IStatusCodeActionResult)result;
            Assert.Equal(200, statusCodeResult.StatusCode);
        }






        [Fact]
        public async Task Update_ShouldReturnStatusCode500_WhenExceptionIsRaised()
        {
            // Arrange
            int userId = 1;
            UserUpdateDto updateUser = new UserUpdateDto
            {
                surname = "Doe",
                email = "john.doe@example.com",
                password_hash = "passwordHash",
                user_id = 0,
                email_confirmed = 1,
                lockout_enabled = 1,
                phone_confirmed = 1,
                twofactor_enabled = 1,
                try_failed_count = 1,
                lockout_end = 1,
                user_information_id = 1,
            };

            _usersService
                .Setup(s => s.UpdateUser(It.IsAny<int>(), It.IsAny<UserUpdateDto>()))
                .ThrowsAsync(new System.Exception("This is an exception"));

            // Act
            Func<Task> act = async () => await _userController.UpdateUser(userId, updateUser);

            // Assert
            await Assert.ThrowsAsync<System.Exception>(act);
        }





















    }
}
