using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using AGL.App.Models;

namespace AGL.App.Logic
{
    public class PetsLogic : IPetsLogic
    {
        private readonly IPeopleAdapter _peopleAdapter;

        public PetsLogic(IPeopleAdapter peopleAdapter)
        {
            if (peopleAdapter == null)
            {
                throw new ArgumentNullException(nameof(peopleAdapter));
            }

            _peopleAdapter = peopleAdapter;
        }        


        /// <summary>
        /// Gets all cats grouped by owner's gender
        /// </summary>
        /// <returns></returns>
        public async Task<ICollection<PetByGender>> GetAllCats()
        {
            var data = await _peopleAdapter.GetPetOwners();
            if (data == null || !data.Any())
            {
                return new PetByGender[0];
            }

            var result = data
                .Where(person => person != null && person.Pets != null && person.Pets.Any())
                .SelectMany(person => person.Pets
                    .Where(pet => pet.Type == PetType.Cat)
                    .Select(pet => new { gender = person.Gender, petName = pet.Name })
                )
                .GroupBy(g => g.gender)
                .Select(genderGroup => new PetByGender
                {
                    Gender = genderGroup.Key,
                    PetNames = genderGroup.Select(groupItem => groupItem.petName).OrderBy(ord => ord).ToArray()
                })
                .ToArray();

            return result;
        }        
    }
}
