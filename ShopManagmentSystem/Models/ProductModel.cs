using ShopManagmentSystem.Models.Base;

namespace ShopManagmentSystem.Models
{
    public class ProductModel : BaseEntity
    {
        public string ModelName { get; set; } = null!;
        public double ModelPrice { get; set; }     
        public List<Product>? Products { get; set; }

    }
}
