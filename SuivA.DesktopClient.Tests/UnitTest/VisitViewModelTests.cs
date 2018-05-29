using System;
using System.Collections.Generic;
using System.Linq;
using Microsoft.VisualStudio.TestTools.UnitTesting;
using Moq;
using SuivA.Data.Context.Interface;
using SuivA.Data.Entity;
using SuivA.DesktopClient.ViewModel;

namespace SuivA.DesktopClient.Tests.UnitTest
{
    [TestClass]
    public class VisitViewModelTests
    {
        private Mock<IVisitContext> _mock;
        private List<Visit> _store;

        [TestInitialize]
        public void TestInitialize()
        {
            _store = new List<Visit>();

            _mock = new Mock<IVisitContext>();
            _mock.Setup(m => m.GetVisitList()).Returns(_store);
            _mock.Setup(m => m.CreateVisit(It.IsAny<Visit>())).Callback<Visit>(visit => _store.Add(visit));
            _mock.Setup(m => m.DeleteVisit(It.IsAny<Visit>())).Callback<Visit>(visit => _store.Remove(visit));
            _mock.Setup(m => m.UpdateVisit(It.IsAny<Visit>())).Callback<Visit>(visit =>
            {
                var i = _store.IndexOf(visit);
                _store[i] = visit;
            });
        }

        [TestMethod]
        public void IsViewModel()
        {
            Assert.IsTrue(typeof(VisitViewModel).BaseType == typeof(Windows.ViewModel));
        }

        [TestMethod]
        public void AddCommand_CannotExecuteWhenVisitDateIsNotValid()
        {
            var viewModel = new VisitViewModel(_mock.Object)
            {
                SelectedVisit = new Visit
                {
                    VisitDate = DateTime.Now.AddYears(-2), // 2016 donc faux
                    Appointment = Convert.ToSByte(false), // bon
                    ArrivingTime = TimeSpan.Parse("13:30") // bon
                }
            };

            Assert.IsFalse(viewModel.AddCommand.CanExecute(null));
        }

        [TestMethod]
        public void AddCommand_CannotExecuteWhenArrivingTimeIsNotValid()
        {
            var viewModel = new VisitViewModel(_mock.Object)
            {
                SelectedVisit = new Visit
                {
                    VisitDate = DateTime.Now, // bon
                    Appointment = Convert.ToSByte(true), // bon
                    ArrivingTime = new TimeSpan(25, 30, 0) // 25h30 = faux
                }
            };

            Assert.IsFalse(viewModel.AddCommand.CanExecute(null));
        }

        /*[TestMethod]
        public void AddCommand_AddsVisitToVisitsCollectionWhenExecutedSuccessfully()
        {
            var viewModel = new VisitViewModel(_mock.Object)
            {
                SelectedVisit = new Visit
                {
                    VisitDate = DateTime.Now,
                    Appointment = Convert.ToSByte(false),
                    ArrivingTime = DateTime.Now.TimeOfDay,
                    StartTimeInterview = DateTime.Now.TimeOfDay,
                    DepartureTime = DateTime.Now.TimeOfDay,
                    User = (from u in _mock.Object.DataContext.Users where u.Id == 1 select u).First(),
                    Doctor = (from d in _mock.Object.DataContext.Doctors where d.User.Id == 1 select d).First()
                }
            };
            viewModel.AddCommand.Execute(null);
            Assert.IsTrue(viewModel.Visits.Count == 1);
        }*/

        [TestMethod]
        public void CanModify_ShouldEqualFalseWhenSelectedVisitIsNull()
        {
            var viewModel = new VisitViewModel(_mock.Object) {SelectedVisit = null};
            Assert.IsFalse(viewModel.CanModify);
        }

        [TestMethod]
        public void CanModify_ShouldEqualTrueWhenSelectedVisitIsNotNull()
        {
            var viewModel = new VisitViewModel(_mock.Object) {SelectedVisit = new Visit()};
            Assert.IsTrue(viewModel.CanModify);
        }

        [TestMethod]
        public void GetVisitListCommand_PopulatesVisitsPropertyWithExpectedCollectionFromDataStore()
        {
            for (var i = 1; i < 4; ++i)
                _mock.Object.CreateVisit(
                    new Visit
                    {
                        VisitDate = DateTime.Now.AddDays(-i),
                        Appointment = Convert.ToSByte(false),
                        ArrivingTime = DateTime.Now.TimeOfDay,
                        StartTimeInterview = DateTime.Now.TimeOfDay,
                        DepartureTime = DateTime.Now.TimeOfDay
                    }
                );

            var viewModel = new VisitViewModel(_mock.Object);

            viewModel.GetVisitListCommand.Execute(null);

            Assert.IsTrue(viewModel.Visits.Count == 3);
        }

