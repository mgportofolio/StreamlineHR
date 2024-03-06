using System.ComponentModel.DataAnnotations.Schema;
using System.ComponentModel.DataAnnotations;
using Microsoft.Extensions.Hosting;

namespace StreamlineHR.Entities
{
    public class Candidate
    {
        public Candidate()
        {
            DateTime localDateTime = DateTime.Now;
            CreatedAt = localDateTime.ToUniversalTime();
        }

        [Key]
        [DatabaseGenerated(DatabaseGeneratedOption.Identity)]
        public int Id { get; set; }

        [Required]
        public required string Name { get; set; }

        [Required]
        public required string About { get; set; }

        [Required]
        public required string Email { get; set; }

        [Required]
        public required string Phone { get; set; }

        [Required]
        public DateTime CreatedAt { get; set; }
    }
}
