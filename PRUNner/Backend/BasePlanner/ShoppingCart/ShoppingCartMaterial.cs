using System;
using PRUNner.Backend.Data;
using ReactiveUI;
using ReactiveUI.Fody.Helpers;

namespace PRUNner.Backend.BasePlanner.ShoppingCart
{
    public class ShoppingCartMaterial : ReactiveObject
    {
        public MaterialData Material { get; }
        [Reactive] public int TotalAmount { get; set; }
        [Reactive] public int RemainingAmount { get; private set; }

        private int _availableAmount;
        public int AvailableAmount
        {
            get => _availableAmount;
            set
            {
                _availableAmount = value;
                RemainingAmount = Math.Max(0, TotalAmount - _availableAmount);
            }
        }

        public ShoppingCartMaterial(MaterialData material)
        {
            Material = material;
        }
    }
}