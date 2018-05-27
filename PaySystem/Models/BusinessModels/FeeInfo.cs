namespace PaySystem.Models.BusinessModels
{
    public class FeeInfo
    {
        public int Id { get; set; }
        public int CheckId { get; set; }
        public string Name { get; set; }
        public string Comment { get; set; }
        public decimal Amount { get; set; }
    }
}
