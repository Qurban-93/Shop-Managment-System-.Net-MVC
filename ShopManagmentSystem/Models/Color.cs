using ShopManagmentSystem.Models.Base;
using System.ComponentModel;

namespace ShopManagmentSystem.Models
{
    public class Color : BaseEntity
    {
        
        public string ColorName { get; set; } = null!;
        public List<Product> Products { get; set; }
        public bool IsDeleted { get; set; }
    }
}
