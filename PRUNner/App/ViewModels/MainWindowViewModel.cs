using System.Linq;
using System.Text;
using NLog;
using NLog.Targets;
using PRUNner.Backend;
using PRUNner.Backend.Data;
using PRUNner.Backend.PlanetFinder;
using PRUNner.Backend.UserDataParser;
using ReactiveUI.Fody.Helpers;

namespace PRUNner.App.ViewModels
{
    public class MainWindowViewModel : ViewModelBase
    {
        [Reactive] public ViewModelBase ActiveView { get; private set; }

        public readonly EmpireViewModel EmpireViewModel;
        public readonly BasePlannerViewModel BasePlannerViewModel;
        public readonly PlanetFinderViewModel PlanetFinderViewModel;

        [Reactive] public string StatusBar { get; private set; }

        private static readonly Logger Logger = LogManager.GetCurrentClassLogger();
        
        public MainWindowViewModel()
        {
            LogConfigurator.AddTarget(new MethodCallTarget("StatusBar", LogEventAction), LogLevel.Debug);

            DataParser.LoadAndParseFromCache();
            
            EmpireViewModel = new EmpireViewModel(this);
            BasePlannerViewModel = new BasePlannerViewModel();
            PlanetFinderViewModel = new PlanetFinderViewModel();

            ActiveView = PlanetFinderViewModel;

            PlanetFinderSearchResult.OnOpenBasePlanner += PlanetFinderSelectPlanetEvent;
            
            LoadFromDisk();
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

        public void BlameFio()
        {
            Logger.Debug("Not yet implemented. If you want to invalidate your cache, delete your FIOCache folder and restart. " +
                         "Not recommended since downloading planet data takes a while.");
        }
        
        public void SaveToDisk()
        {
            UserDataWriter.Save(EmpireViewModel.Empire);
            Logger.Info("Data successfully saved.");
        }

        public void LoadFromDisk()
        {
            EmpireViewModel.SetEmpire(UserDataReader.Load());
            if (EmpireViewModel.Empire.PlanetaryBases.Count <= 0)
            {
                return;
            }
            
            BasePlannerViewModel.SetActiveBase(EmpireViewModel.Empire.PlanetaryBases.First());
            Logger.Info("Data successfully loaded.");
        }
        
        public void LogEventAction(LogEventInfo info, object[] objects)
        {
            var builder = new StringBuilder();
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
        }
    }
}