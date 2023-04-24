using ShopManagmentSystem.Models.Base;

namespace ShopManagmentSystem.Models
{
    public class Brand : BaseEntity
    {
        public string BrandName { get; set; }
        public List<Product>? Products { get; set; }
    }
}
