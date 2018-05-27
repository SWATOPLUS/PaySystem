using System;

namespace PaySystem.Models.BusinessModels
{
    public class WorkLog
    {
        public int Id { get; set; }
        public int CardId { get; set; }
        public Card Card { get; set; }
        public DateTime Date { get; set; }
        public int HoursWorked { get; set; }
    }
}
