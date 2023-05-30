using System.ComponentModel.DataAnnotations;

namespace ShopManagmentSystem.ViewModels
{
    public class BranchVM
    {
        [Required]
        public string Name { get; set; }
    }
}
