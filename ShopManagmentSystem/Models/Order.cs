namespace ShopManagmentSystem.Models
{
    public class Order
    {
        public int Id { get; set; }
        public int ProdId { get; set; }
        public string Name { get; set; }
        public string Brand { get; set; }
        public string Color { get; set; }
        public string Series { get; set; }
        public double Price { get; set; }
        public string Category { get; set; }
    }
}
