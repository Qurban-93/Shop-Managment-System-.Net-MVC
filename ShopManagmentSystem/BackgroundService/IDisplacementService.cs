using ShopManagmentSystem.Models;

namespace ShopManagmentSystem.BackgroundService
{
    public interface IDisplacementService
    {
        public string ScheduleDisplacement(Displacement displacement);
    }
}
