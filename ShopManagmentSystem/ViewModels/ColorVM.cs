using System.ComponentModel.DataAnnotations;

namespace ShopManagmentSystem.ViewModels
{
    public class ColorVM
    {
        [Required]
        public string ColorName { get; set; }
    }
}
