using AGL.App.Models;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace AGL.App.Logic
{
    public interface IPeopleLogic
    {
        Task<ICollection<Person>> GetPersons();

        Task<ICollection<PetByGender>> GetAllCats();
    }
}
