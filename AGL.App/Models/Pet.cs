using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace AGL.App.Models
{
    public class Pet
    {
        public string Name { get;set;}
        public PetType Type { get; set; }

        public Pet() { }
        public Pet(string name, PetType petType)
        {
            Name = name;
            Type = petType;
        }

        public override bool Equals(object obj)
        {
            if ( !(obj is Pet) ||  obj == null)
            {
                return false;
            }

            var test = obj as Pet;

            return test.Name.Equals(Name) && test.Type.Equals(Type);
        }

        public override int GetHashCode()
        {
            var hash = 35;
            hash = hash * 24 + Name.GetHashCode();
            hash = hash * 24 + Type.GetHashCode();
            return hash;
        }

        public override string ToString()
        {
            return $"'{Name}', {Type}";
        }
    }
}
