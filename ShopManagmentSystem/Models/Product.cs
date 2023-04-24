using ShopManagmentSystem.Models.Base;

namespace ShopManagmentSystem.Models
{
    public class Product : BaseEntity
    {
        public string Name { get; set; } = null!;
        public double Price { get; set; }
        public double CostPrice { get; set; }
        public string Series { get; set; } = null!;
        public string? Desc { get; set; }
        public int ProductCategoryId { get; set; }
        public ProductCategory ProductCategory { get; set; }
        public int ColorId { get; set; }
        public Color Color { get; set; }
        public int? BrandId { get; set; }
        public Brand Brand { get; set; }
        public List<ProductImage> Images { get; set; }
    }
}
