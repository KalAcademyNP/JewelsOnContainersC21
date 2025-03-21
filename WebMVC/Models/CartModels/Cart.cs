﻿namespace WebMVC.Models.CartModels
{
    public class Cart
    {
        public string BuyerId { get; set; }
        public List<CartItem> Items { get; set; } = new List<CartItem>();

        public decimal Total()
        {
            return Math.Round(Items.Sum(x => x.UnitPrice * x.Quantity), 2);
        }
    }
}
