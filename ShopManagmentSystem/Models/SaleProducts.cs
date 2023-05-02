using ShopManagmentSystem.Models.Base;

namespace ShopManagmentSystem.Models
{
    public class SaleProducts
    {   
        public int Id { get; set; }
        public int ProductId { get; set; }
        public Product Product { get; set; }
        public int SaleId { get; set; }
        public Sale Sale { get; set; }
    }
}
