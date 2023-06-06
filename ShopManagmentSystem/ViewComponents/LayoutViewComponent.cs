using Microsoft.AspNetCore.Identity;
using Microsoft.AspNetCore.Mvc;
using Microsoft.EntityFrameworkCore;
using Newtonsoft.Json;
using ShopManagmentSystem.DAL;
using ShopManagmentSystem.Models;
using ShopManagmentSystem.ViewModels;

namespace ShopManagmentSystem.ViewComponents
{
    public class LayoutViewComponent : ViewComponent
    {
        private readonly AppDbContext _appDbContext;
        private readonly UserManager<AppUser> _userManager;

        public LayoutViewComponent(AppDbContext appDbContext, UserManager<AppUser> userManager)
        {
            _appDbContext = appDbContext;
            _userManager = userManager;
        }

        public async Task<IViewComponentResult> InvokeAsync()
        {
            LayoutVM layoutVM = new LayoutVM();
            if (User.Identity.IsAuthenticated)
            {
                AppUser user = await _userManager.FindByNameAsync(User.Identity.Name);
                int count = await _appDbContext.Orders.Where(o => o.BranchId == user.BranchId).CountAsync();
                layoutVM.CountOrder = count;
                layoutVM.Role =await _userManager.GetRolesAsync(user);
                layoutVM.MessageCount = await _appDbContext.Messages.Where(m=>!m.IsRead && m.DestinationId == user.Id).CountAsync();
                layoutVM.UserId= user.Id;
                return View(layoutVM);
            }  
            
            return View(layoutVM);
        }
    }
}
