using System;

namespace SkipTakeRepro.Models
{
    public class Money
    {
        public decimal Amount { get; set; }
        public Currency Currency { get; set; }
        
        public override string ToString()
        {
            return $"{Currency} {Amount:#.00}";
        }
    }
}