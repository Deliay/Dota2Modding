using Dota2Modding.Common.Models.Addon;
using Dota2Modding.Common.Models.GameStructure;
using Dota2Modding.Common.Models.Parser;
using Microsoft.Extensions.Logging;
using System.Collections.Generic;
using System.IO;
using System.Linq;

namespace Dota2Modding.VisualEditor.Plugins.Project.Abstraction
{
    public class DotaProject
    {
        private readonly ILogger<DotaProject> logger;
        private readonly string addonInfoFilePath;

        public string WorkingDirectory { get; }

        public string Dota2Directory { get; }

        public AddonInfo AddonInfo { get; private set; }

        public Packages Packages { get; }

        public DotaProject(ILogger<DotaProject> logger, string addonInfoFilePath, Dota2Locator dota2Locator)
        {
            this.logger = logger;
            this.addonInfoFilePath = addonInfoFilePath;
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

        public void InitBasePackages(string addonFile)
        {

            logger.LogInformation("Starting loading dota2 files");

            foreach (var vpk in GenerateDota2BasicVPKs(Dota2Directory))
            {
                logger.LogInformation($"Load VPK: {vpk}");
                this.Packages.AddVpk(vpk);
            }
            logger.LogInformation($"Indexed {Packages.Count} files");

            logger.LogInformation("Starting loading addon-ins files");
            this.Packages.AddAddon(addonFile);
            logger.LogInformation($"Indexed {Packages.Count} files");

            logger.LogInformation($"Resolving addoninfo.txt");

            var addonInfoEntry = this.Packages.RootFolder.Folders.Values
                .Where(folder => !folder.IsVirtual)
                .Select(folder => folder.Entries.FirstOrDefault(ent => ent.Name == "addoninfo"))
                .FirstOrDefault(entry => entry is not null);

            if (addonInfoEntry == null)
            {
                return;
            }

            logger.LogInformation($"Found {addonInfoEntry.FullName}");

            var kvObject = KvLoader.Parse(Path.Combine(addonInfoEntry.Source.Path, addonInfoEntry.FullName));
            AddonInfo = new(kvObject.Name, kvObject.Value);
            AddonInfo.SetSite(addonInfoEntry);
            logger.LogInformation($"Dota2 custom game [{addonInfoEntry.Path}] loaded ");

        }
    }
}