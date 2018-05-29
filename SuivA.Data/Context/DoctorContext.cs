using System;
using System.Collections.Generic;
using System.Linq;
using SuivA.Data.Context.Interface;
using SuivA.Data.Entity;

namespace SuivA.Data.Context
{
    public sealed class DoctorContext : GlobalAppContext, IDoctorContext
    {
        public void CreateDoctor(Doctor doctor)
        {
            Check.Require(doctor.Firstname);
            Check.Require(doctor.Lastname);
            Check.Require(doctor.Phone);

            doctor.CreatedAt = DateTime.Now;

            DataContext.Doctors.Add(doctor);
            DataContext.SaveChanges();
        }

        public void UpdateDoctor(Doctor doctor)
        {
            var entity = DataContext.Doctors.Find(doctor.Id);

            if (entity == null) throw new NotImplementedException("Handle appropriately for API design.");

            Check.Require(doctor.Firstname);
            Check.Require(doctor.Lastname);
            Check.Require(doctor.Phone);

            doctor.UpdatedAt = DateTime.Now;

            DataContext.Entry(doctor).CurrentValues.SetValues(doctor);
            DataContext.SaveChanges();
        }

        public ICollection<Doctor> GetDoctorList()
        {
            return DataContext.Doctors.OrderByDescending(p => p.Id).ToArray();
        }
    }
}