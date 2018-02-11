using System;
using Microsoft.VisualStudio.TestTools.UnitTesting;
using Unity;
using AGL.App.Unity;
using System.Threading.Tasks;
using FluentAssertions;
using AGL.App;

namespace AGL.Test.Integration
{
    [TestClass]
    public class endToEndTests
    {
        private IUnityContainer _container = new UnityContainer();
        [TestInitialize]
        public void TestInit()
        {
            _container.WireupAglDependencies();
        }

        [TestMethod]
        public async Task ShouldGetDataFromAzureAndMakeView()
        {
            var logic = _container.Resolve<IPetsController>();            
            var view = await logic.GetCatsByGender();
            view.Should().NotBeNullOrEmpty();

            view.Should().Contain("Male");
            view.Should().Contain("Female");
            view.Should().Contain("Garfield");
        }
    }
}
