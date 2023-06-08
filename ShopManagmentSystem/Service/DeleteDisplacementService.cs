using Microsoft.AspNetCore.Http.HttpResults;
using Microsoft.AspNetCore.SignalR;
using Microsoft.EntityFrameworkCore;
using ShopManagmentSystem.DAL;
using ShopManagmentSystem.Hubs;
using ShopManagmentSystem.Models;

namespace ShopManagmentSystem.Service
{
    public  class DeleteDisplacementService : IDeleteDisplacementService
    {
        private readonly AppDbContext _context;
        private readonly IHubContext<UpdateHub> _hubContext;

        public DeleteDisplacementService(AppDbContext context, IHubContext<UpdateHub> hubContext)
        {
            _context = context;
            _hubContext = hubContext;
        }

        public void DeleteDisplacement(Displacement displacement)
        {
            Displacement existDisplacement = _context.Displacement
                .Include(ed=>ed.DisplacementProducts)
                .FirstOrDefault(d=>d.CreateDate== displacement.CreateDate);
            if (existDisplacement == null) return;
            if(existDisplacement.IsAcceppted) return;
            foreach (var item in existDisplacement.DisplacementProducts)
            {
                _context.Products.FirstOrDefault(p => p.Id == item.ProductId).BranchId = existDisplacement.SenderId;
            }
            int id = existDisplacement.Id;
            existDisplacement.IsDeleted = true;
            existDisplacement.DeleteInfo = "Time limit";
            _context.SaveChangesAsync();
            _hubContext.Clients.All.SendAsync("DeleteDisplacement",id);
        }
    }
}
