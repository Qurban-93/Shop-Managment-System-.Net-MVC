using ShopManagmentSystem.Models.Base;

namespace ShopManagmentSystem.Models
{
    public class ExpensesCategory : BaseEntity
    {
        public string Name { get; set; }
        public List<Expenses> Expensess { get; set; }
    }
}
