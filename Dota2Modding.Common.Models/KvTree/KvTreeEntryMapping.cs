using Dota2Modding.Common.Models.GameStructure;
using System;
using System.Collections.Generic;
using System.Diagnostics.CodeAnalysis;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using ValveKeyValue;

namespace Dota2Modding.Common.Models.KvTree
{
    public class KvTreeEntryMapping : Dictionary<string, Entry>
    {
        public Entry this[KVObject obj]
        {
            get
            {
                return this[obj.Name];
            }
            set
            {
                var entry = value ?? throw new ArgumentNullException(nameof(value));
                Save(obj, entry);
            }
        }

        public bool ContainsKey(KVObject obj)
        {
            return ContainsKey(obj.Name);
        }

        public bool TryGetValue(KVObject obj, [MaybeNullWhen(false)] out Entry? entry)
        {
            return TryGetValue(obj.Name, out entry);
        }

        public void Save(KVObject kv, Entry entry)
        {
            foreach (var obj in kv) 
            {
                if (!ContainsKey(obj.Name))
                {
                    Add(obj.Name, entry);
                }
            }
        }
    }
}
