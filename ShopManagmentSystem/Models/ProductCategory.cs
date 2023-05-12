using ShopManagmentSystem.Models.Base;

namespace ShopManagmentSystem.Models
{
    public class ProductCategory : BaseEntity
    {
        public string Name { get; set; } = null!;
        public List<Product>? Products { get; set; }
        public double Bonus { get; set; }

    }
}
