using System;
using System.Collections.Generic;
using System.Threading.Tasks;
using AutoMapper;
using FluentAssertions;
using Moq;
using skolesystem.DTOs;
using skolesystem.Models;
using skolesystem.Repository;
using skolesystem.Service;
using Xunit;

namespace skolesystem.Tests.Services
{
    public class User_informationServiceTests
    {
        private readonly IMapper _mapper;
        private readonly IUser_informationService _user_informationService;
        private readonly Mock<IUser_informationRepository> _user_informationRepositoryMock = new Mock<IUser_informationRepository>();

        public User_informationServiceTests()
        {
            var configuration = new MapperConfiguration(cfg => cfg.AddProfile<MappingProfiles>());
            _mapper = new Mapper(configuration);

            _user_informationService = new User_informationService(_user_informationRepositoryMock.Object, _mapper);
        }

        [Fact]
        public async Task GetUser_informationById_ShouldReturnUser_informationReadDto_WhenUser_informationExists()
        {
            // Arrange
            int user_informationId = 1;
            var existingUser_information = new User_information
            {
                user_information_id = user_informationId,
                // Set other properties
            };

            _user_informationRepositoryMock.Setup(repo => repo.GetById(user_informationId)).ReturnsAsync(existingUser_information);

            // Act
            var result = await _user_informationService.GetUser_informationById(user_informationId);

            // Assert
            result.Should().NotBeNull().And.BeAssignableTo<User_informationReadDto>();
            var resultDto = (User_informationReadDto)result;
            resultDto.user_information_id.Should().Be(user_informationId);
            // Assert other properties as needed
        }

        [Fact]
        public async Task GetUser_informationById_ShouldReturnNull_WhenUser_informationDoesNotExist()
        {
            // Arrange
            int nonExistingUser_informationId = 2;

            _user_informationRepositoryMock.Setup(repo => repo.GetById(nonExistingUser_informationId)).ReturnsAsync((User_information)null);

            // Act
            var result = await _user_informationService.GetUser_informationById(nonExistingUser_informationId);

            // Assert
            result.Should().BeNull();
        }

        [Fact]
        public async Task GetAllUser_informations_ShouldReturnUser_informationReadDtoList_WhenUser_informationsExist()
        {
            // Arrange
            var user_informationsData = new List<User_information>
            {
                
                new User_information
                {
                    user_information_id = 1,
                    name = "John",
                    last_name = "Doe",
                },
                
            };

            _user_informationRepositoryMock.Setup(repo => repo.GetAll()).ReturnsAsync(user_informationsData);

            // Act
            var result = await _user_informationService.GetAllUser_informations();

            // Assert
            result.Should().NotBeNull().And.BeAssignableTo<IEnumerable<User_informationReadDto>>();
            var user_informationDtos = result.Should().BeAssignableTo<IEnumerable<User_informationReadDto>>().Subject;

            user_informationDtos.Should().HaveCount(user_informationsData.Count);

            foreach (var (user_informationDto, user_information) in user_informationDtos.Zip(user_informationsData))
            {
                user_informationDto.Should().BeEquivalentTo(user_information);
            }
        }

        [Fact]
        public async Task GetAllUser_informations_ShouldReturnEmptyList_WhenUser_informationsDoNotExist()
        {
            // Arrange
            _user_informationRepositoryMock.Setup(repo => repo.GetAll()).ReturnsAsync(new List<User_information>());

            // Act
            var result = await _user_informationService.GetAllUser_informations();

            // Assert
            result.Should().NotBeNull().And.BeAssignableTo<IEnumerable<User_informationReadDto>>().And.BeEmpty();
        }

        [Fact]
        public async Task GetDeletedUser_informations_ShouldReturnListOfDeletedUser_informationReadDto_WhenDeletedUser_informationsExist()
        {
            // Arrange
            var deletedUser_informations = new List<User_information>
    {
        new User_information { user_information_id = 1, name = "John", last_name = "Doe", is_deleted = true },
        new User_information { user_information_id = 2, name = "Jane", last_name = "Doe", is_deleted = true },
    };

            _user_informationRepositoryMock.Setup(repo => repo.GetDeletedUser_informations()).ReturnsAsync(deletedUser_informations);

            // Act
            var result = await _user_informationService.GetDeletedUser_informations();

            // Assert
            result.Should().NotBeNull().And.BeAssignableTo<IEnumerable<User_informationReadDto>>();
            var deletedUser_informationList = result.ToList();
            deletedUser_informationList.Should().HaveCount(2);
        }

