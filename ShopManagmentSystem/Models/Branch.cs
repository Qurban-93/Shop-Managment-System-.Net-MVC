namespace ShopManagmentSystem.Models
{
    public class Branch
    {
        public int Id { get; set; }
        public string Name { get; set; } = null!;
        public int? AppUserId { get; set; }
        public List<Product>? Products { get; set; }
        public List<Order>? Orders { get; set; }
        public List<Employee>? Employee { get; set; }
        public List<Sale>? Sales { get; set; }
        public List<Money>? Money { get; set; }
    }
}
