using AGL.App.Models;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace AGL.App
{
    /// <summary>
    /// Http client to read data
    /// </summary>
    public interface IPeopleAdapter
    {
        /// <summary>
        /// Reads all data from http service
        /// </summary>
        /// <returns></returns>
        Task<ICollection<Person>> GetPetOwners();
    }
}
