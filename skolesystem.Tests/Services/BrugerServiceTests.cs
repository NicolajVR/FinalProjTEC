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
    public class BrugerServiceTests
    {
        private readonly IMapper _mapper;
        private readonly IBrugerService _brugerService;
        private readonly Mock<IBrugerRepository> _brugerRepositoryMock = new Mock<IBrugerRepository>();

        public BrugerServiceTests()
        {
            var configuration = new MapperConfiguration(cfg => cfg.AddProfile<MappingProfiles>());
            _mapper = new Mapper(configuration);

            _brugerService = new BrugerService(_brugerRepositoryMock.Object, _mapper);
        }

        [Fact]
        public async Task GetBrugerById_ShouldReturnBrugerReadDto_WhenBrugerExists()
        {
            // Arrange
            int brugerId = 1;
            var existingBruger = new Bruger
            {
                user_information_id = brugerId,
                // Set other properties
            };

            _brugerRepositoryMock.Setup(repo => repo.GetById(brugerId)).ReturnsAsync(existingBruger);

            // Act
            var result = await _brugerService.GetBrugerById(brugerId);

            // Assert
            result.Should().NotBeNull().And.BeAssignableTo<BrugerReadDto>();
            var resultDto = (BrugerReadDto)result;
            resultDto.user_information_id.Should().Be(brugerId);
            // Assert other properties as needed
        }

        [Fact]
        public async Task GetBrugerById_ShouldReturnNull_WhenBrugerDoesNotExist()
        {
            // Arrange
            int nonExistingBrugerId = 2;

            _brugerRepositoryMock.Setup(repo => repo.GetById(nonExistingBrugerId)).ReturnsAsync((Bruger)null);

            // Act
            var result = await _brugerService.GetBrugerById(nonExistingBrugerId);

            // Assert
            result.Should().BeNull();
        }

        [Fact]
        public async Task GetAllBrugers_ShouldReturnBrugerReadDtoList_WhenBrugersExist()
        {
            // Arrange
            var brugersData = new List<Bruger>
            {
                
                new Bruger
                {
                    user_information_id = 1,
                    name = "John",
                    last_name = "Doe",
                },
                
            };

            _brugerRepositoryMock.Setup(repo => repo.GetAll()).ReturnsAsync(brugersData);

            // Act
            var result = await _brugerService.GetAllBrugers();

            // Assert
            result.Should().NotBeNull().And.BeAssignableTo<IEnumerable<BrugerReadDto>>();
            var brugerDtos = result.Should().BeAssignableTo<IEnumerable<BrugerReadDto>>().Subject;

            brugerDtos.Should().HaveCount(brugersData.Count);

            foreach (var (brugerDto, bruger) in brugerDtos.Zip(brugersData))
            {
                brugerDto.Should().BeEquivalentTo(bruger);
            }
        }

        [Fact]
        public async Task GetAllBrugers_ShouldReturnEmptyList_WhenBrugersDoNotExist()
        {
            // Arrange
            _brugerRepositoryMock.Setup(repo => repo.GetAll()).ReturnsAsync(new List<Bruger>());

            // Act
            var result = await _brugerService.GetAllBrugers();

            // Assert
            result.Should().NotBeNull().And.BeAssignableTo<IEnumerable<BrugerReadDto>>().And.BeEmpty();
        }

        [Fact]
        public async Task GetDeletedBrugers_ShouldReturnListOfDeletedBrugerReadDto_WhenDeletedBrugersExist()
        {
            // Arrange
            var deletedBrugers = new List<Bruger>
    {
        new Bruger { user_information_id = 1, name = "John", last_name = "Doe", is_deleted = true },
        new Bruger { user_information_id = 2, name = "Jane", last_name = "Doe", is_deleted = true },
    };

            _brugerRepositoryMock.Setup(repo => repo.GetDeletedBrugers()).ReturnsAsync(deletedBrugers);

            // Act
            var result = await _brugerService.GetDeletedBrugers();

            // Assert
            result.Should().NotBeNull().And.BeAssignableTo<IEnumerable<BrugerReadDto>>();
            var deletedBrugerList = result.ToList();
            deletedBrugerList.Should().HaveCount(2);
        }

        [Fact]
        public async Task GetDeletedBrugers_ShouldReturnEmptyList_WhenNoDeletedBrugersExist()
        {
            // Arrange
            _brugerRepositoryMock.Setup(repo => repo.GetDeletedBrugers()).ReturnsAsync(new List<Bruger>());

            // Act
            var result = await _brugerService.GetDeletedBrugers();

            // Assert
            result.Should().NotBeNull().And.BeAssignableTo<IEnumerable<BrugerReadDto>>().And.BeEmpty();
        }

        [Fact]
        public async Task AddBruger_ShouldReturnCreatedBrugerReadDto_WhenBrugerAddedSuccessfully()
        {
            // Arrange
            var brugerCreateDto = new BrugerCreateDto
            {
                user_information_id = 1,
                name = "John",
                last_name = "Doe",
                
            };

            var createdBrugerId = 1;

            _brugerRepositoryMock.Setup(repo => repo.AddBruger(It.IsAny<Bruger>()))
                .Callback<Bruger>(bruger =>
                {
                    // Simulate setting the bruger ID when adding to the repository
                    bruger.user_information_id = createdBrugerId;
                });

            // Act
            var result = await _brugerService.AddBruger(brugerCreateDto);

            // Assert
            result.Should().NotBeNull().And.BeAssignableTo<BrugerReadDto>();
            result.user_information_id.Should().Be(createdBrugerId);
        }

        [Fact]
        public async Task AddBruger_ShouldThrowException_WhenBrugerCreationFails()
        {
            // Arrange
            var brugerCreateDto = new BrugerCreateDto
            {
                user_information_id = 1,
                name = "John",
                last_name = "Doe",
                
            };

            _brugerRepositoryMock.Setup(repo => repo.AddBruger(It.IsAny<Bruger>()))
                .ThrowsAsync(new Exception("Failed to create bruger"));

            // Act and Assert
            await Assert.ThrowsAsync<Exception>(() => _brugerService.AddBruger(brugerCreateDto));
        }

        [Fact]
        public async Task UpdateBruger_ShouldNotThrowException_WhenBrugerExists()
        {
            // Arrange
            int existingBrugerId = 1;
            var brugerUpdateDto = new BrugerUpdateDto
            {
                name = "Updated Name",
                last_name = "Updated Last Name",
                // Add other properties as needed
            };

            _brugerRepositoryMock.Setup(repo => repo.GetById(existingBrugerId))
                .ReturnsAsync(new Bruger { user_information_id = existingBrugerId });

            // Act and Assert
            await _brugerService.Invoking(async x => await x.UpdateBruger(existingBrugerId, brugerUpdateDto))
                .Should().NotThrowAsync<Exception>();
        }

        [Fact]
        public async Task UpdateBruger_ShouldThrowException_WhenBrugerDoesNotExist()
        {
            // Arrange
            int nonExistingBrugerId = 999; // Assuming this ID doesn't exist
            var brugerUpdateDto = new BrugerUpdateDto
            {
                name = "Updated Name",
                last_name = "Updated Last Name",
                // Add other properties as needed
            };

            _brugerRepositoryMock.Setup(repo => repo.GetById(nonExistingBrugerId))
                .ReturnsAsync((Bruger)null);

            // Act and Assert
            await Assert.ThrowsAsync<NotFoundException>(() => _brugerService.UpdateBruger(nonExistingBrugerId, brugerUpdateDto));
        }

        [Fact]
        public async Task SoftDeleteBruger_ShouldNotThrowException_WhenBrugerExists()
        {
            // Arrange
            int existingBrugerId = 1;

            _brugerRepositoryMock.Setup(repo => repo.GetById(existingBrugerId))
                .ReturnsAsync(new Bruger { user_information_id = existingBrugerId });

            // Act and Assert
            await _brugerService.Invoking(async x => await x.SoftDeleteBruger(existingBrugerId))
                .Should().NotThrowAsync<Exception>();
        }
        [Fact]
        public async Task SoftDeleteBruger_ShouldThrowNotFoundException_WhenBrugerDoesNotExist()
        {
            // Arrange
            int nonExistingBrugerId = 999; // Assuming this ID doesn't exist

            _brugerRepositoryMock.Setup(repo => repo.GetById(nonExistingBrugerId))
                .ReturnsAsync((Bruger)null);

            // Act and Assert
            await Assert.ThrowsAsync<NotFoundException>(() => _brugerService.SoftDeleteBruger(nonExistingBrugerId));
        }



    }
}
