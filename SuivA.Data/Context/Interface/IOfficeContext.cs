using System.Collections.Generic;
using SuivA.Data.Entity;

namespace SuivA.Data.Context.Interface
{
    public interface IOfficeContext
    {
        DataContext DataContext { get; }

        void CreateOffice(Office office);

        //OfficeContext.GeocoderLocation GetGspCoordinates(offices office);

        ICollection<Office> GetOfficeList();

        void UpdateOffice(Office office);
    }
}