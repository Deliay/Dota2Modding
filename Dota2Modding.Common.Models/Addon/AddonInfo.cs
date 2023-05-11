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
    public struct AddonInfo
    {
        public List<string> Maps { get; set; }
        public bool MinimalPrecache { get; set; }
        public bool IsPlayable { get; set; }
        public bool EventGame { get; set; }

        public struct MapConfig
        {
            public int? MaxPlayers { get; set; }
        }

        public Dictionary<string, MapConfig> MapConfigs { get; set; }

        public static bool TryParse(KVObject kv, out AddonInfo addonInfo)
        {
            addonInfo = new();
            if (kv.Name != "AddonInfo") throw new InvalidDataException($"Invalid kv format: {kv.Name}");

            addonInfo = new AddonInfo()
            {
                Maps = kv["maps"].ToString(CultureInfo.CurrentCulture).Split(' ').ToList(),
                MinimalPrecache = kv["MinimalPrecache"].ToBoolean(CultureInfo.CurrentCulture),
                IsPlayable = kv["IsPlayable"].ToBoolean(CultureInfo.CurrentCulture),
                EventGame = kv["EventGame"].ToBoolean(CultureInfo.CurrentCulture),
                MapConfigs = new(),
            };

            foreach (var map in addonInfo.Maps)
            {
                if (kv[map] is not null)
                {
                    addonInfo.MapConfigs.Add(map, new MapConfig
                    {
                        MaxPlayers = kv[map]["MaxPlayers"]?.ToInt32(CultureInfo.CurrentCulture),
                    });
                }
            }

            return true;
        }
    }
}
