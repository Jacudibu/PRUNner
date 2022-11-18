using System;
using PRUNner.Backend.Data;
using PRUNner.Backend.Data.Components;
using ReactiveUI;
using ReactiveUI.Fody.Helpers;

namespace PRUNner.Backend.BasePlanner.ShoppingCart
{
    public class ShoppingCartMaterial : ReactiveObject
    {
        public MaterialData Material { get; }
        private readonly PlanetaryBase _planetaryBase;

        [Reactive] public int RemainingAmount { get; private set; }

        private int _totalAmount;
        private int _inventory;

        public int TotalAmount
        {
            get => _totalAmount;
            internal set
            {
                _totalAmount = value;
                RecalculateRemainingAmount();
                this.RaisePropertyChanged(nameof(TotalAmount));
            }
        }

        public int Inventory
        {
            get => _inventory;
            set
            {
                _inventory = value;
                RecalculateRemainingAmount();
            }
        }

        [Reactive]
        public double TotalCost { get; set; }

        private readonly ObservableAsPropertyHelper<string> formattedTotalCost;
        public string FormattedTotalCost => formattedTotalCost.Value;
        private void RecalculateRemainingAmount()
        {
            RemainingAmount = Math.Max(0, TotalAmount - _inventory);
            TotalCost = RemainingAmount * Material.PriceData.GetPrice(true, _planetaryBase);
        }

        public ShoppingCartMaterial(MaterialData material, PlanetaryBase planetaryBase)
        {
            Material = material;
            _planetaryBase = planetaryBase;

            this.WhenAny(x => x.TotalCost, v =>
                {
                    switch (Math.Log10(TotalCost))
                    {
                        case double.NegativeInfinity:
                        case < 3:
                            return $"{TotalCost:0.00}";
                        case < 6:
                            return $"{TotalCost / 1000:0.00} K";
                        case double.PositiveInfinity:
                        default:
                            return $"{TotalCost / 1000000:0.00} M";
                    }
                })
                .ToProperty(this, x => x.FormattedTotalCost, out formattedTotalCost);
        }
    }
}