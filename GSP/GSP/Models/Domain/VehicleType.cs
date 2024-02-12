namespace GSP.Models.Domain
{
    public class VehicleType
    {
        public int Id { get; set; }
        public string Name { get; set; }
        public int VehicleId { get; set; }
        public Vehicle Vehicle { get; set; }
    }
}
