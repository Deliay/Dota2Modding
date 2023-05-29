using System;
using System.Collections.Generic;
using System.Globalization;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using ValveKeyValue;

namespace Dota2Modding.Common.Models
{
    public class BasicObject : KVObject
    {
        public BasicObject(string name, KVValue value) : base(name, value)
        {
        }
        public BasicObject(string name) : base(name, Enumerable.Empty<KVObject>())
        {
        }

        public string Name { get; set; }

        protected IReadOnlySet<T> FlagsOf<T>(string key) where T : struct
        {
            return ParseFlags<T>(this[key].ToString(CultureInfo.CurrentCulture));
        }

        protected T ToFlag<T>(string key) where T : struct
        {
            return Enum.Parse<T>(this[key].ToString(CultureInfo.CurrentCulture));
        }

        public static string FromFlag<T>(T flag) where T : struct
        {
            return flag.ToString();
        }

        public static IReadOnlySet<T> ParseFlags<T>(string raw) where T : struct
        {
            return raw.Split('|')
                .Select(item => item.Trim())
                .Select(item => Enum.Parse<T>(item))
                .ToHashSet();
        }

        public static string ToFlag<T>(IReadOnlySet<T> set) where T : struct
        {
            return string.Join(" | ", set);
        }
    }
}
