namespace GSP.Models.Domain
{
    public class Brand
    {
        public int Id { get; set; }
        public string Name { get; set; }

        // Navigation properties
        public ICollection<Vehicle> Vehicles { get; set; }
        


    }
}
