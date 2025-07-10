namespace eShiftManagementSystem.Models
{
    public class PriceRange
    {
        public int Id { get; set; }
        public decimal FromWeight { get; set; }
        public decimal ToWeight { get; set; }
        public decimal Price { get; set; }
    }
}