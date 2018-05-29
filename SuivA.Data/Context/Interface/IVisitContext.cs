using System.Collections.Generic;
using SuivA.Data.Entity;

namespace SuivA.Data.Context.Interface
{
    public interface IVisitContext
    {
        DataContext DataContext { get; }

        void CreateVisit(Visit visit);

        void DeleteVisit(Visit visit);

        ICollection<Visit> GetVisitList();

        void UpdateVisit(Visit visit);
    }
}