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
        public List<BoxOfficeVM> GetAll(DateTime dateFrom, AppUser user, DateTime dateTo )
        {
            List<BoxOfficeVM> result = new List<BoxOfficeVM>();
            List<Sale> sales =  _context.Sales.Where(s => s.CreateDate > dateFrom 
            && s.CreateDate < dateTo.AddHours(23)
            && s.BranchId == user.BranchId).ToList();
            List<Refund> refunds = _context.Refunds.Where(r => r.CreateDate > dateFrom
            && r.CreateDate < dateTo.AddHours(23)
            && r.BranchId == user.BranchId).ToList();

            foreach (Sale sale in sales)
            {
                BoxOfficeVM boxOffice = new();
                boxOffice.Date = sale.CreateDate;
                boxOffice.TotalIncoming = sale.TotalPrice;
                boxOffice.CashlessPayment = sale.CashlessPayment;
                boxOffice.Discount = sale.Discount;
                boxOffice.Profit = sale.TotalProfit;
                result.Add(boxOffice);
            }

            foreach (Refund refund in refunds)
            {
                BoxOfficeVM boxOffice = new();
                boxOffice.Date = refund.CreateDate;
                boxOffice.Returns = refund.TotalPrice;
                boxOffice.CashlessReturns = refund.CashlessPayment;
                boxOffice.Profit = 0 - refund.TotalLoss;
                result.Add(boxOffice);
            }

            return result.OrderBy(b => b.Date).ToList();
        }

        public  List<BoxOfficeVM> GetAll( AppUser user , DateTime dateTo)
        {
            List<BoxOfficeVM> result = new List<BoxOfficeVM>();
            List<Sale> sales =  _context.Sales.Where(s => s.CreateDate < dateTo.AddHours(23)
            && s.BranchId == user.BranchId).ToList();
            List<Refund> refunds = _context.Refunds.Where(r => r.CreateDate < dateTo.AddHours(23) 
            && r.BranchId == user.BranchId).ToList();

            foreach (Sale sale in sales)
            {
                BoxOfficeVM boxOffice = new();
                boxOffice.Date = sale.CreateDate;
                boxOffice.TotalIncoming = sale.TotalPrice;
                boxOffice.CashlessPayment = sale.CashlessPayment;
                boxOffice.Discount = sale.Discount;
                boxOffice.Profit = sale.TotalProfit;
                result.Add(boxOffice);
            }

            foreach (Refund refund in refunds)
            {
                BoxOfficeVM boxOffice = new();
                boxOffice.Date = refund.CreateDate;
                boxOffice.Returns = refund.TotalPrice;
                boxOffice.CashlessReturns = refund.CashlessPayment;
                boxOffice.Profit = 0 - refund.TotalLoss;
                result.Add(boxOffice);
            }

            return result.OrderBy(b => b.Date).ToList();
        }

        public  List<BoxOfficeVM> GetAll(DateTime dateFrom, AppUser user)
        {
            List<BoxOfficeVM> result = new List<BoxOfficeVM>();
            List<Sale> sales =  _context.Sales.Where(s => s.CreateDate > dateFrom
            && s.BranchId == user.BranchId).ToList();
            List<Refund> refunds = _context.Refunds.Where(r => r.CreateDate > dateFrom
            && r.BranchId == user.BranchId).ToList();

            foreach (Sale sale in sales)
            {
                BoxOfficeVM boxOffice = new();
                boxOffice.Date = sale.CreateDate;
                boxOffice.TotalIncoming = sale.TotalPrice;
                boxOffice.CashlessPayment = sale.CashlessPayment;
                boxOffice.Discount = sale.Discount;
                boxOffice.Profit = sale.TotalProfit;
                result.Add(boxOffice);
            }

            foreach (Refund refund in refunds)
            {
                BoxOfficeVM boxOffice = new();
                boxOffice.Date = refund.CreateDate;
                boxOffice.Returns = refund.TotalPrice;
                boxOffice.CashlessReturns = refund.CashlessPayment;
                boxOffice.Profit = 0 - refund.TotalLoss;
                result.Add(boxOffice);
            }

            return result.OrderBy(b => b.Date).ToList();
        }

        public List<BoxOfficeVM> GetAll(AppUser user)
        {
            List<BoxOfficeVM> result = new List<BoxOfficeVM>();
            List<Sale> sales = _context.Sales.Where(s => s.CreateDate > DateTime.Today
            && s.BranchId == user.BranchId).ToList();
            List<Refund> refunds = _context.Refunds.Where(r => r.CreateDate > DateTime.Today
            && r.BranchId == user.BranchId).ToList();

            foreach (Sale sale in sales)
            {
                BoxOfficeVM boxOffice = new();
                boxOffice.Date = sale.CreateDate;
                boxOffice.TotalIncoming = sale.TotalPrice;
                boxOffice.CashlessPayment = sale.CashlessPayment;
                boxOffice.Discount = sale.Discount;
                boxOffice.Profit = sale.TotalProfit;
                result.Add(boxOffice);
            }

            foreach (Refund refund in refunds)
            {
                BoxOfficeVM boxOffice = new();
                boxOffice.Date = refund.CreateDate;
                boxOffice.Returns = refund.TotalPrice;
                boxOffice.CashlessReturns = refund.CashlessPayment;
                boxOffice.Profit = 0 - refund.TotalLoss;
                result.Add(boxOffice);
            }

            return result.OrderBy(b => b.Date).ToList();
        }


    }
}
