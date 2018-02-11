using AGL.App.Models;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace AGL.App
{
    public interface IPetsLogic
    {      

        Task<ICollection<PetByGender>> GetAllCats();        
    }
}
