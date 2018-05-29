using System;
using System.ComponentModel.DataAnnotations;
using System.ComponentModel.DataAnnotations.Schema;
using System.Runtime.Serialization;

namespace SuivA.Data.Entity
{
    [DataContract]
    [Table("visits")]
    public class Visit
    {
        [DataMember] [Key] [Column("id")] public long Id { get; set; }

        [DataMember] [Column("visit_date")] public DateTime VisitDate { get; set; }

        [DataMember] [Column("appointment")] public sbyte Appointment { get; set; }

        [DataMember] [Column("arriving_time")] public TimeSpan ArrivingTime { get; set; }

        [DataMember]
        [Column("start_time_interview")]
        public TimeSpan? StartTimeInterview { get; set; }

        [DataMember]
        [Column("departure_time")]
        public TimeSpan? DepartureTime { get; set; }

        [DataMember] [Column("created_at")] public DateTime? CreatedAt { get; set; }

        [DataMember] [Column("updated_at")] public DateTime? UpdatedAt { get; set; }

        [DataMember] [Column("user_id")] public long UserId { get; set; }

        [DataMember] [Column("doctor_id")] public long DoctorId { get; set; }


        [DataMember] public virtual Doctor Doctor { get; set; }

        [DataMember] public virtual User User { get; set; }
    }
}