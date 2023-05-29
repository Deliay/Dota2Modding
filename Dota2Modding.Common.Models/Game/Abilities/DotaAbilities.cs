using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using ValveKeyValue;

namespace Dota2Modding.Common.Models.Game.Abilities
{
    public class DotaAbilities : KvSingleTypeCollecion<DotaAbility>
    {
        public DotaAbilities() : base("DOTAAbilities")
        {
        }

        public DotaAbilities(KVValue value) : base("DOTAAbilities", value)
        {
        }

        protected override DotaAbility Convert(string key, KVValue value)
        {
            return new DotaAbility(key, value);
        }
    }
}
