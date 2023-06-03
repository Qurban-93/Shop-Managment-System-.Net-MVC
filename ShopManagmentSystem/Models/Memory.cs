using ShopManagmentSystem.Models.Base;

namespace ShopManagmentSystem.Models
{
    public class Memory : BaseEntity
    {
        public string? MemoryCapacity { get; set; }
        public List<Product>? Products { get; set; }

    }
}
