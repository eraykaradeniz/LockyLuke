using System.ComponentModel.DataAnnotations;
using System.ComponentModel.DataAnnotations.Schema;

namespace LockyLuke.Web.Entities
{
    public class Information:BaseEntity
    {
        [Key]
        [DatabaseGenerated(DatabaseGeneratedOption.Identity)]
        public Guid Id { get; set; }
        public string Site { get; set; }
        public string UserName { get; set; }
        public string UserId { get; set; }
        public string Password { get; set; } 
        public string? Description { get; set; }

    }
}
