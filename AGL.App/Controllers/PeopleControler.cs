using AGL.App.Logic;
using System;
using System.Collections.Generic;
using System.IO;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace AGL.App.Controllers
{
    public class PeopleControler
    {        
        private IPeopleLogic _peopleLogic;

        public PeopleControler(IPeopleLogic peopleLogic)
        {
            if (peopleLogic == null)
            {
                throw new ArgumentNullException(nameof(peopleLogic));
            }
            _peopleLogic = peopleLogic;
        }

        public async Task<string> GetCatsWithOwnersGender()
        {
            var data = await _peopleLogic.GetAllCats();
            if (data == null)
            {
                throw new InvalidOperationException("data is null");
            }

            if (!data.Any())
            {
                return string.Empty;
            }

            var sb = new StringBuilder();
            foreach(var rec in data)
            {
                if (rec.PetNames == null || !rec.PetNames.Any())
                {
                    continue;
                }
                sb.AppendLine($"-{rec.Gender}");
                sb.AppendLine($"\t{string.Join("\n\t", rec.PetNames.ToArray())}");
            }

            return sb.ToString();
        }
    }
}
