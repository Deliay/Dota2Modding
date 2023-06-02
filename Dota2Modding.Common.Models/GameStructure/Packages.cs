using DBreeze.Transactions;
using Dota2Modding.Common.Models.Searching;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using static System.Runtime.InteropServices.JavaScript.JSType;

namespace Dota2Modding.Common.Models.GameStructure
{
    public class Packages : IDisposable
    {
        public Folder RootFolder { get; } = new("", null!);
        private readonly List<Entry> entries = new();
        private readonly Dictionary<Source, HashSet<Entry>> sourcesCache = new();
        private readonly Dictionary<string, HashSet<Entry>> entitySearchCache = new();
        private readonly Dictionary<string, HashSet<Entry>> fullPathCache = new();
        private readonly Dictionary<Entry, Dictionary<Type, object>> entryExtraDataCache = new();
        private readonly List<IDisposable> disposables = new();

        public int Count => entries.Count;

        public IEnumerable<Entry> Search(string name)
        {
            return entitySearchCache.Keys.Where(k => k.Contains(name)).SelectMany(k => entitySearchCache[k]);
        }

        public IEnumerable<Entry> Get(string fullPath)
        {
            return fullPathCache[fullPath] ?? Enumerable.Empty<Entry>();
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

            if (fullPathCache.TryGetValue(searchTxt, out var fullPath))
            {
                fullPath.Add(entry);
            }
            else
            {
                fullPathCache.Add(searchTxt, new() { entry });
            }

            if (sourcesCache.TryGetValue(entry.Source, out var sourceCache))
            {
                sourceCache.Add(entry);
            }
            else
            {
                sourcesCache.Add(entry.Source, new () { entry });
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
            else
            {
                entryExtraDataCache.Add(entry, new() { { typeof(T), data } });
            }
        }

        public T GetExtraData<T>(Entry entry)
        {
            if (entryExtraDataCache.TryGetValue(entry, out var ext))
            {
                if (ext.TryGetValue(typeof(T), out var value))
                {
                    return (T)value;
                }
            }
            return default!;
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
