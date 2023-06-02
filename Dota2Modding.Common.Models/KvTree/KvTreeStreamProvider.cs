using Dota2Modding.Common.Models.GameStructure;
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
        private Dictionary<string, Entry> bases = new();
        private KvTreeKeyDependencyTree keyTree = new();

        public KvTreeStreamProvider(Packages packages)
        {
            this.packages = packages;
        }

        public void RegisterBase(string key, Entry entry)
        {
            bases.Add(key, entry);
        }

        public void Dispose()
        {
            throw new NotImplementedException();
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

        public Stream OpenFile(string filePath)
        {
            if (bases.TryGetValue(filePath, out var extraBase))
            {
                
            }
        }
    }
}
