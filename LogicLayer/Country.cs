using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using DataLayer.Models;
namespace LogicLayer
{
    public class Country:ICountry
    {
        InsurewaveContext db;
        public Country()
        {
            db = new InsurewaveContext();
        }
        public List<CurrencyConversion> GetCountries()
        {
            List<CurrencyConversion> countries = db.CurrencyConversions.ToList();
            return countries;
        }
    }
}
