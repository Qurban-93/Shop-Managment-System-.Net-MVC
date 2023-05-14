using ShopManagmentSystem.Models.Base;

namespace ShopManagmentSystem.Models
{
    public class ProductModel : BaseEntity
    {
        public int? BrandId { get; set; }
        public Brand Brand { get; set; }
        public int? ProductCategoryId { get; set; }
        public ProductCategory ProductCategory { get; set; }
        public string ModelName { get; set; } = null!;
        public double ModelPrice { get; set; }     
        public List<Product>? Products { get; set; }

    }
}
