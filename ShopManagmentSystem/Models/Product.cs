using ShopManagmentSystem.Models.Base;

namespace ShopManagmentSystem.Models
{
    public class Product : BaseEntity
    {      
        public double CostPrice { get; set; }
        public string Series { get; set; } = null!;
        public string? Desc { get; set; }
        public int ProductCategoryId { get; set; }
        public ProductCategory? ProductCategory { get; set; }
        public int ColorId { get; set; }
        public Color? Color { get; set; }
        public int? BrandId { get; set; }
        public Brand? Brand { get; set; }      
        public bool IsSold { get; set; }
        public int? BranchId { get; set; }
        public List<SaleProducts>? SaleProducts { get; set; }
        public List<Refund>? Refunds { get; set; }
        public int? ProductModelId { get; set; }
        public ProductModel? ProductModel { get; set; }
        
    }
}
