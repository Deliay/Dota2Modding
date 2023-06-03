using Dota2Modding.Common.Models.Game.Entities;
using Dota2Modding.Common.Models.GameStructure;
using System;
using System.Collections.Generic;
using System.Globalization;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using ValveKeyValue;

namespace Dota2Modding.Common.Models.KvTree
{
    public class DotaHeroesTree : MergedKvTree<DotaHeroesTree, DotaHero>
    {
        internal DotaHeroesTree(KvTreeEntryMapping mapping, KVValue value) : base(mapping, "DOTAHeroes", value)
        {
        }

        private BasicObject FindOverrideObject(BasicObject basicObject, string key)
        {
            var baseClassValue = basicObject[key];
            if (baseClassValue is not null)
            {
                var baseClass = baseClassValue.ToString(CultureInfo.InvariantCulture);
                var targetHero = this[baseClass];

                if (targetHero is not null)
                {
                    return targetHero;
                }
            }

            return null;
        }

        protected override DotaHero Convert(string key, KVValue value)
        {
            var hero = new DotaHero(key, value);
            if (FindOverrideObject(hero, "BaseClass") is BasicObject baseHero) hero.AddOverride(baseHero);
            if (FindOverrideObject(hero, "override_hero") is BasicObject overrideHero) hero.AddOverride(overrideHero);

            return hero;
        }

        public class Builder : MergedKvTreeBuilder<DotaHeroesTree, DotaHero>
        {
            public Builder(Packages packages, string path) : base(packages, path)
            {
            }

            public override DotaHeroesTree Build()
            {
                AddBase("dota/scripts/npc/npc_heroes.txt");
                return new DotaHeroesTree(provider.Mapping, ReadKvObject().Value);
            }
        }
    }
}
