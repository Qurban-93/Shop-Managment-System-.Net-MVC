using Hangfire;
using ShopManagmentSystem.Models;
using ShopManagmentSystem.Service;

namespace ShopManagmentSystem.BackgroundService
{
    public class DisplacementService : IDisplacementService
    {
        private readonly IDeleteDisplacementService _deleteService;

        public DisplacementService(IDeleteDisplacementService deleteService)
        {
            _deleteService = deleteService;
        }

        public string ScheduleDisplacement(Displacement displacement)
        {
            return BackgroundJob.Schedule(() => _deleteService.DeleteDisplacement(displacement), displacement.CreateDate.AddSeconds(60));
        }

    }
}
