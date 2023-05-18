using System.ComponentModel.DataAnnotations;

namespace ShopManagmentSystem.ViewModels.ProductModelVMs
{
    public class ProductModelEditVM
    {
        [Required]
        public string ModelName { get; set; } 
        [Required]
        public double ModelPrice { get; set; }
        [Required]
        public int ProductCategoryId { get; set; }
        [Required]
        public int BrandId { get; set; }
    }
}
