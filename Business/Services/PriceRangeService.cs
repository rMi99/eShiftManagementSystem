using eShiftManagementSystem.Business.Interfaces;
using eShiftManagementSystem.DataAccess.Repositories;
using eShiftManagementSystem.Models;
using System.Collections.Generic;

namespace eShiftManagementSystem.Business.Services
{
    public class PriceRangeService : IPriceRangeService
    {
        private readonly PriceRangeRepository _priceRangeRepository;

        public PriceRangeService()
        {
            _priceRangeRepository = new PriceRangeRepository();
        }

        public List<PriceRange> GetAllPriceRanges()
        {
            return _priceRangeRepository.GetAll();
        }

        public void AddPriceRange(PriceRange priceRange)
        {
            _priceRangeRepository.Add(priceRange);
        }

        public void UpdatePriceRange(PriceRange priceRange)
        {
            _priceRangeRepository.Update(priceRange);
        }

        public void DeletePriceRange(int id)
        {
            _priceRangeRepository.Delete(id);
        }
    }
}