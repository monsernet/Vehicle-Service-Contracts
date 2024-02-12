namespace GSP.Models.Domain
{
    public class Customer
    {
        public int Id { get; set; }
        public string CustomerName { get; set; }
        public string CustomerCode { get; set; }
        public string? CivilId { get; set; }
        public string Phone { get; set; }
        public string? Phone2 { get; set; }
        public string? Phone3 { get; set; }
        public string? Phone4 { get; set; }
        public string? Email { get; set; }
        public string? Address { get; set; }
    }
}
