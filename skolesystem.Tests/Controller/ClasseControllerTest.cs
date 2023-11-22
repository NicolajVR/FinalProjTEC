using System;
using Microsoft.AspNetCore.Mvc.Infrastructure;
using Moq;
using skolesystem.Controllers;
using skolesystem.DTOs.Classe.Request;
using skolesystem.DTOs.Classe.Response;
using skolesystem.Service.ClasseService;

namespace skolesystem.test.Controller
{
	public class ClasseControllerTest
	{
        private readonly ClasseController _ClasseController;
        private readonly Mock<IClasseService> _mockClasseService = new();

        public ClasseControllerTest()
        {
            _ClasseController = new(_mockClasseService.Object);
        }

        [Fact]
        public async void GetAll_ShouldReturnStatusCode200_WhenClassesExists()
        {

            //arrange
            List<ClasseResponse> Classes = new();

            Classes.Add(new()
            {
                Id = 1,
                className = "ClassA",
                location = "LocationA"
            });

            Classes.Add(new()
            {
                Id = 2,
                className = "ClassB",
                location = "LocationB"
            });

            _mockClasseService
                .Setup(x => x.GetAll()).ReturnsAsync(Classes);
            //act
            var result = await _ClasseController.GetAll();
            //assert
            var statusCodeResult = (IStatusCodeActionResult)result;
            Assert.Equal(200, statusCodeResult.StatusCode);
        }

        [Fact]
        public async void GetAll_ShouldReturnStatusCode204_WhenNoClassesExists()
        {

            //arrange
            List<ClasseResponse> Classes = new();

            _mockClasseService.Setup(x => x.GetAll()).ReturnsAsync(Classes);
            //act
            var result = await _ClasseController.GetAll();
            //assert
            var statusCodeResult = (IStatusCodeActionResult)result;
            Assert.Equal(204, statusCodeResult.StatusCode);
        }

        [Fact]
        public async void GetAll_ShouldReturnStatusCode500_WhenNullIsReturnedFromService()
        {

            //arrange
            _mockClasseService.Setup(x => x.GetAll()).ReturnsAsync(() => null);
            //act
            var result = await _ClasseController.GetAll();
            //assert
            var statusCodeResult = (IStatusCodeActionResult)result;
            Assert.Equal(500, statusCodeResult.StatusCode);
        }

        [Fact]
        public async void GetAll_ShouldReturnStatusCode500_WhenExceptionIsRaised()
        {

            //arrange
            _mockClasseService.Setup(x => x.GetAll()).ReturnsAsync(() => throw new Exception("This is an exception"));
            //act
            var result = await _ClasseController.GetAll();
            //assert
            var statusCodeResult = (IStatusCodeActionResult)result;
            Assert.Equal(500, statusCodeResult.StatusCode);
        }

        [Fact]
        public async void GetById_ShouldReturnStatusCode200_WhenDataExists()
        {
            int ClasseId = 1;

            ClasseResponse category = new()
            {
                Id = ClasseId,
                className = "ClassA",
                location = "LocationA"
            };

            _mockClasseService
                .Setup(x => x.GetById(It.IsAny<int>()))
                .ReturnsAsync(category);
            //act
            var result = await _ClasseController.GetById(ClasseId);

            //assert
            var statusCodeResult = (IStatusCodeActionResult)result;
            Assert.Equal(200, statusCodeResult.StatusCode);
        }

        [Fact]
        public async void GetById_ShouldReturnStatusCode500_WhenClasseDoesNotExists()
        {
            int ClasseId = 1;
            _mockClasseService
                .Setup(x => x.GetById(It.IsAny<int>()))
                .ReturnsAsync(() => null);

            //act
            var result = await _ClasseController.GetById(ClasseId);

            //assert
            var statusCodeResult = (IStatusCodeActionResult)result;
            Assert.Equal(500, statusCodeResult.StatusCode);

        }

        [Fact]
        public async void GetById_ShouldReturnStatusCode500_WhenExceptionIsRaised()
        {
            _mockClasseService
                .Setup(x => x.GetById(It.IsAny<int>()))
                .ReturnsAsync(() => throw new System.Exception("This is an Exception"));

            //act
            var result = await _ClasseController.GetById(1);

            //assert
            var statusCodeResult = (IStatusCodeActionResult)result;
            Assert.Equal(500, statusCodeResult.StatusCode);
        }

