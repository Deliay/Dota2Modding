using Dota2Modding.Common.Models.Addon;
using System.IO;

namespace Dota2Modding.VisualEditor.Plugins.Project.Abstraction
{
    public class DotaProject
    {
        private readonly string addonInfoFilePath;

        public string WorkingDirectory { get; set; }

        public AddonInfo AddonInfo { get; set; }

        public DotaProject(string addonInfoFilePath, AddonInfo addonInfo)
        {
            this.addonInfoFilePath = addonInfoFilePath;
            AddonInfo = addonInfo;
            WorkingDirectory = Path.GetDirectoryName(addonInfoFilePath)!;
        }


    }
}