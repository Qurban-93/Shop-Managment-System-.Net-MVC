using ShopManagmentSystem.Models;
using System.ComponentModel.DataAnnotations;

namespace ShopManagmentSystem.ViewModels.RefundVMs
{
    public class RefundOrderVM
    {
        public Sale? Sale { get; set; }
        public Product? Product { get; set; }
        public int SaleId { get; set; }
        public int ProductId { get; set; }
        [Required]
        public string Description { get; set; }
    }
}
