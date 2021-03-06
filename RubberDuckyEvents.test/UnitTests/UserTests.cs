using System;
using Xunit;
using RubberDuckyEvents.API.Controllers;
using Moq;
using Microsoft.Extensions.Logging;
using RubberDuckyEvents.API.Ports;
using RubberDuckyEvents.API.Domain;
using System.Threading.Tasks;
using Microsoft.AspNetCore.Mvc;

namespace RubberDuckyEvents.Test.UnitTests
{
    public class UserControllerUnitTest
    {
        private Mock<ILogger<UserController>> _mockedLogger = new Mock<ILogger<UserController>>();
        private Mock<IDatabase> _mockedDatabase = new Mock<IDatabase>();

        public UserControllerUnitTest()
        {
            _mockedDatabase.Reset();
            _mockedLogger.Reset();
        }

        [Fact]
        public async Task TestGetUserById_Success()
        {
            var testId = 1;

            //Create mock up user
            var testUser = new User { Id = testId, Name = "Bailey", 
            DateOfBirth = Convert.ToDateTime("1998-11-02 11:38:56.307"), 
            Mail = "bailey@lievens.be", EventId = 1};


            //Call the method GetUserById to test our mocked user, no db calls
            _mockedDatabase.Setup(x => x.GetUserById(testId)).Returns(Task.FromResult(testUser));

            //Link controller
            var controller = new UserController(_mockedLogger.Object, _mockedDatabase.Object);
            //Get results from the controller
            var actualResult = await controller.GetUserById(testId) as OkObjectResult;

            //Check results
            Assert.Equal(200, actualResult.StatusCode);
            var viewModel = actualResult.Value as ViewUser;
            Assert.Equal(testUser.Id, viewModel.Id);
            Assert.Equal(testUser.Name, viewModel.Name);
            Assert.Equal(testUser.DateOfBirth, viewModel.DateOfBirth);
            Assert.Equal(testUser.Mail, viewModel.Mail);
            Assert.Equal(testUser.EventId, viewModel.EventId);

            _mockedLogger.VerifyAll();
            _mockedDatabase.VerifyAll();
        }

        [Fact]
        public async Task TestGetUserById_DoesntExist()
        {
            var rnd = new Random();
            var testId = rnd.Next(101);

            //Create mock up user
            var testUser = new User { Id = testId, Name = "Bailey", 
            DateOfBirth = Convert.ToDateTime("1998-11-02 11:38:56.307"), 
            Mail = "bailey@lievens.be", EventId = 1};

            _mockedDatabase.Setup(x => x.GetUserById(testId)).Returns(Task.FromResult(null as User)); //Doesn't return the found event but returns null

            //Link the EventController
            var controller = new UserController(_mockedLogger.Object, _mockedDatabase.Object);

            //Check the results
            var result = await new UserController(_mockedLogger.Object, _mockedDatabase.Object).GetUserById(testId);
            Assert.IsType<NotFoundResult>(result);

            _mockedLogger.VerifyAll();
            _mockedDatabase.VerifyAll();
        }

        [Fact]
        public async Task TestGetUserById_ErrorOnRetrievalAsync()
        {
            var rnd = new Random();
            var testId = rnd.Next(101);

            //Create mock up user
            var testUser = new User { Id = testId, Name = "Bailey", 
            DateOfBirth = Convert.ToDateTime("1998-11-02 11:38:56.307"), 
            Mail = "bailey@lievens.be", EventId = 1};

            _mockedDatabase.Setup(x => x.GetUserById(testId)).ThrowsAsync(new Exception("Cowboy Bebop"));

            //Link the EventController
            var result = await new UserController(_mockedLogger.Object, _mockedDatabase.Object).GetUserById(testId);

            //Check the results
            Assert.IsType<BadRequestObjectResult>(result);
            _mockedLogger.VerifyAll();
            _mockedDatabase.VerifyAll();
        }


    }
}