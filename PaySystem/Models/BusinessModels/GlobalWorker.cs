using System;

namespace PaySystem.Models.BusinessModels
{
    public class GlobalWorker
    {
        public int Id { get; set; }
        public string Name { get; set; }
        public DateTime DateOfBirth { get; set; }
        public string Biography { get; set; }
        public DateTime UpdateTimeDateTime { get; set; }
    }
}