using System;

namespace PaySystem.Models.BusinessModels
{
    public class Worker
    {
        public int Id { get; set; }
        public string Name { get; set; }
        public string Job { get; set; }
        public DateTime DateOfBirth { get; set; }
    }
}