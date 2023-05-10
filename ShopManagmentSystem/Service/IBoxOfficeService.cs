using ShopManagmentSystem.Models;
using ShopManagmentSystem.ViewModels;

namespace ShopManagmentSystem.Service
{
    public interface IBoxOfficeService
    {
        public List<BoxOfficeVM> GetAll(DateTime dateFrom,AppUser user ,DateTime dateTo);
        public List<BoxOfficeVM> GetAll(AppUser user, DateTime dateTo);
        public List<BoxOfficeVM> GetAll(DateTime dateFrom, AppUser user);
        public List<BoxOfficeVM> GetAll(AppUser user);
    }
}
