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
    public class KvTreeStreamProvider : IIncludedFileLoader
    {
        private readonly Packages packages;
        private readonly string baseFolder;
        private readonly Dictionary<string, Entry> bases = new();
        private readonly KvTreeEntryMapping keyTree = new();

        public KvTreeEntryMapping Mapping => keyTree;
        public Dictionary<string, Entry> Bases => bases;

        public KvTreeStreamProvider(Packages packages, string baseFolder)
        {
            this.packages = packages;
            this.baseFolder = baseFolder;
        }

        public void RegisterBase(string key, Entry entry)
        {
            bases.Add(key, entry);
        }

        public void Dispose()
        {
        }

        private Stream GetStream(Entry entry)
        {
            if (entry.Source.IsVpk)
            {
                var vpk = packages.GetVpk(entry);
                var vpkEntry = packages.GetPackageEntry(entry);
                vpk.ReadEntry(vpkEntry, out var data, true);

                return new MemoryStream(data);
            }

            return File.OpenRead(Path.Combine(entry.Source.Path, entry.FullName));
        }

        public Stream _OpenFile(Entry entry)
        {
            using var raw = GetStream(entry);
            var kv = KvLoader.Plain.Deserialize(GetStream(entry), new KVSerializerOptions
            {
                FileLoader = BlankFileStreamProdiver.Instance,
            });
            keyTree.Save(kv, entry);

            return GetStream(entry);
        }

        public Stream OpenFile(string filePath)
        {
            if (bases.TryGetValue(filePath, out var extraBase))
            {                   
                return _OpenFile(extraBase);
            }

            var entry = packages.Get(Path.Combine(baseFolder, filePath)).FirstOrDefault()
                ?? throw new InvalidDataException("Invalid path:" + filePath);

            return _OpenFile(entry);
        }
    }
}
