using InsightCore.Domain.Common;
using InsightCore.Domain.Entities;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;

namespace InsightCore.Domain.Specifications
{
    public class CountryInBlackListSpecification : ISpecification<User>
    {
        readonly List<string> countriesInBlackList = new List<string>
        {
            "Argentina",
            "Brasil",
            "Chile",
            "Colombia",
            "México",
            "España",
            "Portugal",
            "Estados Unidos",
            "Canadá",
            "Alemania"
        };

        public bool IsSatisfiedBy(User entity)
        {
            if (entity == null) return false;

            // If user has no country assigned, consider it satisfied (not in blacklist)
            if (entity.Country == null || string.IsNullOrEmpty(entity.Country.Name))
                return true;

            // Case-insensitive comparison against blacklist
            var isBlackListed = countriesInBlackList
                .Any(c => string.Equals(c, entity.Country.Name, StringComparison.OrdinalIgnoreCase));

            return !isBlackListed;
        }
    }
}
