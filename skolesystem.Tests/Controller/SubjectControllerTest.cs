using Microsoft.AspNetCore.Mvc.Infrastructure;
using Moq;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using skolesystem.Controllers;
using skolesystem.Service.SubjectService;
using skolesystem.DTOs.Subject.Response;
using skolesystem.DTOs.Subject.Request;

namespace skolesystem.test.Controller
{
    public class SubjectControllerTest
	{
        private readonly SubjectsController _subjectController;
        private readonly Mock<ISubjectService> _mocksubjectService = new();

        public SubjectControllerTest()
        {
            _subjectController = new(_mocksubjectService.Object);
        }

        [Fact]
        public async void GetAll_ShouldReturnStatusCode200_WhenSubjectsExists()
        {

            //arrange
            List<SubjectResponse> subjects = new();

            subjects.Add(new()
            {
                Id = 1,
                subjectname = "Math"
            });

            subjects.Add(new()
            {
                Id = 2,
                subjectname = "Science"
            });

            _mocksubjectService
                .Setup(x => x.GetAll()).ReturnsAsync(subjects);
            //act
            var result = await _subjectController.GetAll();
            //assert
            var statusCodeResult = (IStatusCodeActionResult)result;
            Assert.Equal(200, statusCodeResult.StatusCode);
        }

        [Fact]
        public async void GetAll_ShouldReturnStatusCode204_WhenNoSubjectsExists()
        {

            //arrange
            List<SubjectResponse> subjects = new();

            _mocksubjectService.Setup(x => x.GetAll()).ReturnsAsync(subjects);
            //act
            var result = await _subjectController.GetAll();
            //assert
            var statusCodeResult = (IStatusCodeActionResult)result;
            Assert.Equal(204, statusCodeResult.StatusCode);
        }

        [Fact]
        public async void GetAll_ShouldReturnStatusCode500_WhenNullIsReturnedFromService()
        {

            //arrange
            _mocksubjectService.Setup(x => x.GetAll()).ReturnsAsync(() => null);
            //act
            var result = await _subjectController.GetAll();
            //assert
            var statusCodeResult = (IStatusCodeActionResult)result;
            Assert.Equal(500, statusCodeResult.StatusCode);
        }

        [Fact]
        public async void GetAll_ShouldReturnStatusCode500_WhenExceptionIsRaised()
        {

            //arrange
            _mocksubjectService.Setup(x => x.GetAll()).ReturnsAsync(() => throw new Exception("This is an exception"));
            //act
            var result = await _subjectController.GetAll();
            //assert
            var statusCodeResult = (IStatusCodeActionResult)result;
            Assert.Equal(500, statusCodeResult.StatusCode);
        }

        [Fact]
        public async void GetById_ShouldReturnStatusCode200_WhenDataExists()
        {
            int subjectId = 1;

            SubjectResponse category = new()
            {
                Id = subjectId,
                subjectname = "Math"
            };

            _mocksubjectService
                .Setup(x => x.GetById(It.IsAny<int>()))
                .ReturnsAsync(category);
            //act
            var result = await _subjectController.GetById(subjectId);

            //assert
            var statusCodeResult = (IStatusCodeActionResult)result;
            Assert.Equal(200, statusCodeResult.StatusCode);
        }

        [Fact]
        public async void GetById_ShouldReturnStatusCode500_WhenSubjectDoesNotExists()
        {
            int subjectId = 1;
            _mocksubjectService
                .Setup(x => x.GetById(It.IsAny<int>()))
                .ReturnsAsync(() => null);

            //act
            var result = await _subjectController.GetById(subjectId);

            //assert
            var statusCodeResult = (IStatusCodeActionResult)result;
            Assert.Equal(500, statusCodeResult.StatusCode);

        }

        [Fact]
        public async void GetById_ShouldReturnStatusCode500_WhenExceptionIsRaised()
        {
            _mocksubjectService
                .Setup(x => x.GetById(It.IsAny<int>()))
                .ReturnsAsync(() => throw new System.Exception("This is an Exception"));

            //act
            var result = await _subjectController.GetById(1);

            //assert
            var statusCodeResult = (IStatusCodeActionResult)result;
            Assert.Equal(500, statusCodeResult.StatusCode);
        }

