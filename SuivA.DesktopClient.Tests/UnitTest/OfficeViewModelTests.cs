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
    public class OfficeViewModelTests
    {
        private Mock<IOfficeContext> _mock;
        private List<Office> _store;

        [TestInitialize]
        public void TestInitialize()
        {
            _store = new List<Office>();

            _mock = new Mock<IOfficeContext>();
            _mock.Setup(m => m.GetOfficeList()).Returns(_store);
            _mock.Setup(m => m.CreateOffice(It.IsAny<Office>())).Callback<Office>(office => _store.Add(office));
            _mock.Setup(m => m.UpdateOffice(It.IsAny<Office>())).Callback<Office>(office =>
            {
                var i = _store.IndexOf(office);
                _store[i] = office;
            });
        }

        [TestMethod]
        public void IsViewModel()
        {
            Assert.IsTrue(typeof(OfficeViewModel).BaseType == typeof(Windows.ViewModel));
        }

        [TestMethod]
        public void AddCommand_CannotExecuteWhenStreetNumberIsNotValid()
        {
            var viewModel = new OfficeViewModel(_mock.Object)
            {
                SelectedOffice = new Office
                {
                    StreetNumber = Convert.ToInt32(null),
                    StreetName = "rue Saint André",
                    City = "Lille",
                    ZipCode = 59800
                }
            };

            Assert.IsFalse(viewModel.AddCommand.CanExecute(null));
        }

        [TestMethod]
        public void AddCommand_CannotExecuteWhenZipCodeIsNotValid()
        {
            var viewModel = new OfficeViewModel(_mock.Object)
            {
                SelectedOffice = new Office
                {
                    StreetNumber = 124,
                    StreetName = "rue Saint André",
                    City = "Lille",
                    ZipCode = Convert.ToInt32(null)
                }
            };

            Assert.IsFalse(viewModel.AddCommand.CanExecute(null));
        }

        [TestMethod]
        public void AddCommand_CannotExecuteWhenStreetNameIsNotValid()
        {
            var viewModel = new OfficeViewModel(_mock.Object)
            {
                SelectedOffice = new Office
                {
                    StreetNumber = 124,
                    StreetName = null,
                    City = "Lille",
                    ZipCode = 59800
                }
            };

            Assert.IsFalse(viewModel.AddCommand.CanExecute(null));
        }

        [TestMethod]
        public void AddCommand_CannotExecuteWhenCityIsNotValid()
        {
            var viewModel = new OfficeViewModel(_mock.Object)
            {
                SelectedOffice = new Office
                {
                    StreetNumber = 124,
                    StreetName = "rue Saint André",
                    City = null,
                    ZipCode = 59800
                }
            };

            Assert.IsFalse(viewModel.AddCommand.CanExecute(null));
        }

        [TestMethod]
        public void CanModify_ShouldEqualFalseWhenSelectedOfficeIsNull()
        {
            var viewModel = new OfficeViewModel(_mock.Object) {SelectedOffice = null};
            Assert.IsFalse(viewModel.CanModify);
        }

        [TestMethod]
        public void CanModify_ShouldEqualTrueWhenSelectedOfficeIsNotNull()
        {
            var viewModel = new OfficeViewModel(_mock.Object) {SelectedOffice = new Office()};
            Assert.IsTrue(viewModel.CanModify);
        }

        [TestMethod]
        public void GetOfficeListCommand_PopulatesOfficesPropertyWithExpectedCollectionFromDataStore()
        {
            for (var i = 1; i < 4; ++i)
                _mock.Object.CreateOffice(
                    new Office
                    {
                        StreetNumber = 124,
                        StreetName = "rue Saint André",
                        City = "Lille",
                        ZipCode = 59800
                    }
                );

            var viewModel = new OfficeViewModel(_mock.Object);

            viewModel.GetOfficeListCommand.Execute(null);

            Assert.IsTrue(viewModel.Offices.Count == 3);
        }

        [TestMethod]
        public void GetOfficeListCommand_SelectedOfficeIsSetToNullWhenExecuted()
        {
            var viewModel = new OfficeViewModel(_mock.Object)
            {
                SelectedOffice = new Office()
            };

            viewModel.GetOfficeListCommand.Execute(null);

            Assert.IsNull(viewModel.SelectedOffice);
        }

        [TestMethod]
        public void SaveCommand_InvokesIOfficeContextUpdateOfficeMethod()
        {
            _mock.Object.CreateOffice(
                new Office
                {
                    StreetNumber = 124,
                    StreetName = "rue Saint André",
                    City = "Lille",
                    ZipCode = 59800
                }
            );

            var viewModel = new OfficeViewModel(_mock.Object);

            viewModel.GetOfficeListCommand.Execute(null);
            viewModel.SelectedOffice = viewModel.Offices.First();

            // Act
            viewModel.SelectedOffice.StreetName = "rue des Ecoles";
            viewModel.SelectedOffice.City = "Marcq-en-Baroeul";
            viewModel.SelectedOffice.StreetNumber = 102;
            viewModel.SelectedOffice.ZipCode = 59700;

            viewModel.UpdateCommand.Execute(null);

            // Assert
            _mock.Verify(m => m.UpdateOffice(It.IsAny<Office>()), Times.Once);
        }

        [TestMethod]
        public void PropertyChanged_IsRaisedForCanModifyWhenSelectedOfficePropertyHasChanged()
        {
            var viewModel = new OfficeViewModel(_mock.Object);

            var eventRaised = false;

            viewModel.PropertyChanged += (sender, e) =>
            {
                if (e.PropertyName == "CanModify")
                    eventRaised = true;
            };

            viewModel.SelectedOffice = null;

            Assert.IsTrue(eventRaised);
        }

        [TestMethod]
        public void PropertyChanged_IsRaisedForSelectedOfficeWhenSelectedOfficePropertyHasChanged()
        {
            var viewModel = new OfficeViewModel(_mock.Object);

            var eventRaised = false;

            viewModel.PropertyChanged += (sender, e) =>
            {
                if (e.PropertyName == "SelectedOffice")
                    eventRaised = true;
            };

            viewModel.SelectedOffice = null;

            Assert.IsTrue(eventRaised);
        }
    }
}