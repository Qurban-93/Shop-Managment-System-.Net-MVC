using ShopManagmentSystem.Models.Base;

namespace ShopManagmentSystem.Models
{
    public class DisplacementProduct : BaseEntity
    {
        public int ProductId { get; set; }
        public Product? Product { get; set; }
        public int DisplacementId { get; set; }
        public Displacement? Displacement { get; set; }
    }
}
