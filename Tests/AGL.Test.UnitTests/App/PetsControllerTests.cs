using AGL.App;
using AGL.App.Logic;
using AGL.App.Models;
using FluentAssertions;
using Microsoft.VisualStudio.TestTools.UnitTesting;
using Moq;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace AGL.Test.UnitTests.App
{
    [TestClass]
    public class PetsControllerTests : TestBase
    {
        private Mock<IPetsLogic> _mockPetsLogic;

        private PetsController _controller;

        [TestInitialize]
        public void TestInit()
        {
            _mockPetsLogic = new Mock<IPetsLogic>();
            _controller = new PetsController(_mockPetsLogic.Object);
        }

        [TestMethod]
        public void NullArgumentShouldThrowException()
        {
            
            Action act = () => new PetsController(null);

            act.ShouldThrow<ArgumentNullException>();
        }

        [TestMethod]
        public async Task ShouldTolerateEmptyData()
        {
            _mockPetsLogic.Setup(x => x.GetAllCats())
                .ReturnsAsync(new List<PetByGender>());

            var view = await _controller.GetCatsByGender();

            view.Should().BeEmpty();
        }

        [TestMethod]
        public async Task ShouldTolerateNullData()
        {
            _mockPetsLogic.Setup(x => x.GetAllCats())
                .ReturnsAsync(new List<PetByGender>());

            var view = await _controller.GetCatsByGender();

            view.Should().BeEmpty();         
        }

        [TestMethod]
        public async Task ShouldGenerateView()
        {
            var data = new List<PetByGender>();

            data.Add(new PetByGender
            {
                Gender = Gender.Male,
                PetNames = new[] { "pet-11", "pet-12" }
            });

            data.Add(new PetByGender
            {
                Gender = Gender.Female,
                PetNames = new[] { "pet-21", "pet-22" }
            });

            _mockPetsLogic.Setup(x => x.GetAllCats())
                .ReturnsAsync(data);

            var view = await _controller.GetCatsByGender();

            //first
            view.Should().Contain("Male");
            view.Should().Contain("pet-11");
            view.Should().Contain("pet-12");

            //second
            view.Should().Contain("Female");
            view.Should().Contain("pet-21");
            view.Should().Contain("pet-22");
        }
    }
}
