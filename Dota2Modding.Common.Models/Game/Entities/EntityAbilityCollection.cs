using Dota2Modding.Common.Models.Game.Entities;
using System.Collections;
using System.Globalization;
using System.Text.RegularExpressions;
using ValveKeyValue;

namespace Dota2Modding.Common.Models.Game.Heroes
{
    public partial class EntityAbilityCollection : IReadOnlyList<string>
    {
        private readonly DotaEntity unit;

        [GeneratedRegex("Ability\\d+")]
        private static partial Regex MyRegex();

        private readonly Regex abilityRegex = MyRegex();

        internal EntityAbilityCollection(DotaEntity unit)
        {
            this.unit = unit;
        }

        public string this[int index]
        {
            get
            {
                if (unit[$"Ability{index}"] is KVValue value)
                {
                    return value.ToString(CultureInfo.CurrentCulture);
                }
                return null!;
            }
            set
            {
                unit[$"Ability{index}"] = value;
            }
        }

        private IEnumerable<KVObject> Search() => unit.Where(obj => abilityRegex.IsMatch(obj.Name));

        public int Count => Search().Count();


        public IEnumerator<string> GetEnumerator() => Search().Select(obj => obj.Name).GetEnumerator();


        IEnumerator IEnumerable.GetEnumerator() => GetEnumerator();
    }
}
