using System;
using Microsoft.AspNetCore.Mvc.Infrastructure;
using Moq;
using skolesystem.Controllers;
using skolesystem.DTOs.Assignment.Response;
using skolesystem.DTOs.UserSubmission.Request;
using skolesystem.DTOs.UserSubmission.Response;
using skolesystem.Models;
using skolesystem.Service.AssignmentService;
using skolesystem.Service.UserSubmissionService;

namespace skolesystem.test.Controller
{
	public class UserSubmissionControllerTest
	{
        private readonly UserSubmissionController _UserSubmissionController;
        private readonly Mock<IUserSubmissionService> _mockUserSubmissionService = new();

        public UserSubmissionControllerTest()
        {
            _UserSubmissionController = new(_mockUserSubmissionService.Object);
        }

        [Fact]
        public async void GetAll_ShouldReturnStatusCode200_WhenUserSubmissionsExists()
        {

            //arrange
            List<UserSubmissionResponse> userSubmissions = new();

            userSubmissions.Add(new()
            {
                userSubmission_Id = 1,
                userSubmission_text = "My submission for Assignment 1",
                userSubmission_date = DateTime.Now
            });

            userSubmissions.Add(new()
            {
                userSubmission_Id = 2,
                userSubmission_text = "My submission for Assignment 2",
                userSubmission_date = DateTime.Now
            });

            _mockUserSubmissionService
                .Setup(x => x.GetAll()).ReturnsAsync(userSubmissions);
            //act
            var result = await _UserSubmissionController.GetAll();
            //assert
            var statusCodeResult = (IStatusCodeActionResult)result;
            Assert.Equal(200, statusCodeResult.StatusCode);
        }

        [Fact]
        public async void GetAll_ShouldReturnStatusCode204_WhenNoUserSubmissionsExists()
        {

            //arrange
            List<UserSubmissionResponse> userSubmissions = new();

            _mockUserSubmissionService.Setup(x => x.GetAll()).ReturnsAsync(userSubmissions);
            //act
            var result = await _UserSubmissionController.GetAll();
            //assert
            var statusCodeResult = (IStatusCodeActionResult)result;
            Assert.Equal(204, statusCodeResult.StatusCode);
        }

        [Fact]
        public async void GetAll_ShouldReturnStatusCode500_WhenNullIsReturnedFromService()
        {

            //arrange
            _mockUserSubmissionService.Setup(x => x.GetAll()).ReturnsAsync(() => null);
            //act
            var result = await _UserSubmissionController.GetAll();
            //assert
            var statusCodeResult = (IStatusCodeActionResult)result;
            Assert.Equal(500, statusCodeResult.StatusCode);
        }

        [Fact]
        public async void GetAll_ShouldReturnStatusCode500_WhenExceptionIsRaised()
        {

            //arrange
            _mockUserSubmissionService.Setup(x => x.GetAll()).ReturnsAsync(() => throw new Exception("This is an exception"));
            //act
            var result = await _UserSubmissionController.GetAll();
            //assert
            var statusCodeResult = (IStatusCodeActionResult)result;
            Assert.Equal(500, statusCodeResult.StatusCode);
        }

        [Fact]
        public async void GetById_ShouldReturnStatusCode200_WhenDataExists()
        {
            int userSubmissionId = 1;

            UserSubmissionResponse userSubmission = new()
            {
                userSubmission_Id = userSubmissionId,
                userSubmission_text = "My submission for Assignment 1",
                userSubmission_date = DateTime.Now
            };

            _mockUserSubmissionService
                .Setup(x => x.GetById(It.IsAny<int>()))
                .ReturnsAsync(userSubmission);
            //act
            var result = await _UserSubmissionController.GetById(userSubmissionId);

            //assert
            var statusCodeResult = (IStatusCodeActionResult)result;
            Assert.Equal(200, statusCodeResult.StatusCode);
        }

        [Fact]
        public async void GetById_ShouldReturnStatusCode404_WhenUserSubmissionsDoesNotExists()
        {
            int userSubmissionId = 1;
            _mockUserSubmissionService
                .Setup(x => x.GetById(It.IsAny<int>()))
                .ReturnsAsync(() => null);

            //act
            var result = await _UserSubmissionController.GetById(userSubmissionId);

            //assert
            var statusCodeResult = (IStatusCodeActionResult)result;
            Assert.Equal(404, statusCodeResult.StatusCode);

        }

        [Fact]
        public async void GetById_ShouldReturnStatusCode500_WhenExceptionIsRaised()
        {
            _mockUserSubmissionService
                .Setup(x => x.GetById(It.IsAny<int>()))
                .ReturnsAsync(() => throw new System.Exception("This is an Exception"));

            //act
            var result = await _UserSubmissionController.GetById(1);

            //assert
            var statusCodeResult = (IStatusCodeActionResult)result;
            Assert.Equal(500, statusCodeResult.StatusCode);
        }

