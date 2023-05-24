using ShopManagmentSystem.Models;
using System.ComponentModel.DataAnnotations;

namespace ShopManagmentSystem.ViewModels
{
    public class DisplacementCreateVM
    {      
        public List<Product>? Products { get; set; }         
        public int SenderId { get; set; }
        [Required]
        public int DestinationId { get; set; }
    }
}
