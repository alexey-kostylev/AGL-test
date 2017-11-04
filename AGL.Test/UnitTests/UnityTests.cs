using System;
using Microsoft.VisualStudio.TestTools.UnitTesting;
using Unity;
using AGL.App.Unity;
using FluentAssertions;
using System.Configuration;
using RestSharp;

namespace AGL.Test.UnitTests
{
    [TestClass]
    public class UnityTests
    {
        private IUnityContainer _container = new UnityContainer();

        [TestInitialize]
        public void TestInit()
        {
            _container.WireupAglDependencies();
        }

        [TestMethod]
        public void ShouldBeAbleToCreateAllItems()
        {        

            foreach (var registration in _container.Registrations)
            {
                var resolved = _container.Resolve(registration.RegisteredType, registration.Name);
                resolved.Should().NotBeNull();
            }
        }

        [TestMethod]
        public void AdapterShouldBeResolvedWithUrl()
        {
            var client = _container.Resolve<IRestClient>();
            client.BaseUrl.Should().Be("http://agl-developer-test.azurewebsites.net/people.json");
        }
    }
}
