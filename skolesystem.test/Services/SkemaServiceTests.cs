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
    public class SkemaServiceTests
    {
        [Fact]
        public async Task GetAllSchemata_ShouldReturnAllSchemata_WhenSchemataExist()
        {
            // Arrange
            var schemataData = new List<Skema>
        {
            new Skema { schedule_id = 1, /* other properties */ },
            new Skema { schedule_id = 2, /* other properties */ },
            // Add more Skema objects as needed
        };

            var skemaRepositoryMock = new Mock<ISkemaRepository>();
            skemaRepositoryMock.Setup(repo => repo.GetAll()).ReturnsAsync(schemataData);

            var skemaService = new SkemaService(skemaRepositoryMock.Object);

            // Act
            var result = await skemaService.GetAllSchemata();

            // Assert
            Assert.NotNull(result);
            Assert.IsAssignableFrom<IEnumerable<Skema>>(result);
            Assert.Equal(schemataData.Count, result.Count());
            Assert.Equal(schemataData, result);
        }

        [Fact]
        public async Task GetAllSchemata_ShouldReturnEmptyList_WhenNoSchemataExist()
        {
            // Arrange
            var skemaRepositoryMock = new Mock<ISkemaRepository>();
            skemaRepositoryMock.Setup(repo => repo.GetAll()).ReturnsAsync(new List<Skema>());

            var skemaService = new SkemaService(skemaRepositoryMock.Object);

            // Act
            var result = await skemaService.GetAllSchemata();

            // Assert
            Assert.NotNull(result);
            Assert.IsAssignableFrom<IEnumerable<Skema>>(result);
            Assert.Empty(result);
        }

        [Fact]
        public async Task GetSkemaById_ShouldReturnSkema_WhenSkemaExists()
        {
            // Arrange
            int skemaId = 1;
            var existingSkema = new Skema { schedule_id = skemaId, /* other properties */ };

            var skemaRepositoryMock = new Mock<ISkemaRepository>();
            skemaRepositoryMock.Setup(repo => repo.GetById(skemaId)).ReturnsAsync(existingSkema);

            var skemaService = new SkemaService(skemaRepositoryMock.Object);

            // Act
            var result = await skemaService.GetSkemaById(skemaId);

            // Assert
            Assert.NotNull(result);
            Assert.IsType<Skema>(result);
            Assert.Equal(skemaId, result.schedule_id);
            // Additional assertions based on your Skema model properties
        }

        [Fact]
        public async Task GetSkemaById_ShouldReturnNull_WhenSkemaDoesNotExist()
        {
            // Arrange
            int nonExistingSkemaId = 999; // Assuming this ID doesn't exist

            var skemaRepositoryMock = new Mock<ISkemaRepository>();
            skemaRepositoryMock.Setup(repo => repo.GetById(nonExistingSkemaId)).ReturnsAsync((Skema)null);

            var skemaService = new SkemaService(skemaRepositoryMock.Object);

            // Act
            var result = await skemaService.GetSkemaById(nonExistingSkemaId);

            // Assert
            Assert.Null(result);
        }
        [Fact]
        public async Task CreateSkema_ShouldReturnSkemaId_WhenSkemaCreatedSuccessfully()
        {
            // Arrange
            var newSkema = new Skema { /* initialize Skema properties */ };
            int createdSkemaId = 1;

            var skemaRepositoryMock = new Mock<ISkemaRepository>();
            skemaRepositoryMock.Setup(repo => repo.Create(newSkema)).ReturnsAsync(createdSkemaId);

            var skemaService = new SkemaService(skemaRepositoryMock.Object);

            // Act
            var result = await skemaService.CreateSkema(newSkema);

            // Assert
            Assert.Equal(createdSkemaId, result);
        }
        [Fact]
        public async Task CreateSkema_ShouldThrowException_WhenSkemaCreationFails()
        {
            // Arrange
            var newSkema = new Skema {};

            var skemaRepositoryMock = new Mock<ISkemaRepository>();
            skemaRepositoryMock.Setup(repo => repo.Create(newSkema)).ThrowsAsync(new Exception("Failed to create Skema"));

            var skemaService = new SkemaService(skemaRepositoryMock.Object);

            // Act and Assert
            await Assert.ThrowsAsync<Exception>(() => skemaService.CreateSkema(newSkema));
        }

        [Fact]
        public async Task UpdateSkema_ShouldNotThrowException_WhenSkemaExists()
        {
            // Arrange
            int existingSkemaId = 1;
            var skemaUpdateDto = new SkemaCreateDto { /* initialize SkemaCreateDto properties */ };

            var skemaRepositoryMock = new Mock<ISkemaRepository>();
            skemaRepositoryMock.Setup(repo => repo.GetById(existingSkemaId)).ReturnsAsync(new Skema { schedule_id = existingSkemaId });

            var skemaService = new SkemaService(skemaRepositoryMock.Object);

            // Act and Assert
            await skemaService.Invoking(async x => await x.UpdateSkema(existingSkemaId, skemaUpdateDto))
                .Should().NotThrowAsync<Exception>();
        }

        [Fact]
        public async Task UpdateSkema_ShouldThrowException_WhenSkemaDoesNotExist()
        {
            // Arrange
            int nonExistingSkemaId = 999; // Assuming this ID doesn't exist
            var skemaUpdateDto = new SkemaCreateDto { /* initialize SkemaCreateDto properties */ };

            var skemaRepositoryMock = new Mock<ISkemaRepository>();
            skemaRepositoryMock.Setup(repo => repo.GetById(nonExistingSkemaId)).ReturnsAsync((Skema)null);

            var skemaService = new SkemaService(skemaRepositoryMock.Object);

            // Act and Assert
            await Assert.ThrowsAsync<ArgumentException>(() => skemaService.UpdateSkema(nonExistingSkemaId, skemaUpdateDto));
        }

        [Fact]
        public async Task DeleteSkema_ShouldNotThrowException_WhenSkemaExists()
        {
            // Arrange
            int existingSkemaId = 1;

            var skemaRepositoryMock = new Mock<ISkemaRepository>();
            skemaRepositoryMock.Setup(repo => repo.GetById(existingSkemaId)).ReturnsAsync(new Skema { /* initialize Skema properties */ });

            var skemaService = new SkemaService(skemaRepositoryMock.Object);

            // Act and Assert
            await skemaService.Invoking(async x => await x.DeleteSkema(existingSkemaId))
                .Should().NotThrowAsync<Exception>();
        }

        [Fact]
        public async Task DeleteSkema_ShouldDeleteSkema_WhenSkemaExists()
        {
            // Arrange
            int existingSkemaId = 1;

            var skemaRepositoryMock = new Mock<ISkemaRepository>();
            skemaRepositoryMock.Setup(repo => repo.GetById(existingSkemaId)).ReturnsAsync(new Skema { schedule_id = existingSkemaId });

            var skemaService = new SkemaService(skemaRepositoryMock.Object);

            // Act
            await skemaService.DeleteSkema(existingSkemaId);

            // Assert
            skemaRepositoryMock.Verify(repo => repo.Delete(existingSkemaId), Times.Once);
        }






    }
}
