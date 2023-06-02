using Dota2Modding.Common.Models.GameStructure;
using Dota2Modding.Common.Models.Parser;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace Dota2Modding.Common.Models.KvTree
{
    public class MergedKvTreeBuilder : IDisposable
    {
        private readonly Packages packages;
        private readonly Dictionary<string, Entry> bases = new();
        private readonly Entry entry;

        public MergedKvTreeBuilder(Packages packages, string path)
        {
            this.packages = packages;
            entry = packages.Get(path).FirstOrDefault();
            if (entry == null)
            {
                throw new InvalidDataException("None entry matched path");
            }
        }

        public void AddBase(string fullPath)
        {
            if (this.entry.Source.IsVpk)
            {
                throw new InvalidOperationException("Can't add base into vpk kv files");
            }
            var entry = packages.Get(fullPath).FirstOrDefault();
            if (entry == null)
            {
                throw new InvalidOperationException("None entry match path!");
            }
            bases.Add(Guid.NewGuid().ToString(), entry);
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
            
        }

        public void Dispose()
        {
            throw new NotImplementedException();
        }
    }
}
