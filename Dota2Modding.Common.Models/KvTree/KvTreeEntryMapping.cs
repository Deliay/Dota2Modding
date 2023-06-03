using Dota2Modding.Common.Models.GameStructure;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using ValveKeyValue;

namespace Dota2Modding.Common.Models.KvTree
{
    public class KvTreeEntryMapping : Dictionary<string, Entry>
    {
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
