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

        /// <summary>
        /// Select a file and save entity to disk, If entity last exist in any other files then remove all from other file
        /// first and save it to target file.
        /// </summary>
        /// <param name="entity"></param>
        public void Save(Entry entry, TItem entity)
        {
            if (mapping.TryGetValue(entity, out var existEntry))
            {
                // all entry from FS tree are same instance
                if (entry == existEntry)
                {

                }
            }
        }
    }
}
