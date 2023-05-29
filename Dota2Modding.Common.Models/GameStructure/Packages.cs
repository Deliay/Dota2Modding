using DBreeze.Transactions;
using Dota2Modding.Common.Models.Searching;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace Dota2Modding.Common.Models.GameStructure
{
    public class Packages : IDisposable
    {
        public Folder RootFolder { get; } = new("", null!);
        private readonly List<Entry> entries = new();
        private readonly Dictionary<string, HashSet<Entry>> entitySearchCache = new();
        private readonly Dictionary<Entry, Dictionary<Type, object>> entryExtraDataCache = new();
        private readonly List<IDisposable> disposables = new();

        public int Count => entries.Count;

        public IEnumerable<Entry> Find(string name)
        {
            return entitySearchCache.Keys.Where(k => k.Contains(name)).SelectMany(k => entitySearchCache[k]);
        }

        public void AddEntry(Entry entry)
        {
            var searchTxt = entry.GetContainsText();
            if (entitySearchCache.TryGetValue(searchTxt, out var searchCache))
            {
                searchCache.Add(entry);
            }
            else
            {
                entitySearchCache.Add(searchTxt, new() { entry });
            }

            RootFolder.Add(entry.FullName, entry);
            entries.Add(entry);
        }

        public void AssociateExtraData<T>(Entry entry, T data)
        {
            if (entryExtraDataCache.TryGetValue(entry, out var ext))
            {
                if (ext.ContainsKey(typeof(T)))
                {
                    ext[typeof(T)] = data;
                }
                else
                {
                    ext.Add(typeof(T), data);
                }
            }
        }

        public void AssociateDisposable(IDisposable disposable)
        {
            disposables.Add(disposable);
        }

        public void Dispose()
        {
            entryExtraDataCache.Clear();
            foreach (var item in disposables)
            {
                using var _t = item;
            }
            GC.SuppressFinalize(this);
        }
    }
}
