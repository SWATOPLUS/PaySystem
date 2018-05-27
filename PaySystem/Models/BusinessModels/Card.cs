using System;
using System.Collections.Generic;

namespace PaySystem.Models.BusinessModels
{
    public class Card
    {
        public int Id { get; set; }

        public int WorkerId { get; set; }
        public Worker Worker { get; set; }

        public DateTime JoinDate { get; set; }
        public bool IsHourPay { get; set; }

        public List<WorkLog> WorkLogs { get; set; }
    }
}