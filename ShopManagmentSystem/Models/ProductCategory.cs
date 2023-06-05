﻿using ShopManagmentSystem.Models.Base;

namespace ShopManagmentSystem.Models
{
    public class ProductCategory : BaseEntity
    {
        public List<ProductModel>? ProductModels { get; set; }
        public string Name { get; set; } = null!;
        public List<Product>? Products { get; set; }
        public double Bonus { get; set; }
        public bool IsDeleted { get; set; }

    }
}
