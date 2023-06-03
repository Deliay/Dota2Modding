using Dota2Modding.Common.Models.Game;
using Dota2Modding.Common.Models.GameStructure;
using Dota2Modding.Common.Models.Parser;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using ValveKeyValue;

namespace Dota2Modding.Common.Models.KvTree
{
    public abstract class MergedKvTree<TSelf, TItem> : KvSingleTypeCollecion<TItem>
        where TItem : BasicObject
        where TSelf : MergedKvTree<TSelf, TItem>
    {
        private readonly KvTreeEntryMapping mapping;
        public KvTreeEntryMapping Mapping => mapping;
        protected MergedKvTree(KvTreeEntryMapping mapping, string name, KVValue value) : base(name, value)
        {
            this.mapping = mapping;
        }
    }
}
