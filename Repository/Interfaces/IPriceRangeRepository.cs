using eShiftManagementSystem.Models;
using System.Collections.Generic;

namespace eShiftManagementSystem.DataAccess.Interfaces
{
    public interface IPriceRangeRepository
    {
        PriceRange GetById(int id);
        List<PriceRange> GetAll();
        void Add(PriceRange priceRange);
        void Update(PriceRange priceRange);
        void Delete(int id);
    }
}