using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace Dota2Modding.Common.Kv.Node
{
    public class KvElement
    {
        public string Name { get; set; }

        public class KvNull : KvElement
        {
            internal KvNull() { }
        }

        public static readonly KvNull Null = new();
    }
}
