using ShopManagmentSystem.Models.Base;

namespace ShopManagmentSystem.Models
{
    public class Expenses : BaseEntity
    {
        public double Amount { get; set; }
        public string? Descpription { get; set; }
        public int ExpensesCategoryId { get; set; }

    }
}
