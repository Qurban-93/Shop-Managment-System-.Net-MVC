using ShopManagmentSystem.Models;
using ShopManagmentSystem.ViewModels;

namespace ShopManagmentSystem.Service
{
    public interface IBoxOfficeService
    {
        public List<Money> GetAll(DateTime dateFrom,AppUser user ,DateTime dateTo);
        public List<Money> GetAll(AppUser user, DateTime dateTo);
        public List<Money> GetAll(DateTime dateFrom, AppUser user);
        public List<Money> GetAll(AppUser user);
    }
}
