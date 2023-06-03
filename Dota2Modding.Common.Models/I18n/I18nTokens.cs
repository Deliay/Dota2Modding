using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using ValveKeyValue;

namespace Dota2Modding.Common.Models.I18n
{
    public class I18nTokens : BasicObject
    {
        public I18nTokens(string name) : base(name)
        {
        }

        public override KVValue this[string key]
        {
            get => Overrides.Reverse().Select(d => d[key]).FirstOrDefault(r => r is not null) ?? ((KVObject)this)[key];
            set => base[key] = value; }
    }
}
