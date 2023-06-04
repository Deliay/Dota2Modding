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
    public class DotaProject : IDisposable
    {
        public const string AddonCustomAbilities = "scripts\\npc\\npc_abilities_custom.txt";
        public const string AddonCustomHeroes = "scripts\\npc\\npc_heroes_custom.txt";
        public const string AddonCustomItems = "scripts\\npc\\npc_items_custom.txt";
        public const string AddonCustomUnits = "scripts\\npc\\npc_units_custom.txt";

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

        public DotaProject(string addonInfoFilePath, Dota2Locator dota2Locator)
        {
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

        public delegate void StatusUpdate(string phase, string message, int maxStep, int currentStep);

        public event StatusUpdate LoadingStatusUpdated;

        public const int InitStep = 7;

        public void InitBasePackages(int extraStep = 0)
        {
            int maxStep = extraStep + InitStep;
            // TODO refactor: move logging to event
            LoadingStatusUpdated?.Invoke("Init", "Starting loading dota2 files", maxStep, 1);

            foreach (var vpk in GenerateDota2BasicVPKs(Dota2Directory))
            {
                LoadingStatusUpdated?.Invoke("VPK", $"Load VPK: {vpk}", maxStep, 2);
                Packages.AddVpk(vpk);
            }
            LoadingStatusUpdated?.Invoke("VPK", $"Indexed {Packages.Count} files", maxStep, 2);

            LoadingStatusUpdated?.Invoke("Addon", "Starting loading addon-ins files", maxStep, 3);
            Packages.AddAddon(addonInfoFilePath);
            LoadingStatusUpdated?.Invoke("Addon", $"Indexed {Packages.Count} files", maxStep, 3);

            LoadingStatusUpdated?.Invoke("Addon", $"Resolving addoninfo.txt", maxStep, 3);

            var addonInfoEntry = Packages.RootFolder.Folders.Values
                .Where(folder => !folder.IsVirtual)
                .Select(folder => folder.Entries.FirstOrDefault(ent => ent.Name == "addoninfo"))
                .FirstOrDefault(entry => entry is not null);

            if (addonInfoEntry == null)
            {
                return;
            }

            LoadingStatusUpdated?.Invoke("Addon", $"Found {addonInfoEntry.FullName}", maxStep, 3);

            var kvObject = KvLoader.Parse(Path.Combine(addonInfoEntry.Source.Path, addonInfoEntry.FullName));
            AddonInfo = new(kvObject.Name, kvObject.Value);
            AddonInfo.SetSite(addonInfoEntry);
            LoadingStatusUpdated?.Invoke("Addon", $"Dota2 custom game [{addonInfoEntry.Path}] loaded ", maxStep, 3);

            LoadingStatusUpdated?.Invoke("Heroes", $"Loading heroes...", maxStep, 4);
            Heroes = new DotaHeroesTree.Builder(Packages, AddonHeroesPath).Build();
            LoadingStatusUpdated?.Invoke("Heroes", $"Loaded {Heroes.Mapping.Count} heroes", maxStep, 4);

            LoadingStatusUpdated?.Invoke("Abilities", $"Loading abilities...", maxStep, 5);
            Abilities = new DotaAbilitiesTree.Builder(Packages, AddonAbilitiesPath).Build();
            LoadingStatusUpdated?.Invoke("Abilities", $"Loaded {Abilities.Mapping.Count} abilities", maxStep, 5);

            LoadingStatusUpdated?.Invoke("I18n", $"Loading localization...", maxStep, 6);
            I18n = new I18nDict.Builder(Packages).Build();
            LoadingStatusUpdated?.Invoke("I18n", $"Loaded {I18n.Languages.Count()} language localizations", maxStep, 6);

        }

        public void Dispose()
        {
            GC.SuppressFinalize(this);
            using var pak = this.Packages;
        }
    }
}