using System;
using Microsoft.VisualStudio.TestTools.UnitTesting;
using Moq;
using RestSharp;
using AGL.App.Adapters;
using Ploeh.AutoFixture;
using FluentAssertions;
using System.Collections.Generic;
using AGL.App.Models;
using System.Threading.Tasks;
using System.Linq;

namespace AGL.Test.UnitTests.Adapters
{
    [TestClass]
    public class AdapterTests: TestBase
    {
        private Mock<IRestClient> _mockRestClient = new Mock<IRestClient>();
        private AzurePeopleAdapter _adapter;

        [TestInitialize] 
        public void Setup()
        {
            _adapter = new AzurePeopleAdapter(_mockRestClient.Object);
        }

        [TestMethod]
        public void NullRestClientSHouldThrowException()
        {
            Action act = () => new AzurePeopleAdapter(null);

            act.ShouldThrow<ArgumentNullException>().Where(x => x.Message.Contains("restClient"));
        }        

        [TestMethod]
        public async Task ShouldReturnData()
        {
            var requestData = Fixture.CreateMany<Person>().ToList();

            IRestResponse<List<Person>> response = new RestResponse<List<Person>>
            {                
                StatusCode = System.Net.HttpStatusCode.OK,
                ResponseStatus = ResponseStatus.Completed,
                Data = requestData
            };

            _mockRestClient.Setup(x => x.ExecuteGetTaskAsync<List<Person>>(It.IsNotNull<IRestRequest>()))
                .ReturnsAsync(response);

            var data = await _adapter.GetPetOwners();
            data.Should().NotBeNull();
            data.Should().BeEquivalentTo(requestData);
        }

        [TestMethod]
        public void BadStatusCodeShouldThrowException()
        {            
            IRestResponse<List<Person>> response = new RestResponse<List<Person>>
            {
                StatusCode = System.Net.HttpStatusCode.BadRequest,                
                ResponseStatus = ResponseStatus.Completed,
                Data = Fixture.CreateMany<Person>().ToList()
            };

            _mockRestClient.Setup(x => x.ExecuteGetTaskAsync<List<Person>>(It.IsAny<IRestRequest>()))
                .ReturnsAsync(response);

            Func<Task> act = async() => await _adapter.GetPetOwners();
            act.ShouldThrow<InvalidOperationException>().WithMessage($"*{System.Net.HttpStatusCode.BadRequest}*");
        }

        [TestMethod]
        public void BadResponseStatusShouldThrowException()
        {
            IRestResponse<List<Person>> response = new RestResponse<List<Person>>
            {
                StatusCode = System.Net.HttpStatusCode.OK,                
                ResponseStatus = ResponseStatus.Error,
                ErrorMessage = "error-test",
            };

            _mockRestClient.Setup(x => x.ExecuteGetTaskAsync<List<Person>>(It.IsAny<IRestRequest>()))
                .ReturnsAsync(response);

            Func<Task> act = async () => await _adapter.GetPetOwners();
            act.ShouldThrow<InvalidOperationException>().WithMessage("*error-test*");
        }
    }
}
