using System.ComponentModel.DataAnnotations;

namespace ShopManagmentSystem.ViewModels.BrandVMs
{
    public class BrandCreateVM
    {
        [Required]
        public string BrandName { get; set; }
    }
}
