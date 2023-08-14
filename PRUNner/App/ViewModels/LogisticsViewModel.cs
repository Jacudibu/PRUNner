using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using PRUNner.Backend.Data;
using PRUNner.Backend.BasePlanner;
using ReactiveUI;
using Avalonia.Media.Imaging;
using System.IO;
using PRUNner.App.Controls;
using DynamicData.Kernel;
using PRUNner.Backend.Utility;

namespace PRUNner.App.ViewModels
{
    public class LogisticsViewModel : ViewModelBase
    {
        public LogisticsViewModel()
        {
            Tickers = new Dictionary<string, bool>();
        }

        public LogisticsViewModel(Empire empire)
        {
            this.empire = empire;
            Tickers = new Dictionary<string, bool>();
        }

        public bool GraphvizInstalled { get; set; } = true;

        public IBitmap GraphvizImage { get; set; }

        public Dictionary<string, bool> Tickers { get; set; }

        private List<PlanetaryBase> FindSourcesFor(string ticker)
        {
            //TODO make this more readable / efficient. LINQ?
            return empire.PlanetaryBases.AsList().FindAll(b => b.ProductionTable.Outputs.AsList().Exists(o => o.Material.Ticker == ticker));
        }

        private string GenerateDotGraph()
        {
            StringBuilder strBuilder = new StringBuilder();
            strBuilder.AppendLine("digraph {");
            strBuilder.AppendLine("concentrate=true"); // makes graphs with many edges going in the same direction more readable
            strBuilder.AppendLine("ranksep=2.0");

            // add all bases as nodes
            // names like KI-401b have to be in quotes, so just put quotes around every planet name to be safe
            strBuilder.AppendLine("# one node for each base");
            foreach (var plBase in empire.PlanetaryBases)
            {
                strBuilder.AppendFormat("\"{0}\"", plBase.Planet.Name);
                strBuilder.AppendLine();
            }

            foreach (var plBase in empire.PlanetaryBases)
            {
                strBuilder.AppendLine();
                strBuilder.AppendFormat("# adding edges for {0}", plBase.Planet.Name);
                strBuilder.AppendLine();
                foreach (var product in plBase.ProductionTable.Inputs)
                {
                    if (!Tickers.GetValueOrDefault(product.Material.Ticker, false)) continue;

                    var sources = FindSourcesFor(product.Material.Ticker);
                    foreach (var source in sources)
                    {
                        strBuilder.AppendFormat("\"{0}\" -> \"{1}\" [label={2} color={3} fontcolor={3}]", source.Planet.Name, plBase.Planet.Name, product.Material.Ticker, MaterialToDotColorString(product.Material));
                        strBuilder.AppendLine();
                    }

                    if (sources.Count == 0)
                    {
                        strBuilder.AppendFormat("CX -> \"{0}\" [label={1} color={2} fontcolor={2}]", plBase.Planet.Name, product.Material.Ticker, MaterialToDotColorString(product.Material));
                    }
                }
            }
            strBuilder.AppendLine("}");

            return strBuilder.ToString();
        }

        private static string MaterialToDotColorString(MaterialData material)
        {
            var materialColors = ItemBoxControl.MaterialColors[material.Category];
            return string.Format("\"#{0:X}{1:X}{2:X}\"", materialColors.Foreground.Color.R, materialColors.Foreground.Color.G, materialColors.Foreground.Color.B);
        }

        private void GenerateGraphBitmap()
        {
            const string dotExecutable = "dot";
            const string dotFile = "logistics.dot";
            const string bitmapFile = "logistics.png";

            try
            {
                File.WriteAllText(dotFile, GenerateDotGraph());

                System.Diagnostics.Process dotProcess = new();
                dotProcess.StartInfo.RedirectStandardOutput = true;
                dotProcess.StartInfo.UseShellExecute = false;
                dotProcess.StartInfo.CreateNoWindow = true;
                dotProcess.EnableRaisingEvents = true;
                dotProcess.StartInfo.FileName = dotExecutable;
                dotProcess.StartInfo.Arguments = string.Format(@"{0} -Kdot -Tpng -o {1}", dotFile, bitmapFile);
                dotProcess.Exited += new EventHandler(OnDotProcessExited);
                dotProcess.Start();
            } catch (Exception ex)
            {
                GraphvizInstalled = false;
                this.RaisePropertyChanged(nameof(GraphvizInstalled));
            }
            GetAllInputTickers();
        }

        private void OnDotProcessExited(object sender, EventArgs e)
        {
            const string bitmapFile = "logistics.png";
            Bitmap result = new(bitmapFile);
            //GraphvizImage = result.CreateScaledBitmap(new Avalonia.PixelSize(2000, 1200));
            GraphvizImage = result;
            GraphvizInstalled = true;
            this.RaisePropertyChanged(nameof(GraphvizInstalled));
            this.RaisePropertyChanged(nameof(GraphvizImage));
        }

        public void OnTickerStateChanged(string ticker)
        {
            Tickers[ticker] = !Tickers[ticker];
            GenerateGraphBitmap();
        }

        public void OnChecked()
        {

        }
        public void OnUnchecked()
        {

        }

        public void Update()
        {
            var tickers = new Dictionary<string, bool>();
            // refresh Tickers, keep previous values for existing keys
            foreach (var ticker in GetAllInputTickers())
            {
                tickers[ticker] = Tickers.GetValueOrDefault(ticker, true);
            }
            Tickers = tickers;
            GenerateGraphBitmap();
        }

        public void SetEmpire(Empire empire)
        {
            this.empire = empire;
        }

        private HashSet<string> GetAllInputTickers()
        {
            HashSet<string> result = new HashSet<string>();
            foreach (var row in empire.PlanetaryBases)
            {
                var tickers = from input in row.ProductionTable.Inputs
                        select input.Material.Ticker;
                result.UnionWith(tickers);
            }
            return result;
        }

        public void OpenGraphvizSite()
        {
            Utils.OpenWebsite("https://graphviz.org/download/");
        }

        private Empire empire;
    }
}
