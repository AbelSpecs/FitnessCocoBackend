using InsightCore.Domain.Common;
using InsightCore.Domain.Entities;
using System;
using System.Collections.Generic;
using System.Text;

namespace InsightCore.Domain.Specifications
{
    public class CountryInBlackListSpecification : ISpecification<User>
    {
        readonly List<string> countriesInBlackList =
        [
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
        ];

        public bool IsSatisfiedBy(User entity)
        {
            return !countriesInBlackList.Contains(entity.Country);
        }
    }
}
