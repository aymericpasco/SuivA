using System;
using System.Linq;
using SuivA.Data.Context;
using SuivA.Data.Entity;
using SuivA.Data.Utility;

namespace SuivA.DesktopClient.Service
{
    public class StatisticService
    {
        public static StatisticUtility.DoctorStatistic GetDoctorStatistics(Doctor doctor)
        {
            using (var gc = new GlobalAppContext())
            {
                return new StatisticUtility.DoctorStatistic
                {
                    NumberVisitsThisWeek = (from v in gc.DataContext.Visits
                        where v.Doctor.Id == doctor.Id &&
                              v.VisitDate <= DateTime.Now &&
                              v.VisitDate >= DateTime.Now.AddDays(DayOfWeek.Monday - DateTime.Now.DayOfWeek)
                        select v).Count(),

                    NumberVisitsLastWeek = (from v in gc.DataContext.Visits
                        where v.Doctor.Id == doctor.Id &&
                              v.VisitDate < DateTime.Now.AddDays(DayOfWeek.Monday - DateTime.Now.DayOfWeek) &&
                              v.VisitDate >= DateTime.Now.AddDays(DayOfWeek.Monday - DateTime.Now.DayOfWeek - 7)
                        select v).Count(),

                    NumberVisitsThisMonth = (from v in gc.DataContext.Visits
                        where v.Doctor.Id == doctor.Id &&
                              v.VisitDate <= DateTime.Now &&
                              v.VisitDate >= new DateTime(DateTime.Now.Year, DateTime.Now.Month, 1)
                        select v).Count(),

                    NumberVisitsThisYear = (from v in gc.DataContext.Visits
                        where v.Doctor.Id == doctor.Id &&
                              v.VisitDate <= DateTime.Now &&
                              v.VisitDate >= new DateTime(DateTime.Now.Year, 1, 1)
                        select v).Count()
                };
            }
        }


        public static StatisticUtility.VisitorStatistic GetVisitorStatistics(User user)
        {
            using (var gc = new GlobalAppContext())
            {
                return new StatisticUtility.VisitorStatistic
                {
                    NumberVisitsThisWeek = (from v in gc.DataContext.Visits
                        where v.User.Id == user.Id &&
                              v.VisitDate <= DateTime.Now &&
                              v.VisitDate >= DateTime.Now.AddDays(DayOfWeek.Monday - DateTime.Now.DayOfWeek)
                        select v).Count(),

                    NumberVisitsThisMonth = (from v in gc.DataContext.Visits
                        where v.User.Id == user.Id &&
                              v.VisitDate <= DateTime.Now &&
                              v.VisitDate >= new DateTime(DateTime.Now.Year, DateTime.Now.Month, 1)
                        select v).Count()
                };
            }
        }
    }
}