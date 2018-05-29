using System;
using System.Collections.Generic;
using System.ComponentModel.DataAnnotations;
using System.ComponentModel.DataAnnotations.Schema;
using System.Runtime.Serialization;
using SuivA.Data.Utility;

namespace SuivA.Data.Entity
{
    [DataContract]
    [Table("offices")]
    public sealed class Office
    {
        public Office()
        {
            Doctors = new HashSet<Doctor>();
        }

        [DataMember] [Key] [Column("id")] public long Id { get; set; }

        [DataMember] [Column("street_number")] public int StreetNumber { get; set; }

        [DataMember] [Column("street_name")] public string StreetName { get; set; }

        [DataMember] [Column("city")] public string City { get; set; }

        [DataMember] [Column("zip_code")] public int ZipCode { get; set; }

        [DataMember] [Column("created_at")] public DateTime? CreatedAt { get; set; }

        [DataMember] [Column("updated_at")] public DateTime? UpdatedAt { get; set; }

        [DataMember] public ICollection<Doctor> Doctors { get; set; }


        [NotMapped] public GpsUtility.Location Gps { get; set; }
    }
}