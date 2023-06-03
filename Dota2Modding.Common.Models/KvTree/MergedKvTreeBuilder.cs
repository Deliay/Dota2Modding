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
    public abstract class MergedKvTreeBuilder<TSelf, TItem>
        where TSelf : MergedKvTree<TSelf, TItem>
        where TItem : BasicObject
    {
        protected readonly Packages packages;
        protected readonly Entry entry;
        protected readonly KvTreeStreamProvider provider;

        public MergedKvTreeBuilder(Packages packages, string path)
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
            var prefix = string.Join('\n', provider.Bases.Keys.Select(k => $"#base {k}"));

            var str = $"{prefix}\n{GetRawString(entry)}";

            return new MemoryStream(Encoding.UTF8.GetBytes(str));
        }

        public KVObject ReadKvObject()
        {
            using var stream = GetStream();
            return KvLoader.Plain.Deserialize(stream, new()
            {
                FileLoader = provider
            });
        }

        public abstract TSelf Build();
    }
}
