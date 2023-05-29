using Dota2Modding.Common.Models.Game.Constants;
using System;
using System.Collections.Generic;
using System.Globalization;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using ValveKeyValue;

namespace Dota2Modding.Common.Models.Game.Entities
{
    public class DotaHero : DotaEntity
    {
        public DotaHero(string name) : base(name)
        {
        }

        public DotaHero(string name, KVValue value) : base(name, value)
        {
        }

        public int HeroID
        {
            get => base["HeroID"].ToInt32(CultureInfo.CurrentCulture);
            set => base["HeroID"] = value;
        }

    }
}
