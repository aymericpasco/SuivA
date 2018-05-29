using System.Collections.Generic;
using System.ComponentModel.DataAnnotations;
using System.ComponentModel.DataAnnotations.Schema;
using System.Runtime.Serialization;

namespace SuivA.Data.Entity
{
    [DataContract]
    [Table("roles")]
    public sealed class Role
    {
        public Role()
        {
            Users = new HashSet<User>();
        }

        [DataMember] [Key] [Column("id")] public long Id { get; set; }

        [DataMember] [Column("name")] public string Name { get; set; }

        [DataMember] [Column("display_name")] public string DisplayName { get; set; }


        [DataMember] public ICollection<User> Users { get; set; }
    }
}