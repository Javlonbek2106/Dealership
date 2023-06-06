namespace Application.DTOs.Group
{
    public abstract class CarBaseDTO
    {
        public string Brand { get; set; }
        public string Model { get; set; }
        public int Year { get; set; }
        public decimal Price { get; set; }
        public string Url { get; set; }
    }
}
