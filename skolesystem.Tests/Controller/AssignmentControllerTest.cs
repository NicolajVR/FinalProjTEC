using System;
using Microsoft.AspNetCore.Mvc.Infrastructure;
using Moq;
using skolesystem.Controllers;
using skolesystem.DTOs.Assignment.Response;
using skolesystem.DTOs.Assignment.Request;
using skolesystem.Service.AssignmentService;
using skolesystem.Models;

namespace skolesystem.test.Controller
{
	public class AssignmentControllerTest
	{
        private readonly AssignmentController _AssignmentController;
        private readonly Mock<IAssignmentService> _mockAssignmentService = new();

        public AssignmentControllerTest()
        {
            _AssignmentController = new(_mockAssignmentService.Object);
        }

        [Fact]
        public async void GetAll_ShouldReturnStatusCode200_WhenAssignmentExists()
        {

            //arrange
            List<AssignmentResponse> assignments = new();

            assignments.Add(new()
            {
                assignment_id = 1,
                assignment_description = "Assignment 1 for ClassA",
                assignment_deadline = DateTime.Now
            });

            assignments.Add(new()
            {
                assignment_id = 2,
                assignment_description = "Assignment 2 for ClassB",
                assignment_deadline = DateTime.Now
            });

            _mockAssignmentService
                .Setup(x => x.GetAll()).ReturnsAsync(assignments);
            //act
            var result = await _AssignmentController.GetAll();
            //assert
            var statusCodeResult = (IStatusCodeActionResult)result;
            Assert.Equal(200, statusCodeResult.StatusCode);
        }

        [Fact]
        public async void GetAll_ShouldReturnStatusCode204_WhenNoAssignmentExists()
        {

            //arrange
            List<AssignmentResponse> Classes = new();

            _mockAssignmentService.Setup(x => x.GetAll()).ReturnsAsync(Classes);
            //act
            var result = await _AssignmentController.GetAll();
            //assert
            var statusCodeResult = (IStatusCodeActionResult)result;
            Assert.Equal(204, statusCodeResult.StatusCode);
        }

        [Fact]
        public async void GetAll_ShouldReturnStatusCode500_WhenNullIsReturnedFromService()
        {

            //arrange
            _mockAssignmentService.Setup(x => x.GetAll()).ReturnsAsync(() => null);
            //act
            var result = await _AssignmentController.GetAll();
            //assert
            var statusCodeResult = (IStatusCodeActionResult)result;
            Assert.Equal(500, statusCodeResult.StatusCode);
        }

        [Fact]
        public async void GetAll_ShouldReturnStatusCode500_WhenExceptionIsRaised()
        {

            //arrange
            _mockAssignmentService.Setup(x => x.GetAll()).ReturnsAsync(() => throw new Exception("This is an exception"));
            //act
            var result = await _AssignmentController.GetAll();
            //assert
            var statusCodeResult = (IStatusCodeActionResult)result;
            Assert.Equal(500, statusCodeResult.StatusCode);
        }

        [Fact]
        public async void GetById_ShouldReturnStatusCode200_WhenDataExists()
        {
            int AssignmentId = 1;

            AssignmentResponse category = new()
            {
                assignment_id = AssignmentId,
                assignment_description = "Assignment 1 for ClassA",
                assignment_deadline = DateTime.Now
            };

            _mockAssignmentService
                .Setup(x => x.GetById(It.IsAny<int>()))
                .ReturnsAsync(category);
            //act
            var result = await _AssignmentController.GetById(AssignmentId);

            //assert
            var statusCodeResult = (IStatusCodeActionResult)result;
            Assert.Equal(200, statusCodeResult.StatusCode);
        }

        [Fact]
        public async void GetById_ShouldReturnStatusCode404_WhenAssignmentDoesNotExists()
        {
            int AssignmentId = 1;
            _mockAssignmentService
                .Setup(x => x.GetById(It.IsAny<int>()))
                .ReturnsAsync(() => null);

            //act
            var result = await _AssignmentController.GetById(AssignmentId);

            //assert
            var statusCodeResult = (IStatusCodeActionResult)result;
            Assert.Equal(404, statusCodeResult.StatusCode);

        }

        [Fact]
        public async void GetById_ShouldReturnStatusCode500_WhenExceptionIsRaised()
        {
            _mockAssignmentService
                .Setup(x => x.GetById(It.IsAny<int>()))
                .ReturnsAsync(() => throw new System.Exception("This is an Exception"));

            //act
            var result = await _AssignmentController.GetById(1);

            //assert
            var statusCodeResult = (IStatusCodeActionResult)result;
            Assert.Equal(500, statusCodeResult.StatusCode);
        }

