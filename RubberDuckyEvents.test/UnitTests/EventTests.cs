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
    public class EventControllerUnitTest
    {
        private Mock<ILogger<EventController>> _mockedLogger = new Mock<ILogger<EventController>>();
        private Mock<IDatabase> _mockedDatabase = new Mock<IDatabase>();

        public EventControllerUnitTest()
        {
            _mockedDatabase.Reset();
            _mockedLogger.Reset();
        }

        [Fact]
        public async Task TestGetEventById_Success()
        {
            var rnd = new Random();
            var testId = rnd.Next(101);

            //Create mock up event
            var testEvent = new Event { Id = testId, Name = "EpicBigFestival", 
            Description = "Festival is big epic", MinAge = 2, MaxAge = 5, 
            StartDate = Convert.ToDateTime("2018-11-05 11:38:56.307"), EndDate = Convert.ToDateTime("2019-11-05 11:38:56.307"), 
            StreetName = "Bruul", StreetNumber = 2, City = "London", Country = "England"};


            //Call the method GetEventById to test our mocked event, no db calls
            _mockedDatabase.Setup(x => x.GetEventById(testId)).Returns(Task.FromResult(testEvent));

            //Link controller
            var controller = new EventController(_mockedLogger.Object, _mockedDatabase.Object);
            //Get results from the controller
            var actualResult = await controller.GetEventById(testId) as OkObjectResult;

            //Check results
            Assert.Equal(200, actualResult.StatusCode);
            var viewModel = actualResult.Value as ViewEvent;
            Assert.Equal(testEvent.Id, viewModel.Id);
            Assert.Equal(testEvent.Name, viewModel.Name);
            Assert.Equal(testEvent.Description, viewModel.Description);
            Assert.Equal(testEvent.MinAge, viewModel.MinAge);
            Assert.Equal(testEvent.MaxAge, viewModel.MaxAge);
            Assert.Equal(testEvent.StartDate, viewModel.StartDate);
            Assert.Equal(testEvent.EndDate, viewModel.EndDate);
            Assert.Equal(testEvent.StreetName, viewModel.StreetName);
            Assert.Equal(testEvent.StreetNumber, viewModel.StreetNumber);
            Assert.Equal(testEvent.City, viewModel.City);
            Assert.Equal(testEvent.Country, viewModel.Country);

            _mockedLogger.VerifyAll();
            _mockedDatabase.VerifyAll();
        }

    }
}