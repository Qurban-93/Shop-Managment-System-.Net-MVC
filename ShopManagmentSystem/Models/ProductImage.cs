using ShopManagmentSystem.Models.Base;

namespace ShopManagmentSystem.Models
{
    public class ProductImage : BaseEntity
    {
        public int ProductId { get; set; }
        public string ImageUrl { get; set; }
    }
}
