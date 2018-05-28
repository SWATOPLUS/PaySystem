using System;
using System.Collections.Generic;

namespace PaySystem.Models.BusinessModels
{
    public class Check
    {
        public int Id { get; set; }
        public int WorkerId { get; set; }
        public Worker Worker { get; set; }
        public List<FeeInfo> FeeInfos { get; set; }
        public DateTime Period { get; set; }
    }
}