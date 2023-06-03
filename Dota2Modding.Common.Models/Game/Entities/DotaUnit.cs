using System;
using System.Collections.Generic;
using System.Globalization;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using ValveKeyValue;

namespace Dota2Modding.Common.Models.Game.Entities
{
    public class DotaUnit : DotaEntity
    {
        public DotaUnit(string name) : base(name)
        {
        }

        public DotaUnit(string name, KVValue value) : base(name, value)
        {
        }

        public string VScripts
        {
            get => GetString("vscripts");
            set => base["vscripts"] = value;
        }

        public bool ConsideredHero
        {
            get => GetBoolean("ConsideredHero") ?? false;
            set => base["ConsideredHero"] = value ? 1 : 0;
        }

        public bool IsAncient
        {
            get => GetBoolean("IsAncient") ?? false;
            set => base["IsAncient"] = value ? 1 : 0;
        }

        public bool IsBossCreature
        {
            get => GetBoolean("IsBossCreature") ?? false;
            set => base["IsBossCreature"] = value ? 1 : 0;
        }

        public string IdleExpression
        {
            get => GetString("IdleExpression");
            set => base["IdleExpression"] = value;
        }

        public string MinimapIcon
        {
            get => GetString("MinimapIcon");
            set => base["MinimapIcon"] = value;
        }
        public float MinimapIconSize
        {
            get => GetSingle("MinimapIconSize") ?? 16f;
            set => base["MinimapIconSize"] = value.ToString();
        }
    }
}
