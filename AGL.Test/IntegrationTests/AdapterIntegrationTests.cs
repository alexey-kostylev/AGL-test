using System;
using Microsoft.VisualStudio.TestTools.UnitTesting;
using AGL.App.Adapters;
using RestSharp;
using System.Configuration;
using System.Threading.Tasks;
using FluentAssertions;
using System.Linq;
using AGL.App.Models;

namespace AGL.Test.IntegrationTests
{
    [TestClass]
    public class AdapterIntegrationTests
    {
        private AzurePeopleAdapter _adapter;

        [TestInitialize]
        public void TestInit()
        {
            _adapter = new AzurePeopleAdapter(new RestClient(ConfigurationManager.AppSettings["peopleUrl"]));
        }

        [TestMethod]
        public async Task ShouldGetPeopleAndModelShouldBeValid()
        {
            var data = await _adapter.GetPersons();
            data.Should().NotBeNullOrEmpty();

            var bob = data.FirstOrDefault(x => x.Name == "Bob");
            ValidateBob(bob);       
        }

        /// <summary>
        /// Bob is a test example to make sure the model is not changed on their end
        /// </summary>
        private void ValidateBob(Person bob)
        {
            bob.Age.Should().Be(23);
            bob.Gender.Should().Be(Gender.Male);
            bob.Pets.Should().HaveCount(2);
            bob.Pets.Should().BeEquivalentTo(new[] {
                new Pet("Garfield", PetType.Cat),
                new Pet("Fido", PetType.Dog),
            });
        }

        [TestMethod]
        public void UrlInvalidShouldThrowException()
        {
            var adapter = new AzurePeopleAdapter(new RestClient("http://test.net/people.json"));
            Func<Task> func = async() => await adapter.GetPersons();

            func.ShouldThrow<InvalidOperationException>();
        }

    }
}
