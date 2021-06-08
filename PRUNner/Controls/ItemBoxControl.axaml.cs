using System;
using System.Collections.Generic;
using Avalonia;
using Avalonia.Controls;
using Avalonia.Markup.Xaml;
using Avalonia.Media;
using PRUNner.Backend.Data;
using PRUNner.Backend.Data.Components;
using PRUNner.Backend.Data.Enums;

namespace PRUNner.Controls
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
                Background.StartPoint = RelativePoint.BottomRight;
                
                Foreground = new SolidColorBrush(foreground);
            }
        }

        public static readonly Dictionary<MaterialCategory, ColorPair> MaterialColors = new()
        {
            {MaterialCategory.AgriculturalProducts, new ColorPair(new Color(255, 92, 18, 18), new Color(255, 117, 43, 43), new Color(255, 219, 145, 145))},
            {MaterialCategory.Alloys, new ColorPair(new Color(255, 77, 77, 77), new Color(255, 102, 102, 102), new Color(255, 204, 204, 204))},
            {MaterialCategory.Chemicals, new ColorPair(new Color(255, 183, 46, 91), new Color(255, 208, 71, 116), new Color(255, 255, 173, 218))},
            {MaterialCategory.ConstructionMaterials, new ColorPair(new Color(255, 24, 91, 211), new Color(255, 49, 116, 236), new Color(255, 151, 218, 255))},
            {MaterialCategory.ConstructionParts, new ColorPair(new Color(255, 35, 30, 68), new Color(255, 60, 55, 93), new Color(255, 162, 157, 195))},
            {MaterialCategory.ConstructionPrefabs, new ColorPair(new Color(255, 54, 54, 54), new Color(255, 79, 79, 79), new Color(255, 181, 181, 181))},
            {MaterialCategory.ConsumablesBasic, new ColorPair(new Color(255, 73, 85, 97), new Color(255, 98, 110, 122), new Color(255, 200, 212, 224))},
            {MaterialCategory.ConsumablesLuxury, new ColorPair(new Color(255, 9, 15, 15), new Color(255, 34, 40, 40), new Color(255, 136, 142, 142))},
            {MaterialCategory.Drones, new ColorPair(new Color(255, 91, 46, 183), new Color(255, 116, 71, 208), new Color(255, 218, 173, 255))},
            {MaterialCategory.ElectronicDevices, new ColorPair(new Color(255, 86, 20, 147), new Color(255, 111, 45, 172), new Color(255, 213, 147, 255))},
            {MaterialCategory.ElectronicParts, new ColorPair(new Color(255, 92, 30, 122), new Color(255, 117, 55, 147), new Color(255, 219, 157, 249))},
            {MaterialCategory.ElectronicPieces, new ColorPair(new Color(255, 73, 85, 97), new Color(255, 98, 110, 122), new Color(255, 200, 212, 224))},
            {MaterialCategory.ElectronicSystems, new ColorPair(new Color(255, 49, 24, 7), new Color(255, 74, 49, 32), new Color(255, 176, 151, 134))},
            {MaterialCategory.Elements, new ColorPair(new Color(255, 91, 46, 183), new Color(255, 116, 71, 208), new Color(255, 218, 173, 255))},
            {MaterialCategory.EnergySystems, new ColorPair(new Color(255, 51, 24, 216), new Color(255, 76, 49, 241), new Color(255, 178, 151, 255))},
            {MaterialCategory.Fuels, new ColorPair(new Color(255, 30, 123, 30), new Color(255, 55, 148, 55), new Color(255, 157, 250, 157))},
            {MaterialCategory.Gases, new ColorPair(new Color(255, 67, 77, 87), new Color(255, 92, 102, 112), new Color(255, 194, 204, 214))},
            {MaterialCategory.Liquids, new ColorPair(new Color(255, 80, 41, 23), new Color(255, 105, 66, 48), new Color(255, 207, 168, 150))},
            {MaterialCategory.MedicalEquipment, new ColorPair(new Color(255, 9, 15, 15), new Color(255, 34, 40, 40), new Color(255, 136, 142, 142))},
            {MaterialCategory.Metals, new ColorPair(new Color(255, 16, 92, 87), new Color(255, 41, 117, 112), new Color(255, 143, 219, 214))},
            {MaterialCategory.Minerals, new ColorPair(new Color(255, 73, 85, 97), new Color(255, 98, 110, 122), new Color(255, 200, 212, 224))},
            {MaterialCategory.Ores, new ColorPair(new Color(255, 57, 95, 96), new Color(255, 82, 120, 121), new Color(255, 184, 222, 223))},
            {MaterialCategory.Plastics, new ColorPair(new Color(255, 26, 60, 162), new Color(255, 51, 85, 187), new Color(255, 153, 187, 255))},
            {MaterialCategory.ShipEngines, new ColorPair(new Color(255, 14, 57, 14), new Color(255, 39, 82, 39), new Color(255, 141, 184, 141))},
            {MaterialCategory.ShipKits, new ColorPair(new Color(255, 29, 36, 16), new Color(255, 54, 61, 41), new Color(255, 156, 163, 143))},
            {MaterialCategory.ShipParts, new ColorPair(new Color(255, 59, 45, 148), new Color(255, 84, 70, 173), new Color(255, 186, 172, 255))},
            {MaterialCategory.ShipShields, new ColorPair(new Color(255, 132, 82, 34), new Color(255, 157, 107, 59), new Color(255, 255, 209, 161))},
            {MaterialCategory.SoftwareComponents, new ColorPair(new Color(255, 67, 77, 87), new Color(255, 92, 102, 112), new Color(255, 194, 204, 214))},
            {MaterialCategory.SoftwareSystems, new ColorPair(new Color(255, 26, 60, 162), new Color(255, 51, 85, 187), new Color(255, 153, 187, 255))},
            {MaterialCategory.SoftwareTools, new ColorPair(new Color(255, 6, 6, 29), new Color(255, 31, 31, 54), new Color(255, 133, 133, 156))},
            {MaterialCategory.Textiles, new ColorPair(new Color(255, 80, 41, 23), new Color(255, 105, 66, 48), new Color(255, 207, 168, 150))},
            {MaterialCategory.UnitPrefabs, new ColorPair(new Color(255, 59, 45, 148), new Color(255, 84, 70, 173), new Color(255, 186, 172, 255))},
            {MaterialCategory.Utility, new ColorPair(new Color(255, 54, 54, 54), new Color(255, 79, 79, 79), new Color(255, 181, 181, 181))},
        };
        
        public static readonly ColorPair BuildingColorPair = new(new Color(255, 52, 140, 160), new Color(255, 77, 165, 185), new Color(255, 179, 255, 255));
        
        private readonly Border _frame; 
        private readonly Border _numberFrame; 
        private readonly TextBlock _itemName; 
        private readonly TextBlock _number; 
        
        public ItemBoxControl()
        {
            InitializeComponent();

            _frame = this.FindControl<Border>("Frame");
            _numberFrame = this.FindControl<Border>("NumberFrame");
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
                    _numberFrame.IsVisible = false;
                    _number.IsVisible = false;
                    break;
                case ResourceData resourceData:
                    _frame.Background = MaterialColors[resourceData.Material.Category].Background;
                    _itemName.Foreground = MaterialColors[resourceData.Material.Category].Foreground;
                    _itemName.Text = resourceData.Material.Ticker;
                    _numberFrame.IsVisible = true;
                    _number.IsVisible = true;
                    _number.Text = Math.Round(resourceData.CalculateDailyProduction(1)).ToString("F0");
                    break;
                case MaterialIO materialIO:
                    _frame.Background = MaterialColors[materialIO.Material.Category].Background;
                    _itemName.Foreground = MaterialColors[materialIO.Material.Category].Foreground;
                    _itemName.Text = materialIO.Material.Ticker;
                    _numberFrame.IsVisible = true;
                    _number.IsVisible = true;
                    _number.Text = materialIO.Amount.ToString();
                    break;
                case BuildingData buildingData:
                    _frame.Background = BuildingColorPair.Background;
                    _itemName.Foreground = BuildingColorPair.Foreground;
                    _itemName.Text = buildingData.Ticker;
                    _numberFrame.IsVisible = false;
                    _number.IsVisible = false;
                    break;
            }
        }

        private void InitializeComponent()
        {
            AvaloniaXamlLoader.Load(this);
        }
    }
}