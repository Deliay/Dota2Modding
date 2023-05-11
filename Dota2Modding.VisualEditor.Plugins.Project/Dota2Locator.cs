using EmberKernel.Plugins.Components;
using GameFinder.StoreHandlers.Steam;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace Dota2Modding.VisualEditor.Plugins.Project
{
    public class Dota2Locator : IComponent
    {
        private readonly SteamHandler locator;

        public Dota2Locator()
        {
            this.locator = new SteamHandler();
        }

        public SteamGame? Locate()
        {
            return locator.FindOneGameById(570, out var _);
        }

        public void Dispose()
        {
            GC.SuppressFinalize(this);
        }
    }
}
