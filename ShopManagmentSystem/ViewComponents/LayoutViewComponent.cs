using Microsoft.AspNetCore.Mvc;
using Newtonsoft.Json;
using ShopManagmentSystem.ViewModels;

namespace ShopManagmentSystem.ViewComponents
{
    public class LayoutViewComponent : ViewComponent
    {
        public async Task<IViewComponentResult> InvokeAsync()
        {
            string basket = Request.Cookies["basket"];
            List<ProductVM> productsInBasket;
            int count = 0;
            if (basket == null)
            {
                productsInBasket = new();

            }
            else
            {
                productsInBasket = JsonConvert.DeserializeObject<List<ProductVM>>(basket);
                count = productsInBasket.Count();
            }
            return View(count);
        }
    }
}
