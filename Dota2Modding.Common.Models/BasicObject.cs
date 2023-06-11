using Dota2Modding.Common.Models.Converters;
using Dota2Modding.Common.Models.GameStructure;
using Dota2Modding.Common.Models.Parser;
using System;
using System.Collections.Generic;
using System.ComponentModel;
using System.Globalization;
using System.Linq;
using System.Runtime.CompilerServices;
using System.Text;
using System.Threading.Tasks;
using ValveKeyValue;

namespace Dota2Modding.Common.Models
{
    public class BasicObject : KVObject, INotifyPropertyChanged
    {

        static BasicObject()
        {
            TypeDescriptor.AddProvider(new BasicObjectDescriptionProvider(typeof(BasicObject), typeof(BasicObject)), typeof(BasicObject));
        }

        public BasicObject(string name, KVValue value) : base(name, value)
        {
        }
        public BasicObject(string name) : base(name, Enumerable.Empty<KVObject>())
        {
        }

        protected IReadOnlySet<T>? FlagsOf<T>(string key) where T : struct
        {
            return ParseFlags<T>(this[key]?.ToString(CultureInfo.CurrentCulture));
        }

        protected T? ToFlag<T>(string key) where T : struct
        {
            var raw = this[key]?.ToString(CultureInfo.CurrentCulture);
            if (raw is null) return default;

            return Enum.Parse<T>(raw);
        }

        public static string FromFlag<T>(T flag) where T : struct
        {
            return flag.ToString();
        }

        public static IReadOnlySet<T>? ParseFlags<T>(string raw) where T : struct
        {
            if (raw is null) return null!;
            return raw.Split('|')
                .Select(item => item.Trim())
                .Where(k =>
                {
                    var checkResult = Enum.IsDefined(typeof(T), k);
                    return checkResult;
                })
                .Select(Enum.Parse<T>)
                .ToHashSet();
        }

        public static string ToFlag<T>(IReadOnlySet<T> set) where T : struct
        {
            return string.Join(" | ", set);
        }

        protected void SetValue(string key, KVValue value)
        {
            base[key] = value;
            if (Site is not null)
            {
                KvLoader.Save(Site.GetFullPath(), this);
            }
        }

        public string? GetString(string key)
        {
            return this[key]?.ToString(CultureInfo.CurrentCulture);
        }

        public float? GetSingle(string key)
        {
            return this[key]?.ToSingle(CultureInfo.CurrentCulture);
        }

        public bool? GetBoolean(string key)
        {
            return this[key]?.ToBoolean(CultureInfo.CurrentCulture);
        }

        public int? GetInt32(string key)
        {
            return this[key]?.ToInt32(CultureInfo.CurrentCulture);
        }

        private Entry Site { get; set; }

        public void SetSite(Entry site)
        {
            if (site.Source.IsVpk)
            {
                throw new InvalidDataException("Can't set site to VPK entry");
            }

            Site = site;
        }

        private readonly List<BasicObject> overrideDict = new();

        public event PropertyChangedEventHandler? PropertyChanged;
        protected virtual void OnPropertyChanged([CallerMemberName] string propertyName = null)
        {
            PropertyChanged?.Invoke(this, new PropertyChangedEventArgs(propertyName));
        }

        public IReadOnlyList<BasicObject> Overrides => overrideDict;

        public void AddOverride(BasicObject obj)
        {
            overrideDict.Add(obj);
        }

        public void AddOverride(IEnumerable<BasicObject> obj)
        {
            overrideDict.AddRange(obj);
        }

        public new virtual KVValue this[string key]
        {
            get
            {
                return base[key] ?? overrideDict.Select(dict => dict[key]).FirstOrDefault(r => r is not null)!;
            }
            set
            {
                base[key] = value;
                OnPropertyChanged(key);
            }
        }
    }
}
