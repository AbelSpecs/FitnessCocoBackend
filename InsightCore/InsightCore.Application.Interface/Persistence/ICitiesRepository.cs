using InsightCore.Domain.Entities;
using System;
using System.Collections.Generic;
using System.Text;

namespace InsightCore.Application.Interface.Persistence
{
    public interface ICitiesRepository
    {
        Task<City> GetByCountryIdAsync(string countryId);
        Task<IEnumerable<City>> GetCitiesByCountryAsync(string countryId);
    }
}
