namespace PaySystem.Models.BusinessModels
{
    public class Card
    {
        public int Id { get; set; }
        public Worker Worker { get; set; }

        public Card(Worker worker)
        {
            Worker = worker;
        }
    }
}