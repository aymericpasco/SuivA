using System.Collections.Generic;
using SuivA.Data.Entity;

namespace SuivA.Data.Context.Interface
{
    public interface IDoctorContext
    {
        DataContext DataContext { get; }

        void CreateDoctor(Doctor doctor);

        ICollection<Doctor> GetDoctorList();

        void UpdateDoctor(Doctor doctor);
    }
}