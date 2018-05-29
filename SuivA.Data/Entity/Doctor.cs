using System;
using System.Collections.Generic;
using System.ComponentModel.DataAnnotations;
using System.ComponentModel.DataAnnotations.Schema;
using System.Runtime.Serialization;
using SuivA.Data.Utility;

namespace SuivA.Data.Entity
{
    [DataContract]
    [Table("doctors")]
    public sealed class Doctor
    {
        public Doctor()
        {
            Visits = new HashSet<Visit>();
        }

        [DataMember] [Key] [Column("id")] public long Id { get; set; }

        [DataMember] [Column("firstname")] public string Firstname { get; set; }

        [DataMember] [Column("lastname")] public string Lastname { get; set; }

        [DataMember] [Column("phone")] public string Phone { get; set; }

        [DataMember] [Column("specialty")] public string Specialty { get; set; }

        [DataMember] [Column("created_at")] public DateTime? CreatedAt { get; set; }

        [DataMember] [Column("updated_at")] public DateTime? UpdatedAt { get; set; }

        [DataMember] [Column("user_id")] public long UserId { get; set; }

        [DataMember] [Column("office_id")] public long OfficeId { get; set; }


        [DataMember] public Office Office { get; set; }

        [DataMember] public User User { get; set; }

        [DataMember] public ICollection<Visit> Visits { get; set; }


        [NotMapped] public StatisticUtility.DoctorStatistic Statistics { get; set; }
    }
}