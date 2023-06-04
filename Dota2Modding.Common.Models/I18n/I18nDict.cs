using Dota2Modding.Common.Models.GameStructure;
using Dota2Modding.Common.Models.Parser;
using System;
using System.Collections.Generic;
using System.Globalization;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using ValveKeyValue;

namespace Dota2Modding.Common.Models.I18n
{
    public class I18nDict
    {

        private readonly Dictionary<string, I18nTokens> dicts = new();

        public IEnumerable<string> Languages => dicts.Keys;

        public string? GetToken(string lang, string key)
        {
            if (dicts.TryGetValue(lang, out var tokens))
            {
                return tokens.GetString(key);
            }

            return null;
        }

        public void Add(KVObject raw)
        {
            if (raw["Language"] is KVValue value)
            {
                var lang = value.ToString(CultureInfo.InvariantCulture);
                var tokens = raw["Tokens"];

                if (!dicts.ContainsKey(lang))
                {
                    dicts.Add(lang, new I18nTokens(lang));
                }

                dicts[lang].AddOverride(new IgnoreCaseBasicObject(new BasicObject(lang, tokens)));
            }
        }

        public class Builder
        {
            private readonly Packages packages;

            public Builder(Packages packages)
            {
                this.packages = packages;
            }

            private IEnumerable<Entry> EnumerateFolder(Folder folder)
            {
                return folder.Entries.Concat(folder.Folders.Values.SelectMany(EnumerateFolder));
            }

            private IEnumerable<Entry> EnumeratePossibleLanguageEntry()
            {
                var vpkFolders = packages.RootFolder.Folders.Values
                    .Where(f => f.IsVirtual)
                    .Where(f => f.Folders.ContainsKey("resource"))
                    .Select(f => f.Folders["resource"])
                    .Where(f => f.Folders.ContainsKey("localization"))
                    .Select(f => f.Folders["localization"])
                    .SelectMany(EnumerateFolder);

                var addonFolders = packages.RootFolder.Folders.Values
                    .Where(f => !f.IsVirtual)
                    .Where(f => f.Folders.ContainsKey("resource"))
                    .Select(f => f.Folders["resource"])
                    .SelectMany(f => f.Entries);

                return vpkFolders.Concat(addonFolders)
                    .Where(ent => ent.Extension == "txt")
                    .Where(ent => ent.Name.StartsWith("abilities_") || ent.Name.StartsWith("dota_") || ent.Name.StartsWith("addon_"));
            }

            public I18nDict Build()
            {
                var dict = new I18nDict();

                foreach (var entry in EnumeratePossibleLanguageEntry())
                {
                    using var stream = entry.LoadResourceStream(packages);
                    var kv = KvLoader.Plain.Deserialize(stream, new()
                    {
                        HasEscapeSequences = true,
                    });

                    if (kv.Name == "lang")
                    {
                        dict.Add(kv);
                    }
                }

                return dict;
            }
        }
    }
}
