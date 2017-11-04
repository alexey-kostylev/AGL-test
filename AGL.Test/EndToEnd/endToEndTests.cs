using System;
using Microsoft.VisualStudio.TestTools.UnitTesting;
using Unity;
using AGL.App.Unity;
using AGL.App.Controllers;
using System.Threading.Tasks;
using FluentAssertions;

namespace AGL.Test.EndToEnd
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
            var controller = _container.Resolve<PeopleControler>();
            var view = await controller.GetCatsWithOwnersGender();
            view.Should().NotBeNullOrEmpty();
        }
    }
}
