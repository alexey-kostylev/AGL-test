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
        /// <summary>
        ///  Gets all cats grouped by owner's gender
        /// </summary>
        /// <returns></returns>
        Task<ICollection<PetByGender>> GetAllCats();        
    }
}
