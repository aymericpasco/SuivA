using System;
using System.Collections.Generic;
using System.ComponentModel.DataAnnotations;
using System.ComponentModel.DataAnnotations.Schema;
using System.Runtime.Serialization;
using SuivA.Data.Utility;

namespace SuivA.Data.Entity
{
    [DataContract]
    [Table("users")]
    public sealed class User
    {
        public User()
        {
            Doctors = new HashSet<Doctor>();
            Visits = new HashSet<Visit>();
        }

        [DataMember] [Key] [Column("id")] public long Id { get; set; }

        [DataMember] [Column("firstname")] public string Firstname { get; set; }

        [DataMember] [Column("lastname")] public string Lastname { get; set; }

        [DataMember] [Column("username")] public string Username { get; set; }

        [DataMember] [Column("email")] public string Email { get; set; }

        [DataMember] [Column("password")] public string Password { get; set; }

        [DataMember] [Column("address")] public string Address { get; set; }

        [DataMember] [Column("zip")] public string Zip { get; set; }

        [DataMember] [Column("city")] public string City { get; set; }

        [DataMember] [Column("hiring_date")] public DateTime? HiringDate { get; set; }

        [DataMember]
        [Column("remember_token")]
        public string RememberToken { get; set; }

        [DataMember] [Column("created_at")] public DateTime? CreatedAt { get; set; }

        [DataMember] [Column("updated_at")] public DateTime? UpdatedAt { get; set; }

        [DataMember] [Column("role_id")] public long RoleId { get; set; }


        [DataMember] public ICollection<Doctor> Doctors { get; set; }

        [DataMember] public Role Role { get; set; }

        [DataMember] public ICollection<Visit> Visits { get; set; }


        [NotMapped] public StatisticUtility.VisitorStatistic Statistics { get; set; }
    }
}