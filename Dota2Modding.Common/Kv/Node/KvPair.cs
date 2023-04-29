using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace Dota2Modding.Common.Kv.Node
{
    public class KvPair<T> : KvElement
    {
        public T Value;
    }
}
