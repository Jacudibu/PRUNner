using System;
using PRUNner.Backend.Data;
using ReactiveUI;
using ReactiveUI.Fody.Helpers;

namespace PRUNner.Backend.BasePlanner.ShoppingCart
{
    public class ShoppingCartMaterial : ReactiveObject
    {
        public MaterialData Material { get; }
        [Reactive] public int TotalAmount { get; internal set; }
        [Reactive] public int RemainingAmount { get; private set; }

        private int _inventory;
        public int Inventory
        {
            get => _inventory;
            set
            {
                _inventory = value;
                RemainingAmount = Math.Max(0, TotalAmount - _inventory);
            }
        }

        public ShoppingCartMaterial(MaterialData material)
        {
            Material = material;
        }
    }
}