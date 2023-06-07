using Dota2Modding.Common.Models.Parser;
using System;
using System.Collections.Generic;
using System.IO;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using ValveKeyValue;

namespace Dota2Modding.Common.Models.KvTree
{
    /// <summary>
    /// Represent a Valve KV file, initialize by a stream which contains entire origin
    /// KV file content, and can 
    /// </summary>
    public class KvFile : KVObject
    {
        private readonly BlankRecordedFileProvider provider;

        private KvFile(BlankRecordedFileProvider provider, string name, KVValue value) : base(name, value)
        {
            this.provider = provider;
        }

        public string Serialize()
        {
            using var stream = new MemoryStream();
            KvLoader.Plain.Serialize(stream, this, new KVSerializerOptions
            {
                HasEscapeSequences = true,
            });

            var str = Encoding.UTF8.GetString(stream.ToArray());

            var includes = provider.IncludedFiles.Select(f => $"#base {f}");
            return $"{string.Join("\n", includes)}\n{str}";
        }

        private class BlankRecordedFileProvider : IIncludedFileLoader
        {
            private readonly List<string> _includedFiles = new();

            private readonly byte[] _template = Encoding.UTF8.GetBytes("\"shit\"{}");

            public Stream OpenFile(string filePath)
            {
                _includedFiles.Add(filePath);
                return new MemoryStream(_template);
            }

            public List<string> IncludedFiles => _includedFiles;
        }

        private static (KVObject, BlankRecordedFileProvider) ReadObject(Stream stream)
        {
            var loader = new BlankRecordedFileProvider();
            return (KvLoader.Plain.Deserialize(stream, new()
            {
                FileLoader = loader,
                HasEscapeSequences = true,
            }), loader);
        }

        public static KvFile Deserialize(Stream stream)
        {
            var (kv, provider) = ReadObject(stream);
            return new KvFile(provider, kv.Name, kv.Value);
        }
    }
}
