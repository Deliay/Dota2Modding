using Dota2Modding.Common.Models.Addon;
using Dota2Modding.Common.Models.GameStructure;
using Microsoft.Extensions.Logging;
using System.Collections.Generic;
using System.IO;

namespace Dota2Modding.VisualEditor.Plugins.Project.Abstraction
{
    public class DotaProject
    {
        private readonly ILogger<DotaProject> logger;
        private readonly string addonInfoFilePath;

        public string WorkingDirectory { get; }

        public string Dota2Directory { get; }

        public AddonInfo AddonInfo { get;  }

        public Packages Packages { get; }

        public DotaProject(ILogger<DotaProject> logger, string addonInfoFilePath, AddonInfo addonInfo, Dota2Locator dota2Locator)
        {
            this.logger = logger;
            this.addonInfoFilePath = addonInfoFilePath;
            AddonInfo = addonInfo;
            WorkingDirectory = Path.GetDirectoryName(addonInfoFilePath)!;

            this.Dota2Directory = dota2Locator.Locate().Path;
            this.Packages = new Packages();
        }

        private static IEnumerable<string> GenerateDota2BasicVPKs(string dota2directory)
        {
            var gameDir = Path.Combine(dota2directory, "game");
            yield return Path.Combine(gameDir, "dota", "pak01_dir.vpk");
            var matchedDirs = Directory.EnumerateDirectories(gameDir, "dota_*");
            foreach (var subDir in matchedDirs)
            {
                var vpk = Path.Combine(subDir, "pak01_dir.vpk");
                if (File.Exists(vpk))
                {
                    yield return vpk;
                }
            }
        }

        public void InitBasePackages()
        {

            logger.LogInformation("Starting loading dota2 files");

            foreach (var vpk in GenerateDota2BasicVPKs(Dota2Directory))
            {
                logger.LogInformation($"Load VPK: {vpk}");
                this.Packages.AddVpk(vpk);
            }

            logger.LogInformation($"Indexed {Packages.Count} files");
        }
    }
}