        [Fact]
        public async Task GetDeletedUser_informations_ShouldReturnEmptyList_WhenNoDeletedUser_informationsExist()
        {
            // Arrange
            _user_informationRepositoryMock.Setup(repo => repo.GetDeletedUser_informations()).ReturnsAsync(new List<User_information>());

            // Act
            var result = await _user_informationService.GetDeletedUser_informations();

            // Assert
            result.Should().NotBeNull().And.BeAssignableTo<IEnumerable<User_informationReadDto>>().And.BeEmpty();
        }

        [Fact]
        public async Task AddUser_information_ShouldReturnCreatedUser_informationReadDto_WhenUser_informationAddedSuccessfully()
        {
            // Arrange
            var user_informationCreateDto = new User_informationCreateDto
            {
                user_information_id = 1,
                name = "John",
                last_name = "Doe",
                
            };

            var createdUser_informationId = 1;

            _user_informationRepositoryMock.Setup(repo => repo.AddUser_information(It.IsAny<User_information>()))
                .Callback<User_information>(user_information =>
                {
                    // Simulate setting the user_information ID when adding to the repository
                    user_information.user_information_id = createdUser_informationId;
                });

            // Act
            var result = await _user_informationService.AddUser_information(user_informationCreateDto);

            // Assert
            result.Should().NotBeNull().And.BeAssignableTo<User_informationReadDto>();
            result.user_information_id.Should().Be(createdUser_informationId);
        }

        [Fact]
        public async Task AddUser_information_ShouldThrowException_WhenUser_informationCreationFails()
        {
            // Arrange
            var user_informationCreateDto = new User_informationCreateDto
            {
                user_information_id = 1,
                name = "John",
                last_name = "Doe",
                
            };

            _user_informationRepositoryMock.Setup(repo => repo.AddUser_information(It.IsAny<User_information>()))
                .ThrowsAsync(new Exception("Failed to create user_information"));

            // Act and Assert
            await Assert.ThrowsAsync<Exception>(() => _user_informationService.AddUser_information(user_informationCreateDto));
        }

        [Fact]
        public async Task UpdateUser_information_ShouldNotThrowException_WhenUser_informationExists()
        {
            // Arrange
            int existingUser_informationId = 1;
            var user_informationUpdateDto = new User_informationUpdateDto
            {
                name = "Updated Name",
                last_name = "Updated Last Name",
                // Add other properties as needed
            };

            _user_informationRepositoryMock.Setup(repo => repo.GetById(existingUser_informationId))
                .ReturnsAsync(new User_information { user_information_id = existingUser_informationId });

            // Act and Assert
            await _user_informationService.Invoking(async x => await x.UpdateUser_information(existingUser_informationId, user_informationUpdateDto))
                .Should().NotThrowAsync<Exception>();
        }

        [Fact]
        public async Task UpdateUser_information_ShouldThrowException_WhenUser_informationDoesNotExist()
        {
            // Arrange
            int nonExistingUser_informationId = 999; // Assuming this ID doesn't exist
            var user_informationUpdateDto = new User_informationUpdateDto
            {
                name = "Updated Name",
                last_name = "Updated Last Name",
                // Add other properties as needed
            };

            _user_informationRepositoryMock.Setup(repo => repo.GetById(nonExistingUser_informationId))
                .ReturnsAsync((User_information)null);

            // Act and Assert
            await Assert.ThrowsAsync<NotFoundException>(() => _user_informationService.UpdateUser_information(nonExistingUser_informationId, user_informationUpdateDto));
        }

        [Fact]
        public async Task SoftDeleteUser_information_ShouldNotThrowException_WhenUser_informationExists()
        {
            // Arrange
            int existingUser_informationId = 1;

            _user_informationRepositoryMock.Setup(repo => repo.GetById(existingUser_informationId))
                .ReturnsAsync(new User_information { user_information_id = existingUser_informationId });

            // Act and Assert
            await _user_informationService.Invoking(async x => await x.SoftDeleteUser_information(existingUser_informationId))
                .Should().NotThrowAsync<Exception>();
        }
        [Fact]
        public async Task SoftDeleteUser_information_ShouldThrowNotFoundException_WhenUser_informationDoesNotExist()
        {
            // Arrange
            int nonExistingUser_informationId = 999; // Assuming this ID doesn't exist

            _user_informationRepositoryMock.Setup(repo => repo.GetById(nonExistingUser_informationId))
                .ReturnsAsync((User_information)null);

            // Act and Assert
            await Assert.ThrowsAsync<NotFoundException>(() => _user_informationService.SoftDeleteUser_information(nonExistingUser_informationId));
        }



    }
}
