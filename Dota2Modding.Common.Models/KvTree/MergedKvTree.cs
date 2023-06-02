using Dota2Modding.Common.Models.GameStructure;
using Dota2Modding.Common.Models.Parser;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using ValveKeyValue;

namespace Dota2Modding.Common.Models.KvTree
{
    public class MergedKvTree : KVObject
    {
        private readonly KvTreeEntryMapping mapping;
        private MergedKvTree(KvTreeEntryMapping mapping, string name, KVValue value) : base(name, value)
        {
            this.mapping = mapping;
        }

        public static MergedKvTreeBuilder From(Packages packages, string path)
        {
            return new MergedKvTreeBuilder(packages, path);
        }

        public class MergedKvTreeBuilder
        {
            private readonly Packages packages;
            private readonly Dictionary<string, Entry> bases = new();
            private readonly Entry entry;
            private readonly KvTreeStreamProvider provider;

            internal MergedKvTreeBuilder(Packages packages, string path)
            {
                this.packages = packages;
                var entry = packages.Get(path).FirstOrDefault()
                    ?? throw new InvalidDataException("None entry matched path");
                this.entry = entry;
                provider = new(packages, entry.Path);
            }

            public void AddBase(string fullPath)
            {
                if (this.entry.Source.IsVpk)
                {
                    throw new InvalidOperationException("Can't add base into vpk kv files");
                }
                var entry = packages.Get(fullPath).FirstOrDefault()
                    ?? throw new InvalidOperationException("None entry match path!");

                provider.RegisterBase(Guid.NewGuid().ToString(), entry);
            }

            private string GetRawString(Entry entry)
            {
                if (entry.Source.IsVpk)
                {
                    var vpk = packages.GetVpk(entry);
                    var vpkEntry = packages.GetPackageEntry(entry);
                    vpk.ReadEntry(vpkEntry, out var data, true);

                    return Encoding.UTF8.GetString(data);
                }

                return File.ReadAllText(Path.Combine(entry.Source.Path, entry.FullName));
            }

            private Stream GetStream()
            {
                var prefix = string.Join('\n', bases.Keys.Select(k => $"#base {k}"));

                var str = $"{prefix}\n{GetRawString(entry)}";

                return new MemoryStream(Encoding.UTF8.GetBytes(str));
            }

            public MergedKvTree Build()
            {
                using var stream = GetStream();
                var kv = KvLoader.Plain.Deserialize(stream, new()
                {
                    FileLoader = provider
                });

                return new MergedKvTree(provider.Mapping, kv.Name, kv.Value);
            }
        }
    }
}
