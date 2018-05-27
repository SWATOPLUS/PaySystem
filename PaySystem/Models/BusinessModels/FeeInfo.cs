namespace PaySystem.Models.BusinessModels
{
    public class FeeInfo
    {
        public int Id { get; set; }
        public string Name { get; set; }
        public decimal Amount { get; set; }

        public FeeInfo(string name, decimal amount)
        {
            Name = name;
            Amount = amount;
        }
    }
}
