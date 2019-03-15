using System.Collections.Generic;
using System.ComponentModel.DataAnnotations;
using System.ComponentModel.DataAnnotations.Schema;

namespace Campus.Core.Models
{
    [Table("Rooms")]
    public class Room
    {
        public int Id { get; set; }

        [Required]
        public int Number { get; set; }

        [Required]
        public int Capacity { get; set; }

        public virtual ICollection<Student> Tenants { get; set; } = new HashSet<Student>();
        public virtual Dorm Dorm { get; set; }
    }
}