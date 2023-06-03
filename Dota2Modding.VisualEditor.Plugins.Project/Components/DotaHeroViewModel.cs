using Dota2Modding.Common.Models;
using Dota2Modding.Common.Models.Game.Entities;
using Dota2Modding.Common.Models.GameStructure;
using Dota2Modding.Common.Models.Project;
using System;
using System.Collections.Generic;
using System.ComponentModel;
using System.IO;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using System.Windows.Media;
using System.Windows.Media.Imaging;
using ValveKeyValue;
using ValveResourceFormat.ResourceTypes;

namespace Dota2Modding.VisualEditor.Plugins.Project.Components
{
    public class DotaHeroViewModel : INotifyPropertyChanged
    {
        private readonly DotaProject project;

        public event PropertyChangedEventHandler? PropertyChanged;

        public IEnumerable<HeroGridItem> HeroList { get; private set; }

        public DotaHeroViewModel(DotaProject project)
        {
            this.project = project;
            this.HeroList = EnumerableHeroList().OrderBy((h) => h.HeroID).ToList();
        }

        public class HeroGridItem : DotaHero
        {
            public ImageSource Avatar { get; set; }
            public HeroGridItem(string name, KVValue value, IEnumerable<BasicObject> overrides) : base(name, value)
            {
                AddOverride(overrides);

            }
        }

        private const string HeroSelectionAvatarFolder = "panorama/images/heroes/selection/";
        private const string VpkHeroSelectionAvatarFolder = $"dota/{HeroSelectionAvatarFolder}";

        private const string NoAvatarUri = "pack://application:,,,/Dota2Modding.VisualEditor.Plugins.Project;component/Resources/_no_avatar.png";
        public static readonly ImageSource NoAvatarSource = new BitmapImage(new Uri(NoAvatarUri));

        private IEnumerable<string> HeroAvatarsFallback(string key)
        {
            if (string.IsNullOrWhiteSpace(key)) yield break;

            yield return $"{VpkHeroSelectionAvatarFolder}{key}_png.vtex_c";
            yield return Path.Combine(project.WorkingDirectoryName, HeroSelectionAvatarFolder, $"{key}_png.vtex_c");
            yield return Path.Combine(project.WorkingDirectoryName, HeroSelectionAvatarFolder, $"{key}.png");
        }

        private IEnumerable<Entry> HeroEntriesFallback(DotaProject project, string key)
            => HeroAvatarsFallback(key).SelectMany(project.Packages.Get);

        public ImageSource GetHeroAvatar(DotaHero hero)
        {
            var key = hero.Name;
            var baseKey = hero.BaseClass;
            var overrideKey = hero.OverrideHero;
            // search vpk first for most occurrence

            var entry = HeroEntriesFallback(project, hero.Name).FirstOrDefault()
                ?? HeroEntriesFallback(project, baseKey).FirstOrDefault()
                ?? HeroEntriesFallback(project, overrideKey).FirstOrDefault()
                ?? null!;

            if (entry == null) return NoAvatarSource;

            var raw = entry.LoadResourceData(project.Packages);
            using var ms = new MemoryStream(raw);
            var bi = new BitmapImage();

            bi.BeginInit();
            bi.CacheOption = BitmapCacheOption.OnLoad;
            bi.StreamSource = ms;
            bi.EndInit();

            return bi;
        }

        public IEnumerable<HeroGridItem> EnumerableHeroList()
        {
            foreach (var heroKey in project.Heroes)
            {
                var key = heroKey.Name;
                var hero = project.Heroes[heroKey.Name];

                if (hero is null) continue;

                yield return new HeroGridItem(key, hero.Value, hero.Overrides)
                {
                    Avatar = GetHeroAvatar(hero),
                };
            }
        }
    }
}
