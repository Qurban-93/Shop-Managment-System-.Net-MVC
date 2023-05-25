using ShopManagmentSystem.Models.Base;

namespace ShopManagmentSystem.Models
{
    public class Displacement : BaseEntity
    {
        public int SenderId { get; set; }
        public string SenderBranch { get; set; }
        public int DestinationId { get; set; }
        public string DestinationBranch { get; set; }
        public bool IsAcceppted { get; set; }
        public List<DisplacementProduct>? DisplacementProducts { get; set; }

    }
}
