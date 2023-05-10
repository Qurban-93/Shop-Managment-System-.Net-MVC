namespace ShopManagmentSystem.ViewModels
{
    public class BoxOfficeVM
    {
        public DateTime Date { get; set; }
        public double TotalIncoming { get; set; }
        public double CashlessPayment { get; set; }
        public double Discount { get; set; }
        public double Returns { get; set; }
        public double CashlessReturns { get; set; }
        public double Expenses { get; set; }
    }
}
