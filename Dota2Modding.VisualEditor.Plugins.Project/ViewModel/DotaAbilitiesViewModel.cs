using Dota2Modding.Common.Models.Game.Abilities;
using Dota2Modding.Common.Models.Project;
using System;
using System.Collections.Generic;
using System.ComponentModel;
using System.Linq;
using System.Runtime.CompilerServices;
using System.Text;
using System.Threading.Tasks;
using System.Windows.Media.Imaging;
using System.Windows.Media;
using ValveKeyValue;
using System.IO;
using Dota2Modding.Common.Models.GameStructure;
using Dota2Modding.Common.Models.Game.Entities;
using static Dota2Modding.VisualEditor.Plugins.Project.ViewModel.DotaHeroesViewModel;
using HandyControl.Tools.Command;

namespace Dota2Modding.VisualEditor.Plugins.Project.ViewModel
{
    public class DotaAbilitiesViewModel : INotifyPropertyChanged
    {
        private readonly DotaProject project;

        public event PropertyChangedEventHandler? PropertyChanged;
        protected virtual void OnPropertyChanged([CallerMemberName] string propertyName = null)
        {
            PropertyChanged?.Invoke(this, new PropertyChangedEventArgs(propertyName));
        }

        public DotaAbilitiesViewModel(DotaProject project)
        {
            this.project = project;
            Language = project.I18n.Languages.FirstOrDefault(v => v == "English" || v == "schinese");
            SnapshotAbilityList = EnumerableAbilities().OrderBy((h) => h.ID).ToList();
            AbilitySources = project.Abilities.Mapping.Values.Select(e => e.GetPath()).ToHashSet();
            SearchCommand = new(SearchCommandLocal, (str) => true);
            ShowEditable = true;
        }

        public class AbilityGridItem : DotaAbility
        {
            private readonly DotaAbilitiesViewModel vm;

            public ImageSource Avatar => vm.GetSpellIcon(this);

            public AbilityGridItem(DotaAbilitiesViewModel vm, string name, KVValue value) : base(name, value)
            {
                this.vm = vm;
                SearchCriteria = $"{Name} {DisplayName}".ToLower();
                Editable = !vm.project.Abilities.Mapping[Name].Source.IsVpk;
            }
            private const string I18nPrefix = "DOTA_Tooltip_ability_";
            public string? DisplayName => vm.project.I18n.GetToken(vm.Language, Name)
                ?? vm.project.I18n.GetToken(vm.Language, $"{I18nPrefix}{Name}");

            public string SearchCriteria { get; }

            public bool Match(string criteria)
            {
                return SearchCriteria.Contains(criteria.ToLower());
            }

            public bool Editable { get; }
        }

        private const string SpellIconFolder = "panorama/images/spellicons/";
        private const string VpkSpellIconFolder = $"dota/{SpellIconFolder}";

        private const string OldSpellIconFolder = "resource/flash3/images/spellicons/";
        private const string OldVpkSpellIconFolder = $"dota/{OldSpellIconFolder}";

        private const string NoAvatarUri = "pack://application:,,,/Dota2Modding.VisualEditor.Plugins.Project;component/Resources/_no_avatar.png";
        public static readonly ImageSource NoAvatarSource = new BitmapImage(new Uri(NoAvatarUri));

        private IEnumerable<string> SpellIconFallback(string key, string tex)
        {
            if (string.IsNullOrWhiteSpace(key)) yield break;

            yield return $"{VpkSpellIconFolder}{key}_png.vtex_c";
            yield return Path.Combine(project.WorkingDirectoryName, SpellIconFolder, $"{key}_png.vtex_c");
            yield return Path.Combine(project.WorkingDirectoryName, SpellIconFolder, $"{key}.png");

            yield return $"{OldVpkSpellIconFolder}{key}_png.vtex_c"; ;
            yield return Path.Combine(project.WorkingDirectoryName, OldSpellIconFolder, $"{key}_png.vtex_c");
            yield return Path.Combine(project.WorkingDirectoryName, OldSpellIconFolder, $"{key}.png");

            yield return $"{VpkSpellIconFolder}{tex}_png.vtex_c";
            yield return Path.Combine(project.WorkingDirectoryName, SpellIconFolder, $"{tex}_png.vtex_c");
            yield return Path.Combine(project.WorkingDirectoryName, SpellIconFolder, $"{tex}.png");

            yield return $"{OldVpkSpellIconFolder}{tex}_png.vtex_c"; ;
            yield return Path.Combine(project.WorkingDirectoryName, OldSpellIconFolder, $"{tex}_png.vtex_c");
            yield return Path.Combine(project.WorkingDirectoryName, OldSpellIconFolder, $"{tex}.png");
        }

        private IEnumerable<Entry> SpellIconFallback(DotaProject project, string key, string tex)
            => SpellIconFallback(key, tex).SelectMany(project.Packages.Get);

        public ImageSource GetSpellIcon(DotaAbility ability)
        {
            var key = ability.Name;
            // search vpk first for most occurrence

            var entry = SpellIconFallback(project, key, ability.AbilityTextureName).FirstOrDefault() ?? null!;

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

        private IEnumerable<AbilityGridItem> EnumerableAbilities()
        {
            foreach (var abilityKv in project.Abilities)
            {
                var key = abilityKv.Name;
                var ability = project.Abilities[key];

                if (ability is null) continue;

                yield return new AbilityGridItem(this, key, ability.Value);
            }
        }

        private readonly List<AbilityGridItem> SnapshotAbilityList;

        private IEnumerable<AbilityGridItem> EnumerableCachedAbilityList()
        {
            var result = SnapshotAbilityList.AsEnumerable();

            if (!string.IsNullOrEmpty(selectedSource))
            {
                result = result.Where(h => project.Abilities.Mapping[h.Name].GetPath() == selectedSource);
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

        public IEnumerable<AbilityGridItem> AbilityList => EnumerableCachedAbilityList();

        public HashSet<string> AbilitySources { get; private set; }

        private bool showEditable;

        public bool ShowEditable
        {
            get { return showEditable; }
            set { showEditable = value; OnPropertyChanged(); OnPropertyChanged(nameof(AbilityList)); }
        }


        public string searchText;
        public string SearchText
        {
            get => searchText;
            set
            {
                searchText = value;
                OnPropertyChanged();
                OnPropertyChanged(nameof(AbilityList));
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
                OnPropertyChanged(nameof(AbilityList));
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
                OnPropertyChanged(nameof(AbilityList));
            }

        }
        public IEnumerable<string> Languages => project.I18n.Languages;
    }
}
