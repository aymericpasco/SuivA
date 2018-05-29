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
    public class DoctorViewModelTests
    {
        private Mock<IDoctorContext> _mock;
        private List<Doctor> _store;

        [TestInitialize]
        public void TestInitialize()
        {
            _store = new List<Doctor>();

            _mock = new Mock<IDoctorContext>();
            _mock.Setup(m => m.GetDoctorList()).Returns(_store);
            _mock.Setup(m => m.CreateDoctor(It.IsAny<Doctor>())).Callback<Doctor>(doctor => _store.Add(doctor));
            _mock.Setup(m => m.UpdateDoctor(It.IsAny<Doctor>())).Callback<Doctor>(doctor =>
            {
                var i = _store.IndexOf(doctor);
                _store[i] = doctor;
            });
        }

        [TestMethod]
        public void IsViewModel()
        {
            Assert.IsTrue(typeof(DoctorViewModel).BaseType == typeof(Windows.ViewModel));
        }

        [TestMethod]
        public void AddCommand_CannotExecuteWhenFirstNameIsNotValid()
        {
            var viewModel = new DoctorViewModel(_mock.Object)
            {
                SelectedDoctor = new Doctor
                {
                    Firstname = null,
                    Lastname = "John",
                    Phone = "+33 7 84 35 02 95"
                }
            };

            Assert.IsFalse(viewModel.AddCommand.CanExecute(null));
        }

        [TestMethod]
        public void AddCommand_CannotExecuteWhenLastNameIsNotValid()
        {
            var viewModel = new DoctorViewModel(_mock.Object)
            {
                SelectedDoctor = new Doctor
                {
                    Firstname = "Paul",
                    Lastname = null,
                    Phone = "+33 7 84 35 02 95"
                }
            };

            Assert.IsFalse(viewModel.AddCommand.CanExecute(null));
        }

        [TestMethod]
        public void AddCommand_CannotExecuteWhenPhoneIsNotValid()
        {
            var viewModel = new DoctorViewModel(_mock.Object)
            {
                SelectedDoctor = new Doctor
                {
                    Firstname = "Paul",
                    Lastname = "Réant",
                    Phone = null
                }
            };

            Assert.IsFalse(viewModel.AddCommand.CanExecute(null));
        }

        [TestMethod]
        public void CanModify_ShouldEqualFalseWhenSelectedDoctorIsNull()
        {
            var viewModel = new DoctorViewModel(_mock.Object) {SelectedDoctor = null};
            Assert.IsFalse(viewModel.CanModify);
        }

        [TestMethod]
        public void CanModify_ShouldEqualTrueWhenSelectedDoctorIsNotNull()
        {
            var viewModel = new DoctorViewModel(_mock.Object) {SelectedDoctor = new Doctor()};
            Assert.IsTrue(viewModel.CanModify);
        }

        [TestMethod]
        public void GetDoctorListCommand_PopulatesDoctorsPropertyWithExpectedCollectionFromDataStore()
        {
            for (var i = 1; i < 4; ++i)
                _mock.Object.CreateDoctor(
                    new Doctor
                    {
                        Firstname = "Paul",
                        Lastname = "Réant",
                        Phone = "+33 6 67 81 09 58"
                    }
                );

            var viewModel = new DoctorViewModel(_mock.Object);

            viewModel.GetDoctorListCommand.Execute(null);

            Assert.IsTrue(viewModel.Doctors.Count == 3);
        }

        [TestMethod]
        public void GetDoctorListCommand_SelectedDoctorIsSetToNullWhenExecuted()
        {
            var viewModel = new DoctorViewModel(_mock.Object)
            {
                SelectedDoctor = new Doctor()
            };

            viewModel.GetDoctorListCommand.Execute(null);

            Assert.IsNull(viewModel.SelectedDoctor);
        }

        [TestMethod]
        public void SaveCommand_InvokesIDoctorContextUpdateDoctorMethod()
        {
            _mock.Object.CreateDoctor(
                new Doctor
                {
                    Firstname = "Paul",
                    Lastname = "Réant",
                    Phone = "+33 6 67 81 09 58"
                }
            );

            var viewModel = new DoctorViewModel(_mock.Object);

            viewModel.GetDoctorListCommand.Execute(null);
            viewModel.SelectedDoctor = viewModel.Doctors.First();

            // Act
            viewModel.SelectedDoctor.Firstname = "Georges";
            viewModel.SelectedDoctor.Lastname = "Snow";
            viewModel.SelectedDoctor.Phone = "+33 7 85 06 25 45";

            viewModel.UpdateCommand.Execute(null);

            // Assert
            _mock.Verify(m => m.UpdateDoctor(It.IsAny<Doctor>()), Times.Once);
        }

        [TestMethod]
        public void PropertyChanged_IsRaisedForCanModifyWhenSelectedDoctorPropertyHasChanged()
        {
            var viewModel = new DoctorViewModel(_mock.Object);

            var eventRaised = false;

            viewModel.PropertyChanged += (sender, e) =>
            {
                if (e.PropertyName == "CanModify")
                    eventRaised = true;
            };

            viewModel.SelectedDoctor = null;

            Assert.IsTrue(eventRaised);
        }

        [TestMethod]
        public void PropertyChanged_IsRaisedForSelectedDoctorWhenSelectedDoctorPropertyHasChanged()
        {
            var viewModel = new DoctorViewModel(_mock.Object);

            var eventRaised = false;

            viewModel.PropertyChanged += (sender, e) =>
            {
                if (e.PropertyName == "SelectedDoctor")
                    eventRaised = true;
            };

            viewModel.SelectedDoctor = null;

            Assert.IsTrue(eventRaised);
        }
    }
}