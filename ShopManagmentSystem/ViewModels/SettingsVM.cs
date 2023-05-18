using ShopManagmentSystem.Models;

namespace ShopManagmentSystem.ViewModels
{
    public class SettingsVM
    {
        public List<Brand> Brands { get; set; }
        public List<EmployeePosition> EmployeePositions { get; set; }
        public List<Color> Colors { get; set; }
        public List<ExpensesCategory> ExpensesCategories { get; set; }
        public List<ProductCategory> ProductCategories { get; set; }
    }
}
