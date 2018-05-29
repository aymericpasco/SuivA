using System;
using System.Linq;
using Microsoft.VisualStudio.TestTools.UnitTesting;
using SuivA.Data.Context;
using SuivA.Data.Entity;

namespace SuivA.Data.Tests.FunctionalTest
{
    [TestClass]
    public class VisitContextTests
    {
        [TestMethod]
        [ExpectedException(typeof(ArgumentOutOfRangeException))]
        public void AddNewVisit_ThrowsException_WhenVisitDateIsInFuture()
        {
            using (var vc = new VisitContext())
            {
                var visit = new Visit
                {
                    VisitDate = DateTime.Now.AddDays(1),
                    Appointment = Convert.ToSByte(true),
                    ArrivingTime = DateTime.Now.TimeOfDay,
                    User = (from v in vc.DataContext.Users where v.Id == 1 select v).First(),
                    Doctor = (from d in vc.DataContext.Doctors where d.Id == 1 select d).First()
                };

                vc.CreateVisit(visit);
            }
        }

        [TestMethod]
        [ExpectedException(typeof(ArgumentOutOfRangeException))]
        public void AddNewVisit_ThrowsException_WhenStartTimeInterviewIsInferiorToArrivingTime()
        {
            using (var vc = new VisitContext())
            {
                var visit = new Visit
                {
                    VisitDate = DateTime.Now,
                    Appointment = Convert.ToSByte(true),
                    ArrivingTime = DateTime.Now.TimeOfDay,
                    StartTimeInterview = DateTime.Now.AddMinutes(-30).TimeOfDay
                };

                vc.CreateVisit(visit);
            }
        }

        [TestMethod]
        [ExpectedException(typeof(ArgumentOutOfRangeException))]
        public void AddNewVisit_ThrowsException_WhenDepartureTimeIsInferiorToStartTimeInterview()
        {
            using (var vc = new VisitContext())
            {
                var visit = new Visit
                {
                    VisitDate = DateTime.Now,
                    Appointment = Convert.ToSByte(true),
                    ArrivingTime = DateTime.Now.TimeOfDay,
                    StartTimeInterview = DateTime.Now.TimeOfDay,
                    DepartureTime = DateTime.Now.TimeOfDay.Subtract(TimeSpan.Parse("00:30")),
                    User = (from v in vc.DataContext.Users where v.Id == 1 select v).First(),
                    Doctor = (from d in vc.DataContext.Doctors where d.Id == 1 select d).First()
                };

                vc.CreateVisit(visit);
            }
        }


        [TestMethod]
        [ExpectedException(typeof(ArgumentException))]
        public void AddNewVisit_ThrowsException_WhenDoctorDoesNotBelongToUser()
        {
            using (var vc = new VisitContext())
            {
                var visit = new Visit
                {
                    VisitDate = DateTime.Now,
                    Appointment = Convert.ToSByte(true),
                    ArrivingTime = DateTime.Now.TimeOfDay,
                    StartTimeInterview = DateTime.Now.TimeOfDay,
                    DepartureTime = DateTime.Now.TimeOfDay,
                    User = (from u in vc.DataContext.Users where u.Id == 1 select u).First(),
                    Doctor = (from d in vc.DataContext.Doctors where d.User.Id == 2 select d).First()
                };

                vc.CreateVisit(visit);
            }
        }

        [TestMethod]
        public void AddNewVisit_VisitIsStoredInDataStore()
        {
            using (var vc = new VisitContext())
            {
                var visit = new Visit
                {
                    VisitDate = DateTime.Now,
                    Appointment = Convert.ToSByte(true),
                    ArrivingTime = DateTime.Now.TimeOfDay,
                    StartTimeInterview = DateTime.Now.TimeOfDay,
                    DepartureTime = DateTime.Now.TimeOfDay,
                    User = (from u in vc.DataContext.Users where u.Id == 1 select u).First(),
                    Doctor = (from d in vc.DataContext.Doctors where d.User.Id == 1 select d).First()
                };

                vc.CreateVisit(visit);

                var exists = vc.DataContext.Visits.Any(v => v.Id == visit.Id);

                Assert.IsTrue(exists);
            }
        }

