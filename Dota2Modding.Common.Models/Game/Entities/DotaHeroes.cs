using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using ValveKeyValue;

namespace Dota2Modding.Common.Models.Game.Entities
{
    public class DotaHeroes : KvSingleTypeCollecion<DotaHero>
    {
        public DotaHeroes(KVValue value) : base("DOTAHeroes", value)
        {
        }

        public DotaHeroes() : base("DOTAHeroes")
        {
        }

        protected override DotaHero Convert(string key, KVValue value)
        {
            return new DotaHero(key, value);
        }
    }
}