        [TestMethod]
        public void GetVisitListCommand_SelectedCustomerIsSetToNullWhenExecuted()
        {
            var viewModel = new VisitViewModel(_mock.Object)
            {
                SelectedVisit = new Visit()
            };

            viewModel.GetVisitListCommand.Execute(null);

            Assert.IsNull(viewModel.SelectedVisit);
        }

        [TestMethod]
        public void SaveCommand_InvokesIVisitContextUpdateVisitMethod()
        {
            _mock.Object.CreateVisit(
                new Visit
                {
                    VisitDate = DateTime.Now,
                    Appointment = Convert.ToSByte(false),
                    ArrivingTime = DateTime.Now.TimeOfDay,
                    StartTimeInterview = DateTime.Now.TimeOfDay,
                    DepartureTime = DateTime.Now.TimeOfDay
                }
            );

            var viewModel = new VisitViewModel(_mock.Object);

            viewModel.GetVisitListCommand.Execute(null);
            viewModel.SelectedVisit = viewModel.Visits.First();

            // Act
            viewModel.SelectedVisit.VisitDate = DateTime.Now.AddDays(-1);
            viewModel.SelectedVisit.Appointment = Convert.ToSByte(true);
            viewModel.SelectedVisit.ArrivingTime = DateTime.Now.AddMinutes(-2).TimeOfDay;
            viewModel.SelectedVisit.StartTimeInterview = DateTime.Now.AddMinutes(-1).TimeOfDay;
            viewModel.SelectedVisit.DepartureTime = DateTime.Now.AddMinutes(0).TimeOfDay;

            viewModel.UpdateCommand.Execute(null);

            // Assert
            _mock.Verify(m => m.UpdateVisit(It.IsAny<Visit>()), Times.Once);
        }

        [TestMethod]
        public void DeleteCommand_InvokesIVisitContextDeleteVisitMethod()
        {
            _mock.Object.CreateVisit(
                new Visit
                {
                    VisitDate = DateTime.Now,
                    Appointment = Convert.ToSByte(false),
                    ArrivingTime = DateTime.Now.TimeOfDay,
                    StartTimeInterview = DateTime.Now.TimeOfDay,
                    DepartureTime = DateTime.Now.TimeOfDay
                }
            );

            var viewModel = new VisitViewModel(_mock.Object);

            viewModel.GetVisitListCommand.Execute(null);
            viewModel.SelectedVisit = viewModel.Visits.First();

            // Act
            viewModel.DeleteCommand.Execute(null);

            // Assert
            _mock.Verify(m => m.DeleteVisit(It.IsAny<Visit>()), Times.Once);
        }

        [TestMethod]
        public void DeleteCommand_SelectedVisitIsSetToNull()
        {
            // Arrange
            _mock.Object.CreateVisit(
                new Visit
                {
                    VisitDate = DateTime.Now,
                    Appointment = Convert.ToSByte(false),
                    ArrivingTime = DateTime.Now.TimeOfDay,
                    StartTimeInterview = DateTime.Now.TimeOfDay,
                    DepartureTime = DateTime.Now.TimeOfDay
                }
            );

            var viewModel = new VisitViewModel(_mock.Object);

            viewModel.GetVisitListCommand.Execute(null);
            viewModel.SelectedVisit = viewModel.Visits.First();

            // Act
            viewModel.DeleteCommand.Execute(null);

            // Assert
            Assert.IsNull(viewModel.SelectedVisit);
        }

        [TestMethod]
        public void PropertyChanged_IsRaisedForCanModifyWhenSelectedVisitPropertyHasChanged()
        {
            var viewModel = new VisitViewModel(_mock.Object);

            var eventRaised = false;

            viewModel.PropertyChanged += (sender, e) =>
            {
                if (e.PropertyName == "CanModify")
                    eventRaised = true;
            };

            viewModel.SelectedVisit = null;

            Assert.IsTrue(eventRaised);
        }

        [TestMethod]
        public void PropertyChanged_IsRaisedForSelectedVisitWhenSelectedVisitPropertyHasChanged()
        {
            var viewModel = new VisitViewModel(_mock.Object);

            var eventRaised = false;

            viewModel.PropertyChanged += (sender, e) =>
            {
                if (e.PropertyName == "SelectedVisit")
                    eventRaised = true;
            };

            viewModel.SelectedVisit = null;

            Assert.IsTrue(eventRaised);
        }
    }
}