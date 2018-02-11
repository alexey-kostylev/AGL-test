using System;
using Microsoft.VisualStudio.TestTools.UnitTesting;
using AGL.App.Logic;
using Moq;
using AGL.App.Models;
using System.Collections.Generic;
using FluentAssertions;
using System.Linq;
using System.Threading.Tasks;
using Ploeh.AutoFixture;
using AGL.App;

namespace AGL.Test.UnitTests.App
{
    [TestClass]
    public class PetsLogicTest : TestBase
    {
        private PetsLogic _logic;
        private readonly Mock<IPeopleAdapter> _mockAdapter = new Mock<IPeopleAdapter>();
        
        [TestInitialize]
        public void TestInit()
        {
            _logic = new PetsLogic(_mockAdapter.Object);
        } 

        [TestMethod]
        public void ShouldThrowExceptionIfAdapterIsNull()
        {
            Action act = () => new PetsLogic(null);
            act.ShouldThrow<ArgumentNullException>("null adapter passed");
        }

        [TestMethod]        
        public async Task ShouldGetDataInRightOrder()
        {
            var persons = new[] {
                new Person
                {
                    Name = "name 1",
                    Gender = Gender.Male,
                    Pets = new List<Pet>(new[]
                    {
                        new Pet("b-cat-m", PetType.Cat),
                        new Pet("dog1", PetType.Dog),
                        new Pet("a-cat-m", PetType.Cat),
                    })
                },
                new Person
                {
                    Name = "name 2",
                    Gender = Gender.Male,
                    Pets = new List<Pet>(new[]
                    {
                        new Pet("fish", PetType.Fish),
                        new Pet("c-cat-m", PetType.Cat)
                    })
                },
                new Person
                {
                    Name = "name 3",
                    Gender = Gender.Female,
                    Pets = new List<Pet>(new[]
                    {
                        new Pet("c-cat-f", PetType.Cat)
                    })
                }
            };

            _mockAdapter.Setup(x => x.GetPetOwners())
                .ReturnsAsync(persons);

            var data = await _logic.GetAllCats();
            _mockAdapter.Verify(x => x.GetPetOwners(), Times.Once);

            data.Should().NotBeNull();
            data.Should().HaveCount(2);
            var males = data.Single(x => x.Gender == Gender.Male);
            males.PetNames.Should().BeEquivalentTo(new[] {"a-cat-m","b-cat-m", "c-cat-m" });
            males.PetNames.Should().BeInAscendingOrder();

            var female = data.Single(x => x.Gender == Gender.Female);
            female.Should().NotBeNull();
            female.PetNames.Should().BeInAscendingOrder();
            female.PetNames.Should().BeEquivalentTo(new[] {"c-cat-f"});
        }

        [TestMethod]
        public async Task ShouldTolerateNullData()
        {
            _mockAdapter.Setup(x => x.GetPetOwners())
                .ReturnsAsync((ICollection<Person>)null);

            var data = await _logic.GetAllCats();

            data.Should().NotBeNull();
            data.Should().BeEmpty();
        }

        [TestMethod]
        public async Task ShouldTolerateEmptyData()
        {
            _mockAdapter.Setup(x => x.GetPetOwners())
                .ReturnsAsync(new List<Person>());

            var data = await _logic.GetAllCats();

            data.Should().NotBeNull();
            data.Should().BeEmpty();
        }

        [TestMethod]
        public async Task ShouldTolerateNullPets()
        {
            var persons = new[] {
                new Person
                {
                    Name = "name 1",
                    Gender = Gender.Male,
                    Pets = new List<Pet>(new[]
                    {
                        new Pet("b-cat-m", PetType.Cat),
                        new Pet("dog1", PetType.Dog),
                        new Pet("a-cat-m", PetType.Cat),
                    })
                },
                new Person
                {
                    Name = "name 2",
                    Gender = Gender.Female,
                    Pets = null
                }
            };
            _mockAdapter.Setup(x => x.GetPetOwners())
                .ReturnsAsync(persons);

            var data = await _logic.GetAllCats();

            data.Should().NotBeNull();
            data.Should().HaveCount(1);
            data.Single().Gender.Should().Be(Gender.Male);
            data.Single().PetNames.Should().HaveCount(2);
        }

        [TestMethod]
        public async Task ShouldTolerateNullPersons()
        {
            var persons = new[] {
                new Person
                {
                    Name = "name 1",
                    Gender = Gender.Male,
                    Pets = new List<Pet>(new[]
                    {
                        new Pet("b-cat-m", PetType.Cat),
                        new Pet("dog1", PetType.Dog),
                        new Pet("a-cat-m", PetType.Cat),
                    })
                },
                null
            };
            _mockAdapter.Setup(x => x.GetPetOwners())
                .ReturnsAsync(persons);

            var data = await _logic.GetAllCats();

            data.Should().NotBeNull();
            data.Should().HaveCount(1);
            data.Single().Gender.Should().Be(Gender.Male);
            data.Single().PetNames.Should().HaveCount(2);
        }

        [TestMethod]
        public async Task NoCatsShouldReturnEmptyList()
        {
            var persons = new[] {
                new Person
                {
                    Name = "name 1",
                    Gender = Gender.Male,
                    Pets = new List<Pet>(new[]
                    {
                        new Pet("fish", PetType.Fish),
                        new Pet("dog1", PetType.Dog),
                        new Pet("dog2", PetType.Dog),
                    })
                },
                new Person
                {
                    Name = "name 2",
                    Gender = Gender.Female,
                    Pets = null
                }
            };
            _mockAdapter.Setup(x => x.GetPetOwners())
                .ReturnsAsync(persons);

            var data = await _logic.GetAllCats();

            data.Should().NotBeNull();
            data.Should().BeEmpty();
        }

        [TestMethod]
        public async Task SameCatNameBelongsToDifferentGendersShouldReturnOneENtryForEachGender()
        {
            var persons = new[] {
                new Person
                {
                    Name = "name 1",
                    Gender = Gender.Male,
                    Pets = new List<Pet>(new[]
                    {
                        new Pet("cat", PetType.Cat)
                    })
                },
                new Person
                {
                    Name = "name 2",
                    Gender = Gender.Female,
                    Pets = new List<Pet>(new[]
                    {
                        new Pet("cat", PetType.Cat)
                    })
                }
            };
            _mockAdapter.Setup(x => x.GetPetOwners())
                .ReturnsAsync(persons);

            var data = await _logic.GetAllCats();

            data.Should().HaveCount(2);
            data.First().PetNames.Should().BeEquivalentTo("cat");
            data.Last().PetNames.Should().BeEquivalentTo("cat");
        }

        [TestMethod]
        public async Task SameCatNameBelongsToSameGendersShouldReturnTwoEntriesForSameGender()
        {
            var persons = new[] {
                new Person
                {
                    Name = "name 1",
                    Gender = Gender.Male,
                    Pets = new List<Pet>(new[]
                    {
                        new Pet("cat", PetType.Cat)
                    })
                },
                new Person
                {
                    Name = "name 2",
                    Gender = Gender.Male,
                    Pets = new List<Pet>(new[]
                    {
                        new Pet("cat", PetType.Cat)
                    })
                }
            };
            _mockAdapter.Setup(x => x.GetPetOwners())
                .ReturnsAsync(persons);

            var data = await _logic.GetAllCats();

            data.Should().HaveCount(1);
            data.Single().PetNames.Should().BeEquivalentTo("cat", "cat");
        }        
    }
}
