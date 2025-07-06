using System;

namespace eShiftManagementSystem.Utils
{
    public static class NumberGenerator
    {
        public static string GenerateJobNumber()
        {
            return $"JOB{DateTime.Now:yyyyMMdd}{DateTime.Now.Millisecond:000}";
        }

        public static string GenerateQuoteNumber()
        {
            return $"QTE{DateTime.Now:yyyyMMdd}{DateTime.Now.Millisecond:000}";
        }

        public static string GenerateCustomerNumber()
        {
            return $"CUST{DateTime.Now:yyyyMMdd}{DateTime.Now.Millisecond:000}";
        }

        public static string GenerateUserNumber()
        {
            return $"USER{DateTime.Now:yyyyMMdd}{DateTime.Now.Millisecond:000}";
        }
    }
}