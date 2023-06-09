﻿using Dota2Modding.Common.Models;
using Dota2Modding.Common.Models.Game.Entities;
using Dota2Modding.Common.Models.GameStructure;
using Dota2Modding.Common.Models.Project;
using HandyControl.Tools.Command;
using HandyControl.Tools.Extension;
using System;
using System.Collections.Generic;
using System.ComponentModel;
using System.IO;
using System.Linq;
using System.Runtime.CompilerServices;
using System.Text;
using System.Threading.Tasks;
using System.Windows.Media;
using System.Windows.Media.Imaging;
using ValveKeyValue;
using ValveResourceFormat.ResourceTypes;

namespace Dota2Modding.VisualEditor.Plugins.Project.ViewModel
{
    public class DotaHeroesViewModel : INotifyPropertyChanged
    {
        private readonly DotaProject project;

        public event PropertyChangedEventHandler? PropertyChanged;


        public DotaHeroesViewModel(DotaProject project)
        {
            this.project = project;
            Language = project.I18n.Languages.FirstOrDefault(v => v == "English" || v == "schinese");
            SnapshotHeroList = EnumerableHeroList().OrderBy((h) => h.HeroID).ToList();
            HeroSources = project.Heroes.Mapping.Values.Select(e => e.GetPath()).ToHashSet();
            SearchCommand = new(SearchCommandLocal, (str) => true);
            ShowEditable = true;
        }

        public class HeroGridItem : DotaHero
        {
            private readonly DotaHeroesViewModel vm;

            public ImageSource Avatar => vm.GetHeroAvatar(this);

            public string? DisplayName => vm.project.I18n.GetToken(vm.Language, Name)
                ?? vm.project.I18n.GetToken(vm.Language, $"{Name}:n")
                ?? vm.project.I18n.GetToken(vm.Language, $"{OverrideHero}")
                ?? vm.project.I18n.GetToken(vm.Language, $"{OverrideHero}:n");

            public bool Editable { get; }

            public string SearchCriteria { get; }

            public HeroGridItem(DotaHeroesViewModel vm, string name, KVValue value, IEnumerable<BasicObject> overrides) : base(name, value)
            {
                AddOverride(overrides);
                this.vm = vm;
                Editable = !this.vm.project.Heroes.Mapping[Name].Source.IsVpk;
                SearchCriteria = $"{Name} {DisplayName} {HeroName} {HeroID}".ToLower();
            }

            public bool Match(string criteria)
            {
                return SearchCriteria.Contains(criteria.ToLower());
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

        private IEnumerable<HeroGridItem> EnumerableHeroList()
        {
            foreach (var heroKey in project.Heroes)
            {
                var key = heroKey.Name;
                var hero = project.Heroes[heroKey.Name];

                if (hero is null) continue;

                yield return new HeroGridItem(this, key, hero.Value, hero.Overrides);
            }
        }


        private readonly List<HeroGridItem> SnapshotHeroList;


        private IEnumerable<HeroGridItem> EnumerableCachedHeroList()
        {
            var result = SnapshotHeroList.AsEnumerable();

            if (!string.IsNullOrEmpty(selectedSource))
            {
                result = result.Where(h => project.Heroes.Mapping[h.Name].GetPath() == selectedSource);
            }

            if (!string.IsNullOrEmpty(searchText))
            {
                result = result.Where(h => h.Match(searchText));
            }
            
            if (showEditable)
            {
                result = result.Where(h => h.Editable);
            }

            return result;
        }

        private void SearchCommandLocal(string searchText)
        {
            SearchText = searchText;
        }

        public RelayCommand<string> SearchCommand { get; }

        public IEnumerable<HeroGridItem> HeroList => EnumerableCachedHeroList();

        public HashSet<string> HeroSources { get; private set; }

        private bool showEditable;

        public bool ShowEditable
        {
            get { return showEditable; }
            set { showEditable = value; OnPropertyChanged(); OnPropertyChanged(nameof(HeroList)); }
        }


        public string searchText;
        public string SearchText
        {
            get => searchText;
            set
            {
                searchText = value;
                OnPropertyChanged();
                OnPropertyChanged(nameof(HeroList));
            }
        }

        public string selectedSource;
        public string SelectedSource
        {
            get => selectedSource;
            set
            {
                selectedSource = value;
                OnPropertyChanged();
                OnPropertyChanged(nameof(HeroList));
            }
        }

        private string language;
        public string Language
        {
            get => language;
            set
            {
                language = value;
                OnPropertyChanged();
                OnPropertyChanged(nameof(HeroList));
            }
        }

        public IEnumerable<string> Languages => project.I18n.Languages;

        protected virtual void OnPropertyChanged([CallerMemberName] string propertyName = null)
        {
            PropertyChanged?.Invoke(this, new PropertyChangedEventArgs(propertyName));
        }
    }
}
