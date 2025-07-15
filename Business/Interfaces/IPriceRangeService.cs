using eShiftManagementSystem.Models;
using System.Collections.Generic;

namespace eShiftManagementSystem.Business.Interfaces
{
    public interface IPriceRangeService
    {
        List<PriceRange> GetAllPriceRanges();
        void AddPriceRange(PriceRange priceRange);
        void UpdatePriceRange(PriceRange priceRange);
        void DeletePriceRange(int id);
    }
}