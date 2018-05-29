using System;
using System.Linq;
using Microsoft.VisualStudio.TestTools.UnitTesting;
using SuivA.Data.Context;
using SuivA.Data.Entity;

namespace SuivA.Data.Tests.FunctionalTest
{
    [TestClass]
    public class OfficeContextTests
    {
        [TestMethod]
        [ExpectedException(typeof(ArgumentException))]
        public void AddNewOffice_ThrowsException_WhenStreetNumberIsEmpty()
        {
            using (var oc = new OfficeContext())
            {
                var office = new Office
                {
                    StreetNumber = Convert.ToInt32(null),
                    StreetName = "rue Saint André",
                    City = "Lille",
                    ZipCode = 59800
                };

                oc.CreateOffice(office);
            }
        }

        [TestMethod]
        [ExpectedException(typeof(ArgumentNullException))]
        public void AddNewOffice_ThrowsException_WhenStreetNameIsNull()
        {
            using (var oc = new OfficeContext())
            {
                var office = new Office
                {
                    StreetNumber = 78,
                    StreetName = null,
                    City = "Lille",
                    ZipCode = 59800
                };

                oc.CreateOffice(office);
            }
        }

        [TestMethod]
        [ExpectedException(typeof(ArgumentException))]
        public void AddNewOffice_ThrowsException_WhenStreetNameIsEmpty()
        {
            using (var oc = new OfficeContext())
            {
                var office = new Office
                {
                    StreetNumber = 78,
                    StreetName = "",
                    City = "Lille",
                    ZipCode = 59800
                };

                oc.CreateOffice(office);
            }
        }

        [TestMethod]
        [ExpectedException(typeof(ArgumentNullException))]
        public void AddNewOffice_ThrowsException_WhenCityIsNull()
        {
            using (var oc = new OfficeContext())
            {
                var office = new Office
                {
                    StreetNumber = 78,
                    StreetName = "rue Saint André",
                    City = null,
                    ZipCode = 59800
                };

                oc.CreateOffice(office);
            }
        }

        [TestMethod]
        [ExpectedException(typeof(ArgumentException))]
        public void AddNewOffice_ThrowsException_WhenCityIsEmpty()
        {
            using (var oc = new OfficeContext())
            {
                var office = new Office
                {
                    StreetNumber = 78,
                    StreetName = "rue Saint André",
                    City = "",
                    ZipCode = 59800
                };

                oc.CreateOffice(office);
            }
        }

        [TestMethod]
        [ExpectedException(typeof(ArgumentException))]
        public void AddNewOffice_ThrowsException_WhenZipCodeIsEmpty()
        {
            using (var oc = new OfficeContext())
            {
                var office = new Office
                {
                    StreetNumber = 74,
                    StreetName = "rue Saint André",
                    City = "Lille"
                    //zip_code = ,
                };

                oc.CreateOffice(office);
            }
        }

        [TestMethod]
        public void AddNewOffice_OfficeIsStoredInDataStore()
        {
            using (var oc = new OfficeContext())
            {
                var office = new Office
                {
                    StreetNumber = 74,
                    StreetName = "rue Saint André",
                    City = "Lille",
                    ZipCode = 59800
                };


                oc.CreateOffice(office);

                var exists = oc.DataContext.Offices.Any(c => c.Id == office.Id);

                Assert.IsTrue(exists);
            }
        }

        /*[TestMethod]
        public void GetGPSCoordinates_ReturnsNonNullLatLongForValidAddress()
        {
            using (var oc = new OfficeContext())
            {
                var office = new Office
                {
                    StreetNumber = 202,
                    StreetName = "rue du Buisson",
                    City = "Marcq-en-Baroeul",
                    ZipCode = 59700,
                };

                oc.CreateOffice(office);

                var gps = oc.GetGspCoordinates(office);

                Assert.IsNotNull(gps.Latitude);
                Assert.IsNotNull(gps.Longitude);
            }
        }*/

        [TestMethod]
        public void UpdateOffice_AppliedValuesAreStoredInDataStore()
        {
            using (var oc = new OfficeContext())
            {
                var office = new Office
                {
                    StreetNumber = 74,
                    StreetName = "rue Saint André",
                    City = "Lille",
                    ZipCode = 59800
                };

                oc.CreateOffice(office);

                const int newStreetNumber = 12,
                    newZipCode = 59700;
                const string newStreetName = "avenue de la République",
                    newCity = "Marcq-en-Baroeul";

                office.StreetNumber = newStreetNumber;
                office.StreetName = newStreetName;
                office.City = newCity;
                office.ZipCode = newZipCode;

                // Act
                oc.UpdateOffice(office);

                // Assert
                oc.DataContext.Entry(office).Reload();

                Assert.AreEqual(newStreetNumber, office.StreetNumber);
                Assert.AreEqual(newZipCode, office.ZipCode);
                Assert.AreEqual(newStreetName, office.StreetName);
                Assert.AreEqual(newCity, office.City);
            }
        }
    }
}