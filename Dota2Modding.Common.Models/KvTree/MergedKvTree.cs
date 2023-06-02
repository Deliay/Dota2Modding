using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using ValveKeyValue;

namespace Dota2Modding.Common.Models.KvTree
{
    public class MergedKvTree : KVObject
    {
        private MergedKvTree(string name, KVValue value) : base(name, value)
        {
        }

        public static MergedKvTree From(Stream stream)
        {

        }
    }
}
