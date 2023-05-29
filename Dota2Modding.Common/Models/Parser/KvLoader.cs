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
        public static KVObject Parse(string file)
        {
            try
            {
                using var stream = File.OpenRead(file);
                var kv = KVSerializer.Create(KVSerializationFormat.KeyValues1Text);

                return kv.Deserialize(stream);
            }
            catch
            {
                using var stream = File.OpenRead(file);
                var kv = KVSerializer.Create(KVSerializationFormat.KeyValues1Binary);

                return kv.Deserialize(stream);
            }
        }
    }
}
