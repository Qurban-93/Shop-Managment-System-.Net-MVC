using System.ComponentModel.DataAnnotations;

namespace ShopManagmentSystem.ViewModels.AccountVMs
{
    public class LoginVM
    {
        [Required, StringLength(100)]
        public string? UserName { get; set; }

        [Required, DataType(DataType.Password)]
        public string? Password { get; set; }

        public bool RememmberMe { get; set; }
    }
}
