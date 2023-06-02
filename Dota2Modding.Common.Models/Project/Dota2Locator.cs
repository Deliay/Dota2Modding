using GameFinder.StoreHandlers.Steam;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace Dota2Modding.Common.Models.Project
{
    public class Dota2Locator
    {
        private readonly SteamHandler locator;

        public Dota2Locator()
        {
            locator = new SteamHandler();
        }

        public SteamGame? Locate()
        {
            return locator.FindOneGameById(570, out var _);
        }
    }
}
