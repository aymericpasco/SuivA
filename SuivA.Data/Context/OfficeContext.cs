using System;
using System.Collections.Generic;
using System.Linq;
using SuivA.Data.Context.Interface;
using SuivA.Data.Entity;

namespace SuivA.Data.Context
{
    public sealed class OfficeContext : GlobalAppContext, IOfficeContext
    {
        public void CreateOffice(Office office)
        {
            Check.Require(office.StreetNumber);
            Check.Require(office.StreetName);
            Check.Require(office.City);
            Check.Require(office.ZipCode);

            office.CreatedAt = DateTime.Now;

            DataContext.Offices.Add(office);
            DataContext.SaveChanges();
        }

        public void UpdateOffice(Office office)
        {
            var entity = DataContext.Offices.Find(office.Id);

            if (entity == null) throw new NotImplementedException("Handle appropriately for API design.");

            Check.Require(office.StreetNumber);
            Check.Require(office.StreetName);
            Check.Require(office.City);
            Check.Require(office.ZipCode);

            office.UpdatedAt = DateTime.Now;

            DataContext.Entry(office).CurrentValues.SetValues(office);
            DataContext.SaveChanges();
        }

        public ICollection<Office> GetOfficeList()
        {
            return DataContext.Offices.OrderByDescending(p => p.Id).ToArray();
        }
    }
}