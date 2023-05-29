using DBreeze;
using DBreeze.Objects;
using DBreeze.TextSearch;
using DBreeze.Transactions;
using DBreeze.Utils;
using Dota2Modding.Common.Models.GameStructure;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Runtime.CompilerServices;
using System.Text;
using System.Text.Json;
using System.Threading.Tasks;

namespace Dota2Modding.Common.Models.Searching
{
    public class SearchEngine : IDisposable
    {
        private readonly DBreezeEngine engine;

        public SearchEngine(string workingDirectory)
        {
            this.engine = new(workingDirectory);
            CustomSerializator.ByteArraySerializator = (object o) => { return JsonSerializer.Serialize(o).To_UTF8Bytes(); };
            CustomSerializator.ByteArrayDeSerializator = (byte[] bt, Type t) => { return JsonSerializer.Deserialize(bt.UTF8_GetString(), t); };
        }

        public void BeginTransaction(Action<Transaction> action)
        {
            using var trx = engine.GetTransaction();
            action(trx);
        }

        public T BeginTransaction<T>(Func<Transaction, T> action)
        {
            using var trx = engine.GetTransaction();
            return action(trx);
        }

        public static void Insert<T>(Transaction trx, T data) where T : IIdentity
        {
            Insert(trx, data, SearchIndex<T>.Indexer(data));
        }

        public static TextSearchTable Search<T>(Transaction trx)
        {
            return trx.TextSearch(typeof(T).FullName);
        }

        public static void Insert<T>(Transaction trx, T data, List<DBreezeIndex> indexes) where T : IIdentity
        {
            var table = typeof(T).FullName;

            data.Id = trx.ObjectGetNewIdentity<ulong>(table);

            trx.ObjectInsert(table, new DBreezeObject<T>
            {
                NewEntity = true,
                Entity = data,
                Indexes = indexes,
            });
            if (SearchIndex<T>.IsFullSearchType)
            {
                var searchable = (IFullContextSearchable)data;
                trx.TextInsert(table, data.Id.To_8_bytes_array_BigEndian(), searchable.GetContainsText());
            }
        }

        public Transaction GetTransaction() => engine.GetTransaction();

        public void Dispose()
        {
            using var _e = engine;
            GC.SuppressFinalize(this);
        }
    }

    public static class SearchIndex<T> where T : IIdentity
    {
        private static readonly List<DBreezeIndex> emptyIndex = new();
        private static Func<T, List<DBreezeIndex>> indexer = (_) => emptyIndex;
        private static bool isFullTextSearchable = typeof(T).IsAssignableTo(typeof(IFullContextSearchable));

        public static Func<T, List<DBreezeIndex>> Indexer => indexer;
        public static bool IsFullSearchType => isFullTextSearchable;


        public static void RegisterIndexer(Func<T, List<DBreezeIndex>> indexer)
        {
            SearchIndex<T>.indexer = indexer;
        }
    }
}
