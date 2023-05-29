using System;
using System.Buffers.Text;
using System.Collections.Generic;
using System.Globalization;
using System.Linq;
using System.Runtime.Serialization;
using System.Text;
using System.Threading.Tasks;
using ValveKeyValue;

namespace Dota2Modding.Common.Models.Addon
{
    public class AddonInfo : BasicObject
    {
        public AddonInfo(KVValue value) : base("AddonInfo", value)
        {
        }
        public AddonInfo(string name, KVValue value) : base(name, value)
        {
        }


        public string Maps
        {
            get => base["maps"].ToString(CultureInfo.CurrentCulture);
            set => base["maps"] = value;
        }
        public bool MinimalPrecache
        {
            get => base["MinimalPrecache"].ToBoolean(CultureInfo.CurrentCulture);
            set => base["MinimalPrecache"] = value ? 1 : 0;
        }
        public bool IsPlayable
        {
            get => base["IsPlayable"].ToBoolean(CultureInfo.CurrentCulture);
            set => base["IsPlayable"] = value ? 1 : 0;
        }
        public bool EventGame
        {
            get => base["EventGame"].ToBoolean(CultureInfo.CurrentCulture);
            set => base["EventGame"] = value ? 1 : 0;
        }
    }
}
