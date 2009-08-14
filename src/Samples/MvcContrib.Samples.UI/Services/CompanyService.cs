using System;
using System.Collections.Generic;
using System.Linq;
using MvcContrib.Samples.UI.Models;

namespace MvcContrib.Samples.UI.Services
{
    public class CompanyService
    {
        private static IList<Company> cachedCompanies = new List<Company>
        {
            new Company { Id = Guid.NewGuid(), Name = "Acme Inc"},
            new Company { Id = Guid.NewGuid(), Name = "MicorSoft" }
        };

        public static IList<Company> GetCompanies()
        {
            return cachedCompanies;
        }

        public static Company GetCompany(Guid id)
        {
            return cachedCompanies.Where(x => x.Id == id).FirstOrDefault();
        }
    }
}