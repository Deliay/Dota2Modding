using Dota2Modding.Common.Models.Addon;
using Dota2Modding.Common.Models.GameStructure;
using Dota2Modding.Common.Models.I18n;
using Dota2Modding.Common.Models.KvTree;
using Dota2Modding.Common.Models.Parser;
using Microsoft.Extensions.Logging;
using System.Collections.Generic;
using System.IO;
using System.Linq;

namespace Dota2Modding.Common.Models.Project
{
    public class DotaProject
    {
        public const string AddonCustomAbilities = "scripts\\npc\\npc_abilities_custom.txt";
        public const string AddonCustomHeroes = "scripts\\npc\\npc_heroes_custom.txt";
        public const string AddonCustomItems = "scripts\\npc\\npc_items_custom.txt";
        public const string AddonCustomUnits = "scripts\\npc\\npc_units_custom.txt";

        private readonly ILogger<DotaProject> logger;
        private readonly string addonInfoFilePath;

        public string WorkingDirectory { get; }
        public string WorkingDirectoryName { get; }
        public string AddonAbilitiesPath { get; }
        public string AddonHeroesPath { get; }
        public string Dota2Directory { get; }

        public AddonInfo AddonInfo { get; private set; }

        public Packages Packages { get; }

        public DotaHeroesTree Heroes { get; private set; }

        public DotaAbilitiesTree Abilities { get; private set; }

        public I18nDict I18n { get; private set; }

        public DotaProject(ILogger<DotaProject> logger, string addonInfoFilePath, Dota2Locator dota2Locator)
        {
            this.logger = logger;
            this.addonInfoFilePath = addonInfoFilePath;
            WorkingDirectory = Path.GetDirectoryName(addonInfoFilePath)!;
            WorkingDirectoryName = Path.GetFileName(WorkingDirectory)!;
            AddonAbilitiesPath = Path.Combine(WorkingDirectoryName, AddonCustomAbilities);
            AddonHeroesPath = Path.Combine(WorkingDirectoryName, AddonCustomHeroes);

            Dota2Directory = dota2Locator.Locate().Path;
            Packages = new Packages();
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
            // TODO refactor: move logging to event
            logger.LogInformation("Starting loading dota2 files");

            foreach (var vpk in GenerateDota2BasicVPKs(Dota2Directory))
            {
                logger.LogInformation($"Load VPK: {vpk}");
                Packages.AddVpk(vpk);
            }
            logger.LogInformation($"Indexed {Packages.Count} files");

            logger.LogInformation("Starting loading addon-ins files");
            Packages.AddAddon(addonInfoFilePath);
            logger.LogInformation($"Indexed {Packages.Count} files");

            logger.LogInformation($"Resolving addoninfo.txt");

            var addonInfoEntry = Packages.RootFolder.Folders.Values
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

            logger.LogInformation($"Loading heroes");
            Heroes = new DotaHeroesTree.Builder(Packages, AddonHeroesPath).Build();
            logger.LogInformation($"Loaded {Heroes.Mapping.Count} heroes");

            logger.LogInformation($"Loading abilities");
            Abilities = new DotaAbilitiesTree.Builder(Packages, AddonAbilitiesPath).Build();
            logger.LogInformation($"Loaded {Abilities.Mapping.Count} abilities");

            logger.LogInformation("Loading localization");
            I18n = new I18nDict.Builder(Packages).Build();
            logger.LogInformation($"Loaded {I18n.Languages.Count()} language localizations");

        }
    }
}