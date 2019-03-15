using System.ComponentModel.DataAnnotations;
using System.ComponentModel.DataAnnotations.Schema;

namespace Campus.Core.Models
{
    public enum Gender
    {
        Male,
        Female
    }

    [Table("Students")]
    public class Student
    {
        public int Id { get; set; }

        [Required]
        public string FirstName { get; set; }

        [Required]
        public string LastName { get; set; }

        [Required]
        public Gender Gender { get; set; }

        public virtual Faculty Faculty { get; set; }

        public virtual Room Room { get; set; }

        public string FullName => FirstName + " " + LastName;
    }
}