using System.Collections.Generic;

namespace PaySystem.Models.BusinessModels
{
    public class Check
    {
        public int Id { get; set; }
        public List<FeeInfo> FeeInfos { get; set; }
    }
}