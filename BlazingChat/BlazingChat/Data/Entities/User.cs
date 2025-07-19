using Microsoft.EntityFrameworkCore;
using System.ComponentModel.DataAnnotations;
using System.ComponentModel.DataAnnotations.Schema;

namespace BlazingChat.Server.Data.Entities
{
    [Table("User")]
    public class User
    {
        [Key]
        public int Id { get; set; }

        [Required, Unicode(false), MaxLength(25)]
        public required string Name { get; set; }      // added 'required'

        public DateTime AddedOn { get; set; }

        [Required, Unicode(false), MaxLength(50)]
        public required string Username { get; set; } // added 'required'

        [Required, MaxLength(20)]
        public required string Password { get; set; } // added 'required'
    }
}
