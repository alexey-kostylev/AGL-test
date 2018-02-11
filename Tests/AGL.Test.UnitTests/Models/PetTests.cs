using AGL.App.Models;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using FluentAssertions;
using Microsoft.VisualStudio.TestTools.UnitTesting;

namespace AGL.Test.UnitTests
{
    [TestClass]
    public class ModelTests : TestBase
    {
        [TestMethod]
        public void PetIsEqualShouldBeTrue()
        {
            var pet1 = new Pet
            {
                Name = "pet",
                Type = PetType.Cat
            };

            var pet2= new Pet
            {
                Name = "pet",
                Type = PetType.Cat
            };

            pet1.Equals(pet2).Should().BeTrue();
        }

        [TestMethod]
        public void PetIsNotEqualShouldBeTrue()
        {
            var pet1 = new Pet
            {
                Name = "pet1",
                Type = PetType.Cat
            };

            var pet2 = new Pet
            {
                Name = "pet",
                Type = PetType.Cat
            };

            pet1.Equals(pet2).Should().BeFalse();
        }

        [TestMethod]
        public void PetIsNotEqualToNullShouldBeTrue()
        {
            var pet1 = new Pet
            {
                Name = "pet1",
                Type = PetType.Cat
            };
            
            pet1.Equals(null).Should().BeFalse();
        }

        [TestMethod]
        public void ToStrtingShouldBeValid()
        {
            var pet1 = new Pet
            {
                Name = "pet",
                Type = PetType.Cat
            };

            pet1.ToString().Should().Match("*pet*Cat*");
        }

        [TestMethod]
        public void HashCodeShouldBePositive()
        {
            var pet1 = new Pet
            {
                Name = "pet",
                Type = PetType.Cat
            };

            pet1.GetHashCode().Should().BePositive();
        }

        [TestMethod]
        public void SameObjectsShouldHaveSameHashCode()
        {
            var pet1 = new Pet
            {
                Name = "pet",
                Type = PetType.Cat
            };

            var pet2 = new Pet
            {
                Name = "pet",
                Type = PetType.Cat
            };

            pet1.GetHashCode().Should().Be(pet2.GetHashCode());
        }

        [TestMethod]
        public void DiffObjectsShouldHaveDiffHashCode()
        {
            var pet1 = new Pet
            {
                Name = "pet1",
                Type = PetType.Cat
            };

            var pet2 = new Pet
            {
                Name = "pet2",
                Type = PetType.Cat
            };

            pet1.GetHashCode().Should().NotBe(pet2.GetHashCode());
        }
    }
}
