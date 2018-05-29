using System;
using System.Collections.Generic;
using System.Collections.ObjectModel;
using System.Windows.Input;
using SuivA.Data.Context.Interface;
using SuivA.Data.Entity;
using SuivA.DesktopClient.Service;
using SuivA.Windows;

namespace SuivA.DesktopClient.ViewModel
{
    public class OfficeViewModel : Windows.ViewModel
    {
        private readonly IOfficeContext _context;
        private Office _selectedOffice;

        public OfficeViewModel(IOfficeContext context)
        {
            Offices = new ObservableCollection<Office>();
            _context = context;
        }

        public bool CanModify => SelectedOffice != null;

        public ICollection<Office> Offices { get; }

        public Office SelectedOffice
        {
            get => _selectedOffice;
            set
            {
                SetProperty(ref _selectedOffice, value);
                if (_selectedOffice != null) _selectedOffice.Gps = GpsService.GetCoordinates(_selectedOffice);
                NotifyPropertyChanged();
                NotifyPropertyChanged($"CanModify");
            }
        }


        public bool IsValid => SelectedOffice == null ||
                               !string.IsNullOrWhiteSpace(SelectedOffice.StreetName) &&
                               !string.IsNullOrWhiteSpace(SelectedOffice.City) &&
                               !(SelectedOffice.StreetNumber <= 0) &&
                               !(SelectedOffice.ZipCode <= 0);

        public ICommand GetOfficeListCommand
        {
            get { return new ActionCommand(p => GetOfficeList()); }
        }

        public ICommand AddCommand
        {
            get
            {
                return new ActionCommand(p => AddOffice(),
                    p => IsValid);
            }
        }

        public ICommand UpdateCommand
        {
            get
            {
                return new ActionCommand(p => SaveOffice(),
                    p => IsValid);
            }
        }

        private void GetOfficeList()
        {
            Offices.Clear();
            SelectedOffice = null;

            foreach (var office in _context.GetOfficeList())
                Offices.Add(office);
        }

        private void AddOffice()
        {
            var office = new Office
            {
                StreetNumber = 1,
                StreetName = "Nom de la rue",
                City = "Nom de la ville",
                ZipCode = 00001
            };

            try
            {
                _context.CreateOffice(office);
            }
            catch (Exception)
            {
                //TODO: cover error handling
                return;
            }

            Offices.Add(office);
        }

        private void SaveOffice()
        {
            _context.UpdateOffice(SelectedOffice);
        }
    }
}