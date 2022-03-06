using System;
using System.Collections.Generic;
using System.Text;
using Avalonia;
using Avalonia.Controls;
using Avalonia.Markup.Xaml;
using Avalonia.Media;
using PRUNner.Backend.BasePlanner;
using PRUNner.Backend.Data;
using PRUNner.Backend.Data.Components;
using PRUNner.Backend.Data.Enums;

namespace PRUNner.App.Controls
{
    public class ItemBoxControl : UserControl
    {
        public class ColorPair
        {
            public readonly LinearGradientBrush Background;
            public readonly SolidColorBrush Foreground;

            public ColorPair(Color backgroundFrom, Color backgroundTo, Color foreground)
            {
                Background = new LinearGradientBrush();
                Background.GradientStops.Add(new GradientStop(backgroundFrom, 0));
                Background.GradientStops.Add(new GradientStop(backgroundTo, 1));
                Background.StartPoint = RelativePoint.TopLeft;
                Background.EndPoint = RelativePoint.BottomRight;
                Background.SpreadMethod = GradientSpreadMethod.Pad;
                
                Foreground = new SolidColorBrush(foreground);
            }

            public ColorPair(byte ar, byte ag, byte ab, byte br, byte bg, byte bb, byte cr, byte cg, byte cb) : this(
                    new Color(255, ar, ag, ab), 
                    new Color(255, br, bg, bb),
                    new Color(255, cr, cg, cb))
            { }
        }

        public static readonly Dictionary<MaterialCategory, ColorPair> MaterialColors = new()
        {
            { MaterialCategory.AgriculturalProducts, new ColorPair(92, 18, 18, 117, 43, 43, 219, 145, 145) },
            { MaterialCategory.Alloys, new ColorPair(123, 76, 30, 148, 101, 55, 250, 203, 157) },
            { MaterialCategory.Chemicals, new ColorPair(183, 46, 91, 208, 71, 116, 255, 173, 218) },
            { MaterialCategory.ConstructionMaterials, new ColorPair(24, 91, 211, 49, 116, 236, 151, 218, 255) },
            { MaterialCategory.ConstructionParts, new ColorPair(41, 77, 107, 66, 102, 132, 168, 204, 234) },
            { MaterialCategory.ConstructionPrefabs, new ColorPair(15, 30, 98, 40, 55, 123, 142, 157, 225) },
            { MaterialCategory.ConsumablesBasic, new ColorPair(149, 46, 46, 174, 71, 71, 255, 173, 173) },
            { MaterialCategory.ConsumablesLuxury, new ColorPair(136, 24, 39, 161, 49, 64, 255, 151, 166) },
            { MaterialCategory.Drones, new ColorPair(140, 52, 18, 165, 77, 43, 255, 179, 145) },
            { MaterialCategory.ElectronicDevices, new ColorPair(86, 20, 147, 111, 45, 172, 213, 147, 255) },
            { MaterialCategory.ElectronicParts, new ColorPair(91, 46, 183, 116, 71, 208, 218, 173, 255) },
            { MaterialCategory.ElectronicPieces, new ColorPair(119, 82, 189, 144, 107, 214, 246, 209, 255) },
            { MaterialCategory.ElectronicSystems, new ColorPair(51, 26, 76, 76, 51, 101, 178, 153, 203) },
            { MaterialCategory.Elements, new ColorPair(61, 46, 31, 86, 71, 57, 188, 173, 159) },
            { MaterialCategory.EnergySystems, new ColorPair(21, 62, 39, 46, 87, 64, 148, 189, 166) },
            { MaterialCategory.Fuels, new ColorPair(30, 123, 30, 55, 148, 55, 157, 250, 157) },
            { MaterialCategory.Gases, new ColorPair(0, 105, 107, 25, 130, 132, 127, 232, 234) },
            { MaterialCategory.Liquids, new ColorPair(114, 164, 202, 139, 189, 227, 241, 255, 255) },
            { MaterialCategory.MedicalEquipment, new ColorPair(85, 170, 85, 110, 195, 110, 212, 255, 212) },
            { MaterialCategory.Metals, new ColorPair(54, 54, 54, 79, 79, 79, 181, 181, 181) },
            { MaterialCategory.Minerals, new ColorPair(153, 113, 73, 178, 138, 98, 255, 240, 200) },
            { MaterialCategory.Ores, new ColorPair(82, 87, 97, 107, 112, 122, 209, 214, 224) },
            { MaterialCategory.Plastics, new ColorPair(121, 31, 60, 146, 56, 85, 248, 158, 187) },
            { MaterialCategory.ShipEngines, new ColorPair(153, 41, 0, 178, 66, 25, 255, 168, 127) },
            { MaterialCategory.ShipKits, new ColorPair(153, 84, 9, 178, 109, 25, 255, 211, 127) },
            { MaterialCategory.ShipParts, new ColorPair(153, 99, 0, 178, 124, 25, 255, 226, 127) },
            { MaterialCategory.ShipShields, new ColorPair(224, 131, 0, 249, 156, 25, 255, 255, 127) },
            { MaterialCategory.SoftwareComponents, new ColorPair(136, 121, 47, 161, 146, 72, 255, 248, 174) },
            { MaterialCategory.SoftwareSystems, new ColorPair(60, 53, 5, 85, 78, 30, 187, 180, 132) },
            { MaterialCategory.SoftwareTools, new ColorPair(129, 98, 19, 154, 123, 44, 255, 225, 146) },
            { MaterialCategory.Textiles, new ColorPair(82, 90, 33, 107, 115, 58, 209, 217, 160) },
            { MaterialCategory.UnitPrefabs, new ColorPair(29, 27, 28, 54, 52, 53, 156, 154, 155) },
            { MaterialCategory.Utility, new ColorPair(161, 148, 136, 186, 173, 161, 255, 255, 255) },
        };
        
