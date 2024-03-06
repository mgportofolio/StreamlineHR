using System.ComponentModel.DataAnnotations;

namespace StreamlineHR.Models
{
    /// <summary>
    /// Model to create candidate and candidate applied to job
    /// </summary>
    public class CandidateAppliesJob
    {
        [Required]
        public required string Name { get; set; }

        [Required]
        public required string About { get; set; }

        [Required]
        public required string Email { get; set; }

        [Required]
        public required string Phone { get; set; }

        [Required]
        public required int JobId { get; set; }
    }
}
