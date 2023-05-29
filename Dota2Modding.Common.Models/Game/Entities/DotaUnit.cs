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
            get => base["vscripts"].ToString(CultureInfo.CurrentCulture);
            set => base["vscripts"] = value;
        }

        public bool ConsideredHero
        {
            get => base["ConsideredHero"].ToBoolean(CultureInfo.CurrentCulture);
            set => base["ConsideredHero"] = value ? 1 : 0;
        }

        public bool IsAncient
        {
            get => base["IsAncient"].ToBoolean(CultureInfo.CurrentCulture);
            set => base["IsAncient"] = value ? 1 : 0;
        }

        public bool IsBossCreature
        {
            get => base["IsBossCreature"].ToBoolean(CultureInfo.CurrentCulture);
            set => base["IsBossCreature"] = value ? 1 : 0;
        }

        public string IdleExpression
        {
            get => base["IdleExpression"].ToString(CultureInfo.CurrentCulture);
            set => base["IdleExpression"] = value;
        }

        public string MinimapIcon
        {
            get => base["MinimapIcon"].ToString(CultureInfo.CurrentCulture);
            set => base["MinimapIcon"] = value;
        }
        public double MinimapIconSize
        {
            get => base["MinimapIconSize"].ToDouble(CultureInfo.CurrentCulture);
            set => base["MinimapIconSize"] = value.ToString();
        }
    }
}
