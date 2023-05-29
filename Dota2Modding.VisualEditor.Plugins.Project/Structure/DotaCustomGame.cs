using Dota2Modding.Common.Models.Game.Abilities;
using Dota2Modding.Common.Models.Game.Entities;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace Dota2Modding.VisualEditor.Plugins.Project.Structure
{
    public class DotaCustomGame
    {
        public DotaAbilities Abilities { get; set; }

        public DotaHeroes Heroes { get; set; }
    }
}
