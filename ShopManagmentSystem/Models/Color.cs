using ShopManagmentSystem.Models.Base;

namespace ShopManagmentSystem.Models
{
    public class Color : BaseEntity
    {
        public string ColorName { get; set; } = null!;
        public List<Product> Products { get; set; }
    }
}
