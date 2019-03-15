using System.Collections.Generic;
using System.ComponentModel.DataAnnotations;
using System.ComponentModel.DataAnnotations.Schema;

namespace Campus.Core.Models
{
    [Table("Dorms")]
    public class Dorm
    {
        public int Id { get; set; }

        [Required]
        public int Number { get; set; }

        public virtual ICollection<Faculty> AllowedFaculties { get; set; } = new HashSet<Faculty>();

        public virtual ICollection<Room> Rooms { get; set; } = new HashSet<Room>();
    }
}