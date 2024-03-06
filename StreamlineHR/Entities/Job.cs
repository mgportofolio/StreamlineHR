using System.ComponentModel.DataAnnotations.Schema;
using System.ComponentModel.DataAnnotations;
using System.ComponentModel;

namespace StreamlineHR.Entities
{
    public class Job
    {
        public Job()
        {
            DateTime localDateTime = DateTime.Now;
            CreatedAt = localDateTime.ToUniversalTime();
        }

        [Key]
        [DatabaseGenerated(DatabaseGeneratedOption.Identity)]
        public int Id { get; set; }

        [Required]
        public required string Title { get; set; }

        [Required]
        public required string Content { get; set; }

        [Required]
        [DefaultValue(0)]
        public int Status { get; set; }

        [Required]
        [DefaultValue(1)]
        public int CreatedBy { get; set; }

        [Required]
        public DateTime CreatedAt { get; set; }
    }
}
