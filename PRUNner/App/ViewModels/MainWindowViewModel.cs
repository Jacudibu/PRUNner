using System;
using System.IO;
using System.Linq;
using Avalonia;
using Avalonia.Controls;
using Avalonia.Controls.ApplicationLifetimes;
using Avalonia.Controls.Primitives;
using Avalonia.Styling;
using Avalonia.Themes.Fluent;
using FIOImport;
using NLog;
using NLog.Targets;
using PRUNner.Backend;
using PRUNner.Backend.BasePlanner;
using PRUNner.Backend.Data;
using PRUNner.Backend.PlanetFinder;
using PRUNner.Backend.UserDataParser;
using PRUNner.Backend.Utility;
using ReactiveUI.Fody.Helpers;

namespace PRUNner.App.ViewModels
{
    public class MainWindowViewModel : ViewModelBase
    {
        [Reactive] public ViewModelBase ActiveView { get; private set; }

        public static MainWindowViewModel Instance { get; private set; }
        public readonly EmpireViewModel EmpireViewModel;
        public readonly BasePlannerViewModel BasePlannerViewModel;
        public readonly PlanetFinderViewModel PlanetFinderViewModel;

        [Reactive] public string StatusBar { get; private set; }

        private static readonly Logger Logger = LogManager.GetCurrentClassLogger();
        
        public MainWindowViewModel()
        {
            Instance = this;
            LogConfigurator.AddTarget(new MethodCallTarget("StatusBar", LogEventAction), LogLevel.Debug);

            DataParser.LoadAndParseFromCache();
            
            EmpireViewModel = new EmpireViewModel(this);
            BasePlannerViewModel = new ();
            PlanetFinderViewModel = new ();

            ActiveView = PlanetFinderViewModel;

            PlanetFinderSearchResult.OnOpenBasePlanner += PlanetFinderSelectPlanetEvent;
            
            LoadFromDisk(true);
        }

        public void ViewPlanetFinder()
        {
            ActiveView = PlanetFinderViewModel;
        }

        private void PlanetFinderSelectPlanetEvent(object? sender, PlanetData planet)
        {
            var planetaryBase = EmpireViewModel.StartNewBase(planet);
            BasePlannerViewModel.SetActiveBase(planetaryBase);
            ViewBasePlanner();
        }

        public void ViewEmpire()
        {
            ActiveView = EmpireViewModel;
        }
        
        public void ViewBasePlanner()
        {
            ActiveView = BasePlannerViewModel;
        }
        
        public void UpdatePriceData()
        {
            DataParser.UpdatePriceData();
            EmpireViewModel.Empire.OnPriceDataUpdate();
        }

        public void OpenCommunityToolsDiscord()
        {
            Utils.OpenWebsite("https://discord.gg/2MDR5DYSfY");
        }

        public void OpenGithubSite()
        {
            Utils.OpenWebsite("https://github.com/Jacudibu/PRUNner");
        }
        
        public void OpenApexHandbookCommunityResourceSite()
        {
            Utils.OpenWebsite("https://handbook.apex.prosperousuniverse.com/wiki/community-resources/");
        }

        public void ResetFioCache()
        {
            if (Directory.Exists(FioImporter.CacheFolder))
            {
                Directory.Delete(FioImporter.CacheFolder, true);       
            }
            
            var lifetime = Application.Current?.ApplicationLifetime as IClassicDesktopStyleApplicationLifetime;
            lifetime!.Shutdown();
        }
        
        public void SaveToDisk()
        {
            UserDataWriter.Save(EmpireViewModel.Empire);
            Logger.Info("Data successfully saved.");
        }

        public void LoadFromDisk(bool initialize = false)
        {
            var empire = UserDataReader.Load();
            if (initialize && GlobalSettings.UseDarkMode)
            {
                SetTheme(FluentThemeMode.Dark);
            }

            EmpireViewModel.SetEmpire(empire);
            if (EmpireViewModel.Empire.PlanetaryBases.Count <= 0)
            {
                return;
            }
            
            BasePlannerViewModel.SetActiveBase(EmpireViewModel.Empire.PlanetaryBases.First());
            Logger.Info("Data successfully loaded.");
        }

        public void ToggleTheme()
        {
            if (Application.Current == null)
                return;

            GlobalSettings.UseDarkMode = !GlobalSettings.UseDarkMode;
            SetTheme(GlobalSettings.UseDarkMode ? FluentThemeMode.Dark : FluentThemeMode.Light);
        }

        //I would really prefer a better way of getting the current FluentTheme
        private FluentTheme GetCurrentFluentTheme()
        {
            foreach (IStyle style in Application.Current.Styles)
            {
                if (!(style is FluentTheme)) continue;

                FluentTheme theme = (FluentTheme)style;
                return theme;
            }

            throw new Exception("Unable to locate FluentTheme");
        }

        private void SetTheme(FluentThemeMode fluentThemeMode)
        {
            FluentTheme fluentTheme = GetCurrentFluentTheme();
            fluentTheme.Mode = fluentThemeMode;
        }
        
        public void LogEventAction(LogEventInfo info, object[] objects)
        {
            var builder = ObjectPools.StringBuilderPool.Get();
            builder.Append('[');
            builder.Append(info.TimeStamp.ToString("HH:mm:ss"));
            builder.Append("] ");

            if (info.Level == LogLevel.Debug)
            {
                builder.Append("🏗️ ");
            } 
            else if (info.Level == LogLevel.Info)
            {
                builder.Append("ℹ️ ");
            } 
            else if (info.Level == LogLevel.Warn)
            {
                builder.Append("⚠️ ");
            } 
            else if (info.Level == LogLevel.Error)
            {
                builder.Append("🛑 ");
            }
            
            builder.Append(info.Message);
            StatusBar = builder.ToString();
            
            ObjectPools.StringBuilderPool.Return(builder);
        }

        public void OpenBaseInNewWindow(PlanetaryBase planetaryBase)
        {
            var basePlanner = new BasePlannerViewModel();
            basePlanner.SetActiveBase(planetaryBase);

            var window = new Window();
            window.Content = new ScrollViewer
            {
                HorizontalScrollBarVisibility = ScrollBarVisibility.Visible,
                VerticalScrollBarVisibility = ScrollBarVisibility.Visible,
                AllowAutoHide = false,
                Content = basePlanner
            };
            window.Width = 1800;
            window.Height = 850;
            window.Icon = (Application.Current.ApplicationLifetime as ClassicDesktopStyleApplicationLifetime)?.MainWindow.Icon;
            window.Title = "PRUNner – " + planetaryBase.Planet.Name;
            window.Show();
        }
    }
}