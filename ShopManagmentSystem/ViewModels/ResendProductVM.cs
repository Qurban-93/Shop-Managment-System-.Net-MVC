using System.ComponentModel.DataAnnotations;

namespace ShopManagmentSystem.ViewModels
{
    public class ResendProductVM
    {     
        public int Id { get; set; }
        public string ProductBrand { get; set; }
        public string  Model { get; set; }
        public string Color { get; set; }
        public string  Series { get; set; }
        public string Category { get; set; }

    }
}
