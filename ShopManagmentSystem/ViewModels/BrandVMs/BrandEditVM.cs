using System.ComponentModel.DataAnnotations;

namespace ShopManagmentSystem.ViewModels.BrandVMs
{
    public class BrandEditVM
    {
        [Required]
        public string BrandName { get; set; }
    }
}