        [Fact]
        public async void Create_ShouldReturnStatusCode200_WhenAssignmentIsSuccessfullyCreated()
        {
            NewAssignment NewAssignment = new()
            {
                assignment_Description = "Assignment 1 for ClassA",
                assignment_Deadline = DateTime.Now
            };
            int AssignmentId = 1;

            AssignmentResponse AssignmentResponse = new()
            {
                assignment_id = AssignmentId,
                assignment_description = "Assignment 1 for ClassA",
                assignment_deadline = DateTime.Now

            };

            _mockAssignmentService
                .Setup(x => x.Create(It.IsAny<NewAssignment>()))
                .ReturnsAsync(AssignmentResponse);

            //act
            var result = await _AssignmentController.Create(NewAssignment);

            //assert
            var statusCodeResult = (IStatusCodeActionResult)result;
            Assert.Equal(200, statusCodeResult.StatusCode);
        }
        [Fact]
        public async void Create_ShouldReturnStatusCode500_WhenExceptionIsRaised()
        {
            NewAssignment NewAssignment = new()
            {
                assignment_Description = "Assignment 1 for ClassA",
                assignment_Deadline = DateTime.Now
            };
            _mockAssignmentService
                .Setup(x => x.Create(It.IsAny<NewAssignment>()))
                .ReturnsAsync(() => throw new System.Exception("this is an exception"));

            //act
            var result = await _AssignmentController.Create(NewAssignment);

            //assert
            var statusCodeResult = (IStatusCodeActionResult)result;
            Assert.Equal(500, statusCodeResult.StatusCode);
        }

        [Fact]
        public async void Update_ShouldReturnStatusCode200_WhenAssignmentIsSuccessfullyUpdated()
        {
            UpdateAssignment UpdateAssignment = new()
            {
                assignment_Description = "Assignment 1 for ClassA",
                assignment_Deadline = DateTime.Now
            };
            int AssignmentId = 1;

            AssignmentResponse AssignmentResponse = new()
            {
                assignment_id = AssignmentId,
                assignment_description = "Assignment 1 for ClassA",
                assignment_deadline = DateTime.Now
            };
            _mockAssignmentService
                .Setup(x => x.Update(It.IsAny<int>(), It.IsAny<UpdateAssignment>()))
                .ReturnsAsync(AssignmentResponse);

            //act
            var result = await _AssignmentController.Update(AssignmentId, UpdateAssignment);

            //assert
            var statusCodeResult = (IStatusCodeActionResult)result;
            Assert.Equal(200, statusCodeResult.StatusCode);
        }

        [Fact]
        public async void Update_ShouldReturnStatusCode500_WhenTryingToUpdateAssignmentWhichDoesNotExists()
        {
            UpdateAssignment UpdateAssignment = new()
            {
                assignment_Description = "Assignment 1 for ClassA",
                assignment_Deadline = DateTime.Now
            };
            int AssignmentId = 1;
            _mockAssignmentService
                .Setup(x => x.Update(It.IsAny<int>(), It.IsAny<UpdateAssignment>()))
                .ReturnsAsync(() => null);

            //act
            var result = await _AssignmentController.Update(AssignmentId, UpdateAssignment);

            //assert
            var statusCodeResult = (IStatusCodeActionResult)result;
            Assert.Equal(500, statusCodeResult.StatusCode);

        }

        [Fact]
        public async void Update_ShouldReturnStatusCode404_WhenExceptionIsRaised()
        {
            UpdateAssignment UpdateAssignment = new()
            {
                assignment_Description = "Assignment 1 for ClassA",
                assignment_Deadline = DateTime.Now
            };

            int AssignmentId = 1;
            _mockAssignmentService
                .Setup(x => x.Update(It.IsAny<int>(), It.IsAny<UpdateAssignment>()))
                .ReturnsAsync(() => throw new System.Exception("This is an exception"));
            //act
            var result = await _AssignmentController.Update(AssignmentId, UpdateAssignment);

            //assert
            var statusCodeResult = (IStatusCodeActionResult)result;
            Assert.Equal(500, statusCodeResult.StatusCode);
        }

        [Fact]
        public async void Delete_ShouldReturnStatusCode204_WhenAssignmentIsDeleted()
        {
            int AssignmentId = 1;

            _mockAssignmentService
                .Setup(s => s.Delete(It.IsAny<int>()))
                .ReturnsAsync(true);

            // Act
            var result = await _AssignmentController.Delete(AssignmentId);

            // Assert 
            var statusCodeResult = (IStatusCodeActionResult)result;
            Assert.Equal(204, statusCodeResult.StatusCode);

        }

        [Fact]
        public async void Delete_ShouldReturnStatusCode500_WhenExceptionIsRaised()
        {
            int AssignmentId = 1;
            _mockAssignmentService
                .Setup(x => x.Delete(It.IsAny<int>()))
                .ReturnsAsync(() => throw new System.Exception("this is an exception"));

            //act
            var result = await _AssignmentController.Delete(AssignmentId);

            //assert
            var statusCodeResult = (IStatusCodeActionResult)result;
            Assert.Equal(500, statusCodeResult.StatusCode);
        }
    }
}