        [Fact]
        public async void Create_ShouldReturnStatusCode200_WhenClasseIsSuccessfullyCreated()
        {
            NewClasse newClasse = new()
            {
                className = "ClassA",
                location = "LocationA"
            };
            int ClasseId = 1;

            ClasseResponse ClasseResponse = new()
            {
                Id = ClasseId,
                className = "ClassA",
                location = "LocationA"

            };

            _mockClasseService
                .Setup(x => x.Create(It.IsAny<NewClasse>()))
                .ReturnsAsync(ClasseResponse);

            //act
            var result = await _ClasseController.Create(newClasse);

            //assert
            var statusCodeResult = (IStatusCodeActionResult)result;
            Assert.Equal(200, statusCodeResult.StatusCode);
        }
        [Fact]
        public async void Create_ShouldReturnStatusCode500_WhenExceptionIsRaised()
        {
            NewClasse NewClasse = new()
            {
                className = "ClassA",
                location = "LocationA"
            };
            _mockClasseService
                .Setup(x => x.Create(It.IsAny<NewClasse>()))
                .ReturnsAsync(() => throw new System.Exception("this is an exception"));

            //act
            var result = await _ClasseController.Create(NewClasse);

            //assert
            var statusCodeResult = (IStatusCodeActionResult)result;
            Assert.Equal(500, statusCodeResult.StatusCode);
        }

        [Fact]
        public async void Update_ShouldReturnStatusCode200_WhenClasseIsSuccessfullyUpdated()
        {
            UpdateClasse updateClasse = new()
            {
                className = "ClassA",
                location = "LocationA"
            };
            int ClasseId = 1;

            ClasseResponse ClasseResponse = new()
            {
                Id = ClasseId,
                className = "ClassA",
                location = "LocationA"
            };
            _mockClasseService
                .Setup(x => x.Update(It.IsAny<int>(), It.IsAny<UpdateClasse>()))
                .ReturnsAsync(ClasseResponse);

            //act
            var result = await _ClasseController.Update(ClasseId, updateClasse);

            //assert
            var statusCodeResult = (IStatusCodeActionResult)result;
            Assert.Equal(200, statusCodeResult.StatusCode);
        }

        [Fact]
        public async void Update_ShouldReturnStatusCode500_WhenTryingToUpdateClasseWhichDoesNotExists()
        {
            UpdateClasse UpdateClasse = new()
            {
                className = "ClassA"
            };
            int ClasseId = 1;
            _mockClasseService
                .Setup(x => x.Update(It.IsAny<int>(), It.IsAny<UpdateClasse>()))
                .ReturnsAsync(() => null);

            //act
            var result = await _ClasseController.Update(ClasseId, UpdateClasse);

            //assert
            var statusCodeResult = (IStatusCodeActionResult)result;
            Assert.Equal(500, statusCodeResult.StatusCode);

        }

        [Fact]
        public async void Update_ShouldReturnStatusCode404_WhenExceptionIsRaised()
        {
            UpdateClasse UpdateClasse = new()
            {
                className = "ClassA"
            };

            int ClasseId = 1;
            _mockClasseService
                .Setup(x => x.Update(It.IsAny<int>(), It.IsAny<UpdateClasse>()))
                .ReturnsAsync(() => throw new System.Exception("This is an exception"));
            //act
            var result = await _ClasseController.Update(ClasseId, UpdateClasse);

            //assert
            var statusCodeResult = (IStatusCodeActionResult)result;
            Assert.Equal(500, statusCodeResult.StatusCode);
        }

        [Fact]
        public async void Delete_ShouldReturnStatusCode204_WhenClasseIsDeleted()
        {
            int ClasseId = 1;

            _mockClasseService
                .Setup(s => s.Delete(It.IsAny<int>()))
                .ReturnsAsync(true);

            // Act
            var result = await _ClasseController.Delete(ClasseId);

            // Assert 
            var statusCodeResult = (IStatusCodeActionResult)result;
            Assert.Equal(204, statusCodeResult.StatusCode);

        }

        [Fact]
        public async void Delete_ShouldReturnStatusCode500_WhenExceptionIsRaised()
        {
            int ClasseId = 1;
            _mockClasseService
                .Setup(x => x.Delete(It.IsAny<int>()))
                .ReturnsAsync(() => throw new System.Exception("this is an exception"));

            //act
            var result = await _ClasseController.Delete(ClasseId);

            //assert
            var statusCodeResult = (IStatusCodeActionResult)result;
            Assert.Equal(500, statusCodeResult.StatusCode);
        }
    }
}

