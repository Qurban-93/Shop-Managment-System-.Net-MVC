using ShopManagmentSystem.Models.Base;

namespace ShopManagmentSystem.Models
{
    public class Message : BaseEntity
    {
        public string SenderId { get; set; }
        public string DestinationId { get; set; }
        public string Content { get; set; }
        public bool IsRead { get; set; }
    }
}
