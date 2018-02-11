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

        public async Task<ICollection<PetByGender>> GetAllCats()
        {
            var data = await _peopleAdapter.GetPetOwners();
            if (data == null || !data.Any())
            {
                return new PetByGender[0];
            }

            var result = data
                .Where(person => person != null && person.Pets != null && person.Pets.Any())
                .SelectMany(x => x.Pets
                    .Where(pet => pet.Type == PetType.Cat)
                    .Select(pet => new { gender = x.Gender, petName = pet.Name })
                )
                .GroupBy(g => g.gender)
                .Select(x => new PetByGender
                {
                    Gender = x.Key,
                    PetNames = x.Select(xx => xx.petName).OrderBy(v => v).ToArray()
                })
                .ToArray();

            return result;
        }        
    }
}
