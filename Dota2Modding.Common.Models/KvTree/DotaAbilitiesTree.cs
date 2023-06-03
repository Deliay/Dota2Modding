using Dota2Modding.Common.Models.Game;
using Dota2Modding.Common.Models.Game.Abilities;
using Dota2Modding.Common.Models.Game.Entities;
using Dota2Modding.Common.Models.GameStructure;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using ValveKeyValue;

namespace Dota2Modding.Common.Models.KvTree
{
    public class DotaAbilitiesTree : MergedKvTree<DotaAbilitiesTree, DotaAbility>
    {
        internal DotaAbilitiesTree(KvTreeEntryMapping mapping, KVValue value) : base(mapping, "DOTAAbilities", value)
        {
        }

        protected override DotaAbility Convert(string key, KVValue value)
        {
            return new DotaAbility(key, value);
        }

        public class Builder : MergedKvTreeBuilder<DotaAbilitiesTree, DotaAbility>
        {
            public Builder(Packages packages, string path) : base(packages, path)
            {
            }

            public override DotaAbilitiesTree Build()
            {
                AddBase("dota/scripts/npc/npc_abilities.txt");
                return new DotaAbilitiesTree(provider.Mapping, ReadKvObject().Value);
            }
        }
    }
}
