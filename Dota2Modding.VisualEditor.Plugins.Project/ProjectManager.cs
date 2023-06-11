using Dota2Modding.Common.Models.Addon;
using Dota2Modding.VisualEditor.Plugins.Project.Abstraction.Events;
using EmberKernel.Plugins.Components;
using EmberKernel.Services.EventBus;
using EmberKernel.Services.EventBus.Handlers;
using Microsoft.WindowsAPICodePack.Dialogs;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using Microsoft.Extensions.Logging;
using Serilog.Core;
using Dota2Modding.Common.Models.Parser;
using Dota2Modding.Common.Models.Project;
using System.ComponentModel;
using System.Runtime.CompilerServices;
using Dota2Modding.VisualEditor.Plugins.Project.Components;
using EmberKernel.Services.UI.Mvvm.ViewComponent.Window;
using HandyControl.Controls;
using HandyControl.Tools.Extension;
using Dota2Modding.VisualEditor.Plugins.Project.ViewModel;
using System.Diagnostics;

namespace Dota2Modding.VisualEditor.Plugins.Project
{
    public class ProjectManager : EmberKernel.Plugins.Components.IComponent, IEventHandler<ProjectSelectedEvent>, INotifyPropertyChanged
    {
        public DotaAbilitiesViewModel AbilitiesViewModel { get; private set; }
        public DotaHeroesViewModel HeroViewModel { get; private set; }
        public DotaProject DotaProject { get; private set; }

        private readonly IEventBus eventBus;
        private readonly ILogger<ProjectManager> logger;
        private readonly IWindowManager windowManager;

        public event PropertyChangedEventHandler? PropertyChanged;
        protected virtual void OnPropertyChanged([CallerMemberName] string propertyName = null)
        {
            PropertyChanged?.Invoke(this, new PropertyChangedEventArgs(propertyName));
        }

        private bool loading;
        public bool Loading
        {
            get => this.loading;
            set
            {
                this.loading = value;
                OnPropertyChanged();
            }
        }

        private string phase;

        public string Phase
        {
            get => phase;
            set
            {
                phase = value;
                OnPropertyChanged();
            }
        }

        private string message;

        public string Message
        {
            get => message;
            set
            {
                message = value;
                OnPropertyChanged();
            }
        }

        private int maxStep;

        public int MaxStep
        {
            get => maxStep;
            set
            {
                maxStep = value;
                OnPropertyChanged();
            }
        }

        private int currentStep;
        private Dialog dialog;

        public int CurrentStep
        {
            get => currentStep;
            set
            {
                currentStep = value;
                OnPropertyChanged();
            }
        }


        public ProjectManager(IEventBus eventBus, ILogger<ProjectManager> logger, IWindowManager windowManager)
        {
            this.eventBus = eventBus;
            this.logger = logger;
            this.windowManager = windowManager;
        }

        public void Dispose()
        {
            if (DotaProject is not null)
                DotaProject.LoadingStatusUpdated -= DotaProject_LoadingStatusUpdated;
            GC.SuppressFinalize(this);
        }

        private void DotaProject_LoadingStatusUpdated(string phase, string message, int maxStep, int currentStep)
        {
            Phase = phase;
            Message = message;
            MaxStep = maxStep;
            CurrentStep = currentStep;

            logger.LogInformation($"({currentStep}/{maxStep})Loading {phase}: {message}");
        }

        public async ValueTask CloseProject()
        {
            await eventBus.Publish(new ProjectUnloadEvent(), default);
            this.AbilitiesViewModel = null!;
            this.HeroViewModel = null!;
            using var proj = this.DotaProject;
            proj.LoadingStatusUpdated -= DotaProject_LoadingStatusUpdated;
            this.DotaProject = null!;
            await windowManager.BeginUIThreadScope(() =>
            {
                OnPropertyChanged(nameof(DotaProject));
                OnPropertyChanged(nameof(AbilitiesViewModel));
                OnPropertyChanged(nameof(HeroViewModel));
            });
            _ = Task.Run(() =>
            {
                foreach (var gen in Enumerable.Range(0, GC.MaxGeneration))
                {
                    GC.Collect(gen, GCCollectionMode.Forced, true);
                    GC.WaitForPendingFinalizers();
                    GC.WaitForFullGCComplete();
                }
            });
        }

        public async ValueTask Handle(ProjectSelectedEvent @event)
        {
            var file = @event.SelectedAddonInfoFile;
            try
            {
                await windowManager.BeginUIThreadScope(() =>
                {
                    if (this.dialog is null)
                    {
                        this.dialog = Dialog.Show<LoadingOverride>();
                        dialog.DataContext = this;
                        dialog.Initialize<ProjectManager>(_ => { });
                    }
                });
                Stopwatch sw = Stopwatch.StartNew();
                DotaProject = new(file, new Dota2Locator());

                DotaProject.LoadingStatusUpdated += DotaProject_LoadingStatusUpdated;

                int extraStep = 3;

                DotaProject.InitBasePackages(extraStep);

                DotaProject_LoadingStatusUpdated("UI", $"Preparing UI for {DotaProject.Heroes.Mapping.Count} heroes", DotaProject.InitStep + extraStep, DotaProject.InitStep + 1);
                HeroViewModel = new(DotaProject);
                DotaProject_LoadingStatusUpdated("UI", $"Preparing UI for {DotaProject.Abilities.Mapping.Count} abilities", DotaProject.InitStep + extraStep, DotaProject.InitStep + 2);
                AbilitiesViewModel = new(DotaProject);
                DotaProject_LoadingStatusUpdated("UI", $"Done", DotaProject.InitStep + extraStep, DotaProject.InitStep + 3);
                Loading = false;
                sw.Stop();
                logger.LogInformation($"Load project took {sw.Elapsed.TotalSeconds} seconds");

                await eventBus.Publish(new ProjectLoadedEvent()
                {
                    WorkingDirectory = DotaProject.WorkingDirectory,
                    AddonInfoFile = file,
                }, default);
                await windowManager.BeginUIThreadScope(() =>
                {
                    dialog.Close();
                    OnPropertyChanged(nameof(DotaProject));
                    OnPropertyChanged(nameof(Loading));
                });
            }
            catch (Exception e)
            {
                var errorDialog = new TaskDialog
                {
                    StartupLocation = TaskDialogStartupLocation.CenterOwner,
                    Text = e.Message,
                    StandardButtons = TaskDialogStandardButtons.Ok,
                    Icon = TaskDialogStandardIcon.Error,
                };
                errorDialog.Show();
            }
        }
    }
}
