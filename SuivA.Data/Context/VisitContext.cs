using System;
using System.Collections.Generic;
using System.Linq;
using SuivA.Data.Context.Interface;
using SuivA.Data.Entity;
using SuivA.Data.Utility.Session;

namespace SuivA.Data.Context
{
    public sealed class VisitContext : GlobalAppContext, IVisitContext
    {
        public void CreateVisit(Visit visit)
        {
            Check.ValidDate(visit.VisitDate);

            if (visit.StartTimeInterview.HasValue)
                Check.ValidTimeSuperior(visit.ArrivingTime, visit.StartTimeInterview);
            if (visit.DepartureTime.HasValue)
                Check.ValidTimeSuperior(visit.StartTimeInterview, visit.DepartureTime);

            // Check.ValidVisitor(visit.User);

            visit.User = (from u in DataContext.Users where u.Id == UserSession.Id select u).First();

            Check.ValidDoctor(visit.Doctor, visit.User);

            visit.CreatedAt = DateTime.Now;

            DataContext.Visits.Add(visit);
            DataContext.SaveChanges();
        }

        public void DeleteVisit(Visit visit)
        {
            DataContext.Visits.Remove(visit);
            DataContext.SaveChanges();
        }

        public void UpdateVisit(Visit visit)
        {
            var entity = DataContext.Visits.Find(visit.Id);

            if (entity == null) throw new NotImplementedException("Handle appropriately for API design.");

            Check.ValidDate(visit.VisitDate);

            //Check.ValidVisitor(visit.User);
            Check.ValidDoctor(visit.Doctor, visit.User);

            if (visit.StartTimeInterview.HasValue)
                Check.ValidTimeSuperior(visit.ArrivingTime, visit.StartTimeInterview);
            if (visit.DepartureTime.HasValue)
                Check.ValidTimeSuperior(visit.StartTimeInterview, visit.DepartureTime);

            visit.UpdatedAt = DateTime.Now;

            DataContext.Entry(visit).CurrentValues.SetValues(visit);
            DataContext.SaveChanges();
        }

        public ICollection<Visit> GetVisitList()
        {
//            return (from v in DataContext.Visits where v.Id == UserSession.Id select v)
//                .OrderByDescending(p => p.VisitDate).ToArray();
            //return DataContext.Visits.OrderByDescending(p => p.VisitDate).ToArray();
            return DataContext.Visits.Where(p => p.User.Id == UserSession.Id).OrderByDescending(p => p.VisitDate)
                .ToArray();
        }
    }
}