using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using ValveKeyValue;

namespace Dota2Modding.Common.Models.Parser
{
    public static class KvLoader
    {
        private static KVSerializer kv1txt = KVSerializer.Create(KVSerializationFormat.KeyValues1Text);
        private static KVSerializer kv1bin = KVSerializer.Create(KVSerializationFormat.KeyValues1Binary);
        public static KVObject Parse(string file)
        {
            try
            {
                using var stream = File.OpenRead(file);;
                return kv1txt.Deserialize(stream);
            }
            catch
            {
                using var stream = File.OpenRead(file);

                return kv1bin.Deserialize(stream);
            }
        }

        public static void Save(string file, KVObject value)
        {
            var tempFileName = Path.GetFileName(Path.GetTempFileName());
            var path = $"{file}_{tempFileName}";
            {
                using var stream = File.OpenWrite(path);

                kv1txt.Serialize(stream, value);
            }
            File.Move(path, file, true);
        }
    }
}
