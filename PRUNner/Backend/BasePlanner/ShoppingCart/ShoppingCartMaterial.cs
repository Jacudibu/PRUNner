using System;
using PRUNner.Backend.Data;
using ReactiveUI;
using ReactiveUI.Fody.Helpers;

namespace PRUNner.Backend.BasePlanner.ShoppingCart
{
    public class ShoppingCartMaterial : ReactiveObject
    {
        public MaterialData Material { get; }
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

        private void RecalculateRemainingAmount()
        {
            RemainingAmount = Math.Max(0, TotalAmount - _inventory);
        }

        public ShoppingCartMaterial(MaterialData material)
        {
            Material = material;
        }
    }
}