        [Fact]
        public async void Create_ShouldReturnStatusCode200_WhenUserSubmissionsIsSuccessfullyCreated()
        {
            NewUserSubmission NewUserSubmission = new()
            {
                userSubmission_text = "My submission for Assignment 1",
                userSubmission_date = DateTime.Now
            };
            int userSubmissionId = 1;

            UserSubmissionResponse UserSubmissionResponse = new()
            {
                userSubmission_Id = userSubmissionId,
                userSubmission_text = "My submission for Assignment 1",
                userSubmission_date = DateTime.Now

            };

            _mockUserSubmissionService
                .Setup(x => x.Create(It.IsAny<NewUserSubmission>()))
                .ReturnsAsync(UserSubmissionResponse);

            //act
            var result = await _UserSubmissionController.Create(NewUserSubmission);

            //assert
            var statusCodeResult = (IStatusCodeActionResult)result;
            Assert.Equal(200, statusCodeResult.StatusCode);
        }
        [Fact]
        public async void Create_ShouldReturnStatusCode500_WhenExceptionIsRaised()
        {
            NewUserSubmission NewUserSubmission = new()
            {
                userSubmission_text = "My submission for Assignment 1",
                userSubmission_date = DateTime.Now
            };
            _mockUserSubmissionService
                .Setup(x => x.Create(It.IsAny<NewUserSubmission>()))
                .ReturnsAsync(() => throw new System.Exception("this is an exception"));

            //act
            var result = await _UserSubmissionController.Create(NewUserSubmission);

            //assert
            var statusCodeResult = (IStatusCodeActionResult)result;
            Assert.Equal(500, statusCodeResult.StatusCode);
        }

        [Fact]
        public async void Update_ShouldReturnStatusCode200_WhenUserSubmissionsIsSuccessfullyUpdated()
        {
            UpdateUserSubmission updateUserSubmission = new()
            {
                userSubmission_text = "My submission for Assignment 1",
                userSubmission_date = DateTime.Now
            };
            int userSubmissionId = 1;

            UserSubmissionResponse UserSubmissionResponse = new()
            {
                userSubmission_Id = userSubmissionId,
                userSubmission_text = "My submission for Assignment 1",
                userSubmission_date = DateTime.Now
            };
            _mockUserSubmissionService
                .Setup(x => x.Update(It.IsAny<int>(), It.IsAny<UpdateUserSubmission>()))
                .ReturnsAsync(UserSubmissionResponse);

            //act
            var result = await _UserSubmissionController.Update(userSubmissionId, updateUserSubmission);

            //assert
            var statusCodeResult = (IStatusCodeActionResult)result;
            Assert.Equal(200, statusCodeResult.StatusCode);
        }

        [Fact]
        public async void Update_ShouldReturnStatusCode500_WhenTryingToUpdateUserSubmissionsWhichDoesNotExists()
        {
            UpdateUserSubmission updateUserSubmission = new()
            {
                userSubmission_text = "My submission for Assignment 1",
                userSubmission_date = DateTime.Now
            };
            int userSubmissionId = 1;
            _mockUserSubmissionService
                .Setup(x => x.Update(It.IsAny<int>(), It.IsAny<UpdateUserSubmission>()))
                .ReturnsAsync(() => null);

            //act
            var result = await _UserSubmissionController.Update(userSubmissionId, updateUserSubmission);

            //assert
            var statusCodeResult = (IStatusCodeActionResult)result;
            Assert.Equal(500, statusCodeResult.StatusCode);

        }

        [Fact]
        public async void Update_ShouldReturnStatusCode404_WhenExceptionIsRaised()
        {
            UpdateUserSubmission updateUserSubmission = new()
            {
                userSubmission_text = "My submission for Assignment 1",
                userSubmission_date = DateTime.Now
            };

            int userSubmissionId = 1;
            _mockUserSubmissionService
                .Setup(x => x.Update(It.IsAny<int>(), It.IsAny<UpdateUserSubmission>()))
                .ReturnsAsync(() => throw new System.Exception("This is an exception"));
            //act
            var result = await _UserSubmissionController.Update(userSubmissionId, updateUserSubmission);

            //assert
            var statusCodeResult = (IStatusCodeActionResult)result;
            Assert.Equal(500, statusCodeResult.StatusCode);
        }

        [Fact]
        public async void Delete_ShouldReturnStatusCode204_WhenUserSubmissionsIsDeleted()
        {
            int userSubmissionId = 1;

            _mockUserSubmissionService
                .Setup(s => s.Delete(It.IsAny<int>()))
                .ReturnsAsync(true);

            // Act
            var result = await _UserSubmissionController.Delete(userSubmissionId);

            // Assert 
            var statusCodeResult = (IStatusCodeActionResult)result;
            Assert.Equal(204, statusCodeResult.StatusCode);

        }

        [Fact]
        public async void Delete_ShouldReturnStatusCode500_WhenExceptionIsRaised()
        {
            int userSubmissionId = 1;
            _mockUserSubmissionService
                .Setup(x => x.Delete(It.IsAny<int>()))
                .ReturnsAsync(() => throw new System.Exception("this is an exception"));

            //act
            var result = await _UserSubmissionController.Delete(userSubmissionId);

            //assert
            var statusCodeResult = (IStatusCodeActionResult)result;
            Assert.Equal(500, statusCodeResult.StatusCode);
        }
    }
}