        [Fact]
        public async void Create_ShouldReturnStatusCode200_WhenSubjectIsSuccessfullyCreated()
        {
            NewSubject newSubject = new()
            {
                subjectname = "Math"
            };
            int subjectId = 1;

            SubjectResponse SubjectResponse = new()
            {
                Id = subjectId,
                subjectname = "Math"
            };

            _mocksubjectService
                .Setup(x => x.Create(It.IsAny<NewSubject>()))
                .ReturnsAsync(SubjectResponse);

            //act
            var result = await _subjectController.Create(newSubject);

            //assert
            var statusCodeResult = (IStatusCodeActionResult)result;
            Assert.Equal(200, statusCodeResult.StatusCode);
        }
        [Fact]
        public async void Create_ShouldReturnStatusCode500_WhenExceptionIsRaised()
        {
            NewSubject NewSubject = new()
            {
                subjectname = "Math"
            };
            _mocksubjectService
                .Setup(x => x.Create(It.IsAny<NewSubject>()))
                .ReturnsAsync(() => throw new System.Exception("this is an exception"));

            //act
            var result = await _subjectController.Create(NewSubject);

            //assert
            var statusCodeResult = (IStatusCodeActionResult)result;
            Assert.Equal(500, statusCodeResult.StatusCode);
        }

        [Fact]
        public async void Update_ShouldReturnStatusCode200_WhenSubjectIsSuccessfullyUpdated()
        {
            UpdateSubject updateSubject = new()
            {
                subjectname = "Math"
            };
            int subjectId = 1;

            SubjectResponse SubjectResponse = new()
            {
                Id = subjectId,
                subjectname = "Math"
            };
            _mocksubjectService
                .Setup(x => x.Update(It.IsAny<int>(), It.IsAny<UpdateSubject>()))
                .ReturnsAsync(SubjectResponse);

            //act
            var result = await _subjectController.Update(subjectId, updateSubject);

            //assert
            var statusCodeResult = (IStatusCodeActionResult)result;
            Assert.Equal(200, statusCodeResult.StatusCode);
        }

        [Fact]
        public async void Update_ShouldReturnStatusCode500_WhenTryingToUpdateSubjectWhichDoesNotExists()
        {
            UpdateSubject UpdateSubject = new()
            {
                subjectname = "Math"
            };
            int subjectId = 1;
            _mocksubjectService
                .Setup(x => x.Update(It.IsAny<int>(), It.IsAny<UpdateSubject>()))
                .ReturnsAsync(() => null);

            //act
            var result = await _subjectController.Update(subjectId, UpdateSubject);

            //assert
            var statusCodeResult = (IStatusCodeActionResult)result;
            Assert.Equal(500, statusCodeResult.StatusCode);

        }

        [Fact]
        public async void Update_ShouldReturnStatusCode404_WhenExceptionIsRaised()
        {
            UpdateSubject UpdateSubject = new()
            {
                subjectname = "Math"
            };

            int subjectId = 1;
            _mocksubjectService
                .Setup(x => x.Update(It.IsAny<int>(), It.IsAny<UpdateSubject>()))
                .ReturnsAsync(() => throw new System.Exception("This is an exception"));
            //act
            var result = await _subjectController.Update(subjectId, UpdateSubject);

            //assert
            var statusCodeResult = (IStatusCodeActionResult)result;
            Assert.Equal(500, statusCodeResult.StatusCode);
        }

        [Fact]
        public async void Delete_ShouldReturnStatusCode204_WhenSubjectIsDeleted()
        {
            int subjectId = 1;

            _mocksubjectService
                .Setup(s => s.Delete(It.IsAny<int>()))
                .ReturnsAsync(true);

            // Act
            var result = await _subjectController.Delete(subjectId);

            // Assert 
            var statusCodeResult = (IStatusCodeActionResult)result;
            Assert.Equal(204, statusCodeResult.StatusCode);

        }

        [Fact]
        public async void Delete_ShouldReturnStatusCode500_WhenExceptionIsRaised()
        {
            int subjectId = 1;
            _mocksubjectService
                .Setup(x => x.Delete(It.IsAny<int>()))
                .ReturnsAsync(() => throw new System.Exception("this is an exception"));

            //act
            var result = await _subjectController.Delete(subjectId);

            //assert
            var statusCodeResult = (IStatusCodeActionResult)result;
            Assert.Equal(500, statusCodeResult.StatusCode);
        }
    }
}

