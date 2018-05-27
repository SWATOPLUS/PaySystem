using System.Collections.Generic;

namespace PaySystem.Models.BusinessModels
{
    public class Check
    {
        public int Id { get; set; }
        public FeeInfo[] FeeInfos { get; set; }

        public Check(FeeInfo[] feeInfos)
        {
            FeeInfos = feeInfos;
        }
    }
}