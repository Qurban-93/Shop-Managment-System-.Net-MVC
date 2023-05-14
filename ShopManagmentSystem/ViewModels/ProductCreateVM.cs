using System.ComponentModel.DataAnnotations;

namespace ShopManagmentSystem.ViewModels
{
    public class ProductCreateVM
    {
        [Required]
        public double CostPrice { get; set; }
        [Required]
        public string Series { get; set; } = null!;
        [Required]
        public int ProductCategoryId { get; set; }
        [Required]
        public int? BrandId { get; set; }
        [Required]
        public int ColorId { get; set; }      
        [Required]
        public int? ProductModelId { get; set; }
    }
}
