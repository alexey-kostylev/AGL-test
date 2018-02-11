using AGL.App.Models;
using FluentAssertions;
using Microsoft.VisualStudio.TestTools.UnitTesting;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace AGL.Test.UnitTests.Models
{

    [TestClass]
    public class PeopleTests : TestBase
    {
        [TestMethod]
        public void PetsNotEmptyToStringShouldBeValid()
        {
            var person = new Person
            {
                Name = "name",
                Age = 1,
                Gender = Gender.Female,
                Pets = new List<Pet>(new[] {
                    new Pet("cat-1", PetType.Cat)
                })
            };

            person.ToString().Should().Match("*name*1*Female*pets: 1*");
        }

        [TestMethod]
        public void NoPetsToStringShouldBeValid()
        {
            var person = new Person
            {
                Name = "name",
                Age = 1,
                Gender = Gender.Female,
                Pets = new List<Pet>()
            };

            person.ToString().Should().Match("*name*1*Female*pets: 0*");
        }

        [TestMethod]
        public void PetsIsNullToStringShouldBeValid()
        {
            var person = new Person
            {
                Name = "name",
                Age = 1,
                Gender = Gender.Female,
                Pets = null
            };

            person.ToString().Should().Match("*name*1*Female*pets: 0*");
        }
    }
}
