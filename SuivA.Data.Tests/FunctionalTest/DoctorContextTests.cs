using System;
using System.Linq;
using Microsoft.VisualStudio.TestTools.UnitTesting;
using SuivA.Data.Context;
using SuivA.Data.Entity;

namespace SuivA.Data.Tests.FunctionalTest
{
    [TestClass]
    public class DoctorContextTests
    {
        [TestMethod]
        [ExpectedException(typeof(ArgumentNullException))]
        public void AddNewDoctor_ThrowsException_WhenFirstNameIsNull()
        {
            using (var dc = new DoctorContext())
            {
                var doctor = new Doctor
                {
                    Firstname = null,
                    Lastname = "David",
                    Phone = "+33 6 67 81 09 58",
                    Specialty = null,
                    User = (from u in dc.DataContext.Users where u.Role.Name == "visiteur" select u).First(),
                    Office = (from o in dc.DataContext.Offices select o).First()
                };

                dc.CreateDoctor(doctor);
            }
        }

        [TestMethod]
        [ExpectedException(typeof(ArgumentException))]
        public void AddNewDoctor_ThrowsException_WhenFirstNameIsEmpty()
        {
            using (var dc = new DoctorContext())
            {
                var doctor = new Doctor
                {
                    Firstname = "",
                    Lastname = "David",
                    Phone = "+33 6 67 81 09 58",
                    Specialty = null,
                    User = (from u in dc.DataContext.Users where u.Role.Name == "visiteur" select u).First(),
                    Office = (from o in dc.DataContext.Offices select o).First()
                };

                dc.CreateDoctor(doctor);
            }
        }

        [TestMethod]
        [ExpectedException(typeof(ArgumentNullException))]
        public void AddNewDoctor_ThrowsException_WhenLastNameIsNull()
        {
            using (var dc = new DoctorContext())
            {
                var doctor = new Doctor
                {
                    Firstname = "Paul",
                    Lastname = null,
                    Phone = "+33 6 67 81 09 58",
                    Specialty = null,
                    User = (from u in dc.DataContext.Users where u.Role.Name == "visiteur" select u).First(),
                    Office = (from o in dc.DataContext.Offices select o).First()
                };

                dc.CreateDoctor(doctor);
            }
        }

        [TestMethod]
        [ExpectedException(typeof(ArgumentException))]
        public void AddNewDoctor_ThrowsException_WhenLastNameIsEmpty()
        {
            using (var dc = new DoctorContext())
            {
                var doctor = new Doctor
                {
                    Firstname = "Paul",
                    Lastname = "",
                    Phone = "+33 6 67 81 09 58",
                    Specialty = null,
                    User = (from u in dc.DataContext.Users where u.Role.Name == "visiteur" select u).First(),
                    Office = (from o in dc.DataContext.Offices select o).First()
                };

                dc.CreateDoctor(doctor);
            }
        }

        [TestMethod]
        [ExpectedException(typeof(ArgumentNullException))]
        public void AddNewDoctor_ThrowsException_WhenPhoneIsNull()
        {
            using (var dc = new DoctorContext())
            {
                var doctor = new Doctor
                {
                    Firstname = "Paul",
                    Lastname = "Johnson",
                    Phone = null,
                    Specialty = null,
                    User = (from u in dc.DataContext.Users where u.Role.Name == "visiteur" select u).First(),
                    Office = (from o in dc.DataContext.Offices select o).First()
                };

                dc.CreateDoctor(doctor);
            }
        }

        [TestMethod]
        [ExpectedException(typeof(ArgumentException))]
        public void AddNewDoctor_ThrowsException_WhenPhoneIsEmpty()
        {
            using (var dc = new DoctorContext())
            {
                var doctor = new Doctor
                {
                    Firstname = "Paul",
                    Lastname = "Johnson",
                    Phone = "",
                    Specialty = null,
                    User = (from u in dc.DataContext.Users where u.Role.Name == "visiteur" select u).First(),
                    Office = (from o in dc.DataContext.Offices select o).First()
                };

                dc.CreateDoctor(doctor);
            }
        }

        [TestMethod]
        public void AddNewDoctor_DoctorIsStoredInDataStore()
        {
            using (var dc = new DoctorContext())
            {
                var doctor = new Doctor
                {
                    Firstname = "Paul",
                    Lastname = "Johnson",
                    Phone = "+33 6 67 81 09 58",
                    Specialty = null,
                    User = (from u in dc.DataContext.Users where u.Role.Name == "visiteur" select u).First(),
                    Office = (from o in dc.DataContext.Offices select o).First()
                };

                dc.CreateDoctor(doctor);

                var exists = dc.DataContext.Doctors.Any(c => c.Id == doctor.Id);

                Assert.IsTrue(exists);
            }
        }

        [TestMethod]
        public void UpdateDoctor_AppliedValuesAreStoredInDataStore()
        {
            using (var dc = new DoctorContext())
            {
                var doctor = new Doctor
                {
                    Firstname = "Paul",
                    Lastname = "Johnson",
                    Phone = "+33 6 67 81 09 58",
                    Specialty = null,
                    User = (from u in dc.DataContext.Users where u.Role.Name == "visiteur" select u).First(),
                    Office = (from o in dc.DataContext.Offices select o).First()
                };

                dc.CreateDoctor(doctor);

                const string newFirstName = "Dave",
                    newLastName = "Scott",
                    newPhone = "+33 7 58 42 15 20";

                doctor.Firstname = newFirstName;
                doctor.Lastname = newLastName;
                doctor.Phone = newPhone;

                // Act
                dc.UpdateDoctor(doctor);

                // Assert
                dc.DataContext.Entry(doctor).Reload();

                Assert.AreEqual(newPhone, doctor.Phone);
                Assert.AreEqual(newFirstName, doctor.Firstname);
                Assert.AreEqual(newLastName, doctor.Lastname);
            }
        }

        [TestMethod]
        public void GetDoctorList_ReturnsExpectedListOfDoctorEntities()
        {
            using (var dc = new DoctorContext())
            {
                var doctors = dc.GetDoctorList();

                Assert.IsTrue(doctors.ElementAt(0).Id == doctors.Count);
                Assert.IsTrue(doctors.ElementAt(1).Id == doctors.Count - 1);
                Assert.IsTrue(doctors.ElementAt(2).Id == doctors.Count - 2);
            }
        }
    }
}