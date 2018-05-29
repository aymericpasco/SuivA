using System;
using System.Collections.Generic;
using System.Collections.ObjectModel;
using System.Globalization;
using System.Linq;
using System.Windows.Input;
using SuivA.Data.Context.Interface;
using SuivA.Data.Entity;
using SuivA.Data.Utility.Session;
using SuivA.Windows;

namespace SuivA.DesktopClient.ViewModel
{
    public class VisitViewModel : Windows.ViewModel
    {
        private readonly IVisitContext _context;
        private Visit _selectedVisit;


        public VisitViewModel(IVisitContext context)
        {
            Visits = new ObservableCollection<Visit>();
            _context = context;
        }

        public ICollection<Visit> Visits { get; }

        public ICollection<Doctor> Doctors =>
            (from d in _context.DataContext.Doctors where d.User.Id == UserSession.Id select d).ToList();

        public bool CanModify => SelectedVisit != null;

        public Visit SelectedVisit
        {
            get => _selectedVisit;
            set
            {
                SetProperty(ref _selectedVisit, value);
                NotifyPropertyChanged();
                NotifyPropertyChanged($"CanModify");
            }
        }

        public bool IsValid => SelectedVisit == null ||
                               SelectedVisit.VisitDate > DateTime.Now.AddYears(-1) &&
                               SelectedVisit.VisitDate <= DateTime.Now &&
                               (Convert.ToBoolean(SelectedVisit.Appointment) == false ||
                                Convert.ToBoolean(SelectedVisit.Appointment)) &&
                               SelectedVisit.ArrivingTime >= TimeSpan.ParseExact("00:00:00", "hh\\:mm\\:ss",
                                   CultureInfo.InvariantCulture) &&
                               SelectedVisit.ArrivingTime <= TimeSpan.ParseExact("23:59:59", "hh\\:mm\\:ss",
                                   CultureInfo.InvariantCulture) &&
                               (SelectedVisit.StartTimeInterview >= SelectedVisit.ArrivingTime ||
                                SelectedVisit.StartTimeInterview.HasValue == false) &&
                               (SelectedVisit.DepartureTime >= SelectedVisit.StartTimeInterview ||
                                SelectedVisit.DepartureTime.HasValue == false);

        public ICommand GetVisitListCommand
        {
            get { return new ActionCommand(p => GetVisitList()); }
        }

        public ICommand AddCommand
        {
            get
            {
                return new ActionCommand(p => AddVisit(),
                    p => IsValid);
            }
        }

        public ICommand DeleteCommand
        {
            get { return new ActionCommand(p => DeleteVisit()); }
        }

        public ICommand UpdateCommand
        {
            get
            {
                return new ActionCommand(p => SaveVisit(),
                    p => IsValid);
            }
        }

        private void GetVisitList()
        {
            Visits.Clear();
            SelectedVisit = null;

            foreach (var visit in _context.GetVisitList())
                Visits.Add(visit);
        }

        private void AddVisit()
        {
            var visit = new Visit
            {
                VisitDate = DateTime.Now,
                Appointment = Convert.ToSByte(false),
                ArrivingTime = DateTime.Now.TimeOfDay,
                StartTimeInterview = null,
                DepartureTime = null,
                User = (from u in _context.DataContext.Users where u.Id == UserSession.Id select u).First(),
                Doctor = (from d in _context.DataContext.Doctors where d.User.Id == UserSession.Id select d).First()
            };

            try
            {
                _context.CreateVisit(visit);
            }
            catch (Exception)
            {
                //TODO: cover error handling
                return;
            }

            Visits.Add(visit);
        }

        private void SaveVisit()
        {
            _context.UpdateVisit(SelectedVisit);
        }

        private void DeleteVisit()
        {
            _context.DeleteVisit(SelectedVisit);
            Visits.Remove(SelectedVisit);
            SelectedVisit = null;
        }
    }
}