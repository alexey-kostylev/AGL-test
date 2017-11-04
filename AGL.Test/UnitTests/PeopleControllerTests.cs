using System;
using Microsoft.VisualStudio.TestTools.UnitTesting;
using AGL.App.Controllers;
using Moq;
using AGL.App.Logic;
using FluentAssertions;
using System.Threading.Tasks;
using Ploeh.AutoFixture;
using AGL.App.Models;
using System.Linq;
using System.Collections.Generic;

namespace AGL.Test.UnitTests
{
    [TestClass]
    public class PeopleControllerTests : UnitTestBase
    {
        private PeopleControler _controller;
        private Mock<IPeopleLogic> _mockLogic = new Mock<IPeopleLogic>();
        
        [TestInitialize]
        public void TEstInit()
        {
            _controller = new PeopleControler(_mockLogic.Object);
        } 

        [TestMethod]
        public void ShouldFailIfLogicIsNull()
        {
            Action act = () => new PeopleControler(null);
            act.ShouldThrow<ArgumentNullException>("logic passed is null");
        }

        [TestMethod]
        public async Task ShouldFormatData()
        {
            var people = Fixture.CreateMany<PetByGender>().ToArray();

            _mockLogic.Setup(x => x.GetAllCats())
                .ReturnsAsync(people);

            var view = await _controller.GetCatsWithOwnersGender();

            view.Should().NotBeNullOrEmpty();

            _mockLogic.Verify(x => x.GetAllCats(), Times.Once);

        }

        [TestMethod]
        public void NullDataSHouldThrowException()
        {
            var people = (ICollection<PetByGender>)null;

            _mockLogic.Setup(x => x.GetAllCats())
                .ReturnsAsync(people);

            Func<Task> act = async () => await _controller.GetCatsWithOwnersGender();

            act.ShouldThrow<InvalidOperationException>();

            _mockLogic.Verify(x => x.GetAllCats(), Times.Once);

        }

        [TestMethod]
        public async Task ShouldTolerateEmptyData()
        {
            var people = new List<PetByGender>();

            _mockLogic.Setup(x => x.GetAllCats())
                .ReturnsAsync(people);

            var view = await _controller.GetCatsWithOwnersGender();

            view.Should().BeEmpty();

            _mockLogic.Verify(x => x.GetAllCats(), Times.Once);
        }

        [TestMethod]
        public async Task ShouldTolerateNullPets()
        {
            var person = Fixture.Create<PetByGender>();
            person.PetNames = null;

            _mockLogic.Setup(x => x.GetAllCats())
                .ReturnsAsync(new[] { person });

            var view = await _controller.GetCatsWithOwnersGender();

            view.Should().BeEmpty();

            _mockLogic.Verify(x => x.GetAllCats(), Times.Once);
        }
    }
}
