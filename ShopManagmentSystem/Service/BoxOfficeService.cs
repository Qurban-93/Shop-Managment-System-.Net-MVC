using Microsoft.EntityFrameworkCore;
using ShopManagmentSystem.DAL;
using ShopManagmentSystem.Models;
using ShopManagmentSystem.ViewModels;

namespace ShopManagmentSystem.Service
{
    public class BoxOfficeService : IBoxOfficeService
    {
        private readonly AppDbContext _context;
        public BoxOfficeService(AppDbContext context)
        {
            _context = context;
        }
        public List<Money> GetAll(DateTime dateFrom, AppUser user, DateTime dateTo )
        {          
            List<Money> money =  _context.Moneys
                .Include(m=>m.Sale)
                .Include(m=>m.Refund)
                .Include(m => m.Expenses)
                .Where(s => s.CreateDate > dateFrom 
            && s.CreateDate < dateTo.AddHours(23)
            && s.BranchId == user.BranchId).ToList();           

            return money.OrderBy(b => b.CreateDate).ToList();
        }

        public  List<Money> GetAll( AppUser user , DateTime dateTo)
        {         
            List<Money> money =  _context.Moneys
                .Include(m => m.Sale)
                .Include(m => m.Refund)
                .Include(m => m.Expenses)
                .Where(s => s.CreateDate < dateTo.AddHours(23)
            && s.BranchId == user.BranchId).ToList();
           
            return money.OrderBy(b => b.CreateDate).ToList();
        }

        public  List<Money> GetAll(DateTime dateFrom, AppUser user)
        {
            
            List<Money> money =  _context.Moneys
                .Include(m => m.Sale)
                .Include(m => m.Refund)
                .Include(m => m.Expenses)
                .Where(s => s.CreateDate > dateFrom
            && s.BranchId == user.BranchId).ToList();
            
            return money.OrderBy(b => b.CreateDate).ToList();
        }

        public List<Money> GetAll(AppUser user)
        {
            
            List<Money> money = _context.Moneys
                .Include(m => m.Sale)
                .Include(m => m.Refund)
                .Include(m => m.Expenses)
                .Where(s => s.CreateDate > DateTime.Today
            && s.BranchId == user.BranchId).ToList();
           
            return money.OrderBy(b => b.CreateDate).ToList();
        }


    }
}
