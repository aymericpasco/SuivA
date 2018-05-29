using System.Collections.Generic;
using System.Collections.ObjectModel;
using System.Linq;
using System.Windows.Input;
using SuivA.Data.Context.Interface;
using SuivA.Data.Entity;
using SuivA.Windows;

namespace SuivA.DesktopClient.ViewModel
{
    public class DoctorViewModel : Windows.ViewModel
    {
        private readonly IDoctorContext _context;
        private Doctor _selectedDoctor;

        public DoctorViewModel(IDoctorContext context)
        {
            Doctors = new ObservableCollection<Doctor>();
            _context = context;
        }

        public bool CanModify => SelectedDoctor != null;

        public ICollection<Doctor> Doctors { get; }

        public ICollection<User> Visitors =>
            (from u in _context.DataContext.Users where u.Role.Name == "visiteur" select u).ToList();

        public ICollection<Office> Offices => (from o in _context.DataContext.Offices select o).ToList();

        public Doctor SelectedDoctor
        {
            get => _selectedDoctor;
            set
            {
                SetProperty(ref _selectedDoctor, value);
                NotifyPropertyChanged();
                NotifyPropertyChanged($"CanModify");
            }
        }

        public bool IsValid => SelectedDoctor == null ||
                               !string.IsNullOrWhiteSpace(SelectedDoctor.Firstname) &&
                               !string.IsNullOrWhiteSpace(SelectedDoctor.Lastname) &&
                               !string.IsNullOrWhiteSpace(SelectedDoctor.Phone);

        public ICommand GetDoctorListCommand
        {
            get { return new ActionCommand(p => GetDoctorList()); }
        }

        public ICommand AddCommand
        {
            get
            {
                return new ActionCommand(p => AddDoctor(),
                    p => IsValid);
            }
        }

        public ICommand UpdateCommand
        {
            get
            {
                return new ActionCommand(p => SaveDoctor(),
                    p => IsValid);
            }
        }

        private void GetDoctorList()
        {
            Doctors.Clear();
            SelectedDoctor = null;

            foreach (var doctor in _context.GetDoctorList())
                Doctors.Add(doctor);
        }

        private void AddDoctor()
        {
            var doctor = new Doctor
            {
                Firstname = "Prénom medecin",
                Lastname = "Nom medecin",
                Phone = "+33 6 00 00 00 00",
                Specialty = null,
                User = (from u in _context.DataContext.Users select u).First(),
                Office = (from o in _context.DataContext.Offices select o).First()
            };

            try
            {
                _context.CreateDoctor(doctor);
            }
            catch
            {
                //TODO: cover error handling
                return;
            }

            Doctors.Add(doctor);
        }

        private void SaveDoctor()
        {
            _context.UpdateDoctor(SelectedDoctor);
        }
    }
}