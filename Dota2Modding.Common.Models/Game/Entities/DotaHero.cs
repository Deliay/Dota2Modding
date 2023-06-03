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

        public string HeroName
        {
            get => GetString("Name");
            set => base["Name"] = value;
        }

        public string HeroKey
        {
            get => base.Name;
        }

        public int HeroID
        {
            get => GetInt32("HeroID") ?? 0;
            set => base["HeroID"] = value;
        }

        public string OverrideHero
        {
            get => GetString("override_hero")!;
            set => base["override_hero"] = value;
        }
    }
}
