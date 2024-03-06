using System.ComponentModel.DataAnnotations.Schema;
using System.ComponentModel.DataAnnotations;

namespace StreamlineHR.Entities
{
    public class Applied
    {
        public Applied()
        {
            DateTime localDateTime = DateTime.Now;
            CreatedAt = localDateTime.ToUniversalTime();
        }

        [Key]
        [DatabaseGenerated(DatabaseGeneratedOption.Identity)]
        public int Id { get; set; }

        public int CandidateId { get; set; }

        public int JobId { get; set; }

        [ForeignKey("JobId")]
        public Job? Job { get; set; }

        [ForeignKey("CandidateId")]
        public Candidate? Candidate { get; set; }

        public DateTime CreatedAt { get; set; }
    }
}
