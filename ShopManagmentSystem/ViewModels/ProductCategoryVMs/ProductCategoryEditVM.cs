using System.ComponentModel.DataAnnotations;

namespace ShopManagmentSystem.ViewModels.ProductCategoryVMs
{
    public class ProductCategoryEditVM
    { 

        [Required]
        public string? Name { get; set; }
        [Required]
        public double Bonus { get; set; }

    }
}