        public static readonly ColorPair BuildingColorPair = new(52, 140, 160, 77, 165, 185, 179, 255, 255);

        private readonly Border _frame; 
        private readonly Border _numberFrame; 
        private readonly TextBlock _toolTip;
        private readonly TextBlock _itemName; 
        private readonly TextBlock _number; 
        
        public ItemBoxControl()
        {
            InitializeComponent();

            _frame = this.FindControl<Border>("Frame");
            _numberFrame = this.FindControl<Border>("NumberFrame");
            _toolTip = this.FindControl<TextBlock>("ToolTip");
            _itemName = this.FindControl<TextBlock>("ItemName");
            _number = this.FindControl<TextBlock>("Number");
        }
        
        protected override void OnDataContextChanged(EventArgs e)
        {
            switch (DataContext)
            {
                case MaterialData materialData:
                    _frame.Background = MaterialColors[materialData.Category].Background;
                    _itemName.Foreground = MaterialColors[materialData.Category].Foreground;
                    _itemName.Text = materialData.Ticker;
                    _toolTip.Text = GetMaterialTooltip(materialData);
                    _numberFrame.IsVisible = false;
                    _number.IsVisible = false;
                    break;
                case ResourceData resourceData:
                    _frame.Background = MaterialColors[resourceData.Material.Category].Background;
                    _itemName.Foreground = MaterialColors[resourceData.Material.Category].Foreground;
                    _itemName.Text = resourceData.Material.Ticker;
                    _toolTip.Text = GetMaterialTooltip(resourceData.Material);
                    _numberFrame.IsVisible = true;
                    _number.IsVisible = true;
                    _number.Text = Math.Round(resourceData.CalculateDailyProduction(1)).ToString("F0");
                    break;
                case MaterialIO materialIO:
                    _frame.Background = MaterialColors[materialIO.Material.Category].Background;
                    _itemName.Foreground = MaterialColors[materialIO.Material.Category].Foreground;
                    _itemName.Text = materialIO.Material.Ticker;
                    _toolTip.Text = GetMaterialTooltip(materialIO.Material);
                    _numberFrame.IsVisible = true;
                    _number.IsVisible = true;
                    _number.Text = materialIO.Amount.ToString();
                    break;
                case BuildingData buildingData:
                    _frame.Background = BuildingColorPair.Background;
                    _itemName.Foreground = BuildingColorPair.Foreground;
                    _itemName.Text = buildingData.Ticker;
                    _toolTip.Text = buildingData.Name;
                    _numberFrame.IsVisible = false;
                    _number.IsVisible = false;
                    break;
                case PlanetProductionRow planetProductionRow:
                    _frame.Background = MaterialColors[planetProductionRow.Material.Category].Background;
                    _itemName.Foreground = MaterialColors[planetProductionRow.Material.Category].Foreground;
                    _itemName.Text = planetProductionRow.Material.Ticker;
                    _toolTip.Text = GetMaterialTooltip(planetProductionRow.Material);
                    _numberFrame.IsVisible = true;
                    _number.IsVisible = true;

                    _number.Text = planetProductionRow.Balance.ToString("F1");
                    
                    if (planetProductionRow.Balance < 0)
                    {
                        _number.Foreground = SolidColorBrush.Parse("red");
                    }
                    break;
            }

            _numberFrame.Width = 6 + _number.Text.Length * 5;
            if (_number.Text.Contains('.'))
            {
                _numberFrame.Width -= 4;
            }

            if (_number.Text.Contains('-'))
            {
                _numberFrame.Width -= 3;
            }
        }

        private readonly StringBuilder _tooltipBuilder = new();
        private string GetMaterialTooltip(MaterialData materialData)
        {
            _tooltipBuilder.Append(materialData.Name);
            _tooltipBuilder.Append(" (min < avg > max)\n");
            AppendPriceData(_tooltipBuilder, nameof(MaterialPriceData.AI1), materialData.PriceData.AI1);
            AppendPriceData(_tooltipBuilder, nameof(MaterialPriceData.CI1), materialData.PriceData.CI1);
            AppendPriceData(_tooltipBuilder, nameof(MaterialPriceData.IC1), materialData.PriceData.IC1);
            AppendPriceData(_tooltipBuilder, nameof(MaterialPriceData.NC1), materialData.PriceData.NC1);
            var result = _tooltipBuilder.ToString();
            _tooltipBuilder.Clear();
            return result;
        }

        private void AppendPriceData(StringBuilder sb, string exchangeName, MaterialPriceDataRegional priceData)
        {
            sb.Append(exchangeName);
            sb.Append(" – ");
            sb.Append(priceData.Ask?.ToString("F2") ?? "-");
            sb.Append(" < ");
            sb.Append(priceData.Average.ToString("F2"));
            sb.Append(" > ");
            sb.Append(priceData.Bid?.ToString("F2") ?? "-");
            sb.Append('\n');
        }
        
        private void InitializeComponent()
        {
            AvaloniaXamlLoader.Load(this);
        }
    }
}