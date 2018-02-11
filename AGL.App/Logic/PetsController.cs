using AGL.App.Models;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace AGL.App.Logic
{

    /// <summary>
    /// Generates text view from data
    /// </summary>
    public class PetsController : IPetsController
    {
        private IPetsLogic _logic;

        public PetsController(IPetsLogic logic)
        {
            if (logic == null)
                throw new ArgumentNullException(nameof(logic));

            _logic = logic;
        }

        /// <summary>
        /// Calls logic to calculate data and generates view in a text format
        /// </summary>
        /// <returns></returns>

        public async Task<string> GetCatsByGender()
        {
            var data = await _logic.GetAllCats();

            if (data == null)
            {
                throw new InvalidOperationException($"{nameof(_logic.GetAllCats)} returned null");
            }            

            if (!data.Any())
            {
                return string.Empty;
            }

            var sb = new StringBuilder();
            foreach (var rec in data)
            {
                if (rec.PetNames == null || !rec.PetNames.Any())
                {
                    continue;
                }
                sb.AppendLine($"{rec.Gender}");
                sb.AppendLine($"\t-{string.Join("\n\t-", rec.PetNames.ToArray())}");
            }

            return sb.ToString();
        }
    }
}
