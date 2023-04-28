using Microsoft.AspNetCore.Mvc;
using Newtonsoft.Json;
using ShopManagmentSystem.DAL;
using ShopManagmentSystem.ViewModels;

namespace ShopManagmentSystem.ViewComponents
{
    public class LayoutViewComponent : ViewComponent
    {
        private readonly AppDbContext _appDbContext;

        public LayoutViewComponent(AppDbContext appDbContext)
        {
            _appDbContext = appDbContext;
        }

        public async Task<IViewComponentResult> InvokeAsync()
        {
            int count = _appDbContext.Orders.Count();
            return View(count);
        }
    }
}