        [TestMethod]
        public void UpdateVisit_AppliedValuesAreStoredInDataStore()
        {
            using (var vc = new VisitContext())
            {
                // Arrange
                var visit = new Visit
                {
                    VisitDate = DateTime.Now,
                    Appointment = Convert.ToSByte(true),
                    ArrivingTime = DateTime.Now.TimeOfDay,
                    StartTimeInterview = DateTime.Now.TimeOfDay,
                    DepartureTime = DateTime.Now.TimeOfDay,
                    User = (from v in vc.DataContext.Users where v.Id == 1 select v).First(),
                    Doctor = (from d in vc.DataContext.Doctors where d.User.Id == 1 select d).First()
                };

                vc.CreateVisit(visit);

                // new values
                var newDate = new DateTime(2018, 04, 27);
                const bool newAppointment = false;
                var newTime = new TimeSpan(19, 20, 0);
                var newUser = (from v in vc.DataContext.Users where v.Id == 2 select v).First();
                var newDoctor = (from d in vc.DataContext.Doctors where d.User.Id == 2 select d).First();

                visit.VisitDate = newDate;
                visit.Appointment = Convert.ToSByte(newAppointment);
                visit.ArrivingTime = newTime;
                visit.StartTimeInterview = newTime;
                visit.DepartureTime = newTime;
                visit.User = newUser;
                visit.Doctor = newDoctor;

                // Act
                vc.UpdateVisit(visit);

                // Assert
                vc.DataContext.Entry(visit).Reload();

                Assert.AreEqual(newDate, visit.VisitDate);
                Assert.AreEqual(newAppointment, Convert.ToBoolean(visit.Appointment));
                Assert.AreEqual(newTime, visit.ArrivingTime);
                Assert.AreEqual(newTime, visit.StartTimeInterview);
                Assert.AreEqual(newTime, visit.DepartureTime);
                Assert.AreEqual(newUser, visit.User);
                Assert.AreEqual(newDoctor, visit.Doctor);
            }
        }

        [TestMethod]
        public void GetVisitList_ReturnsExpectedListOfVisitEntities()
        {
            using (var vc = new VisitContext())
            {
                var firstCount = vc.GetVisitList().Count;

                for (var i = 1; i < 4; ++i)
                    vc.CreateVisit(new Visit
                    {
                        VisitDate = DateTime.Now.AddDays(-i),
                        Appointment = Convert.ToSByte(true),
                        ArrivingTime = DateTime.Now.TimeOfDay,
                        User = (from v in vc.DataContext.Users where v.Id == 1 select v).First(),
                        Doctor = (from d in vc.DataContext.Doctors where d.User.Id == 1 select d).First()
                    });

                var visits = vc.GetVisitList();

                Assert.IsTrue(firstCount != 0);
                Assert.IsTrue(visits.Count == firstCount + 3);
            }
        }

        [TestMethod]
        public void DeleteVisit_RemovesVisitFromDataStore()
        {
            using (var vc = new VisitContext())
            {
                var visit = new Visit
                {
                    VisitDate = DateTime.Now,
                    Appointment = Convert.ToSByte(true),
                    ArrivingTime = DateTime.Now.TimeOfDay,
                    StartTimeInterview = DateTime.Now.TimeOfDay,
                    DepartureTime = DateTime.Now.TimeOfDay,
                    User = (from v in vc.DataContext.Users where v.Id == 1 select v).First(),
                    Doctor = (from d in vc.DataContext.Doctors where d.User.Id == 1 select d).First()
                };

                vc.CreateVisit(visit);
                vc.DeleteVisit(visit);
                Assert.IsFalse(vc.DataContext.Visits.Find(visit.Id) == visit);
            }
        }
    }
}