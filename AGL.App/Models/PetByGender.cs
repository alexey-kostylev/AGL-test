using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace AGL.App.Models
{
    public class PetByGender
    {
        public Gender Gender { get; set; }
        public ICollection<string> PetNames { get; set; }
    }
}
