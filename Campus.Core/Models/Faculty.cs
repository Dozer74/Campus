using System.Collections.Generic;
using System.ComponentModel.DataAnnotations;
using System.ComponentModel.DataAnnotations.Schema;

namespace Campus.Core.Models
{
    [Table("Faculties")]
    public class Faculty
    {
        public int Id { get; set; }

        [Index(IsUnique = true), StringLength(128)]
        public string Title { get; set; }

        public virtual ICollection<Dorm> AllowedDorms { get; set; }

    }
}