using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using ValveKeyValue;

namespace Dota2Modding.Common.Models
{
    public static class KvLoader
    {
        public static KVObject Parse(string file)
        {
            using var stream = File.OpenRead(file);
            var kv = KVSerializer.Create(KVSerializationFormat.KeyValues1Text);

            return kv.Deserialize(stream);
        }
    }
}
