using System.ComponentModel.DataAnnotations;

namespace StreamlineHR.Models
{
    /// <summary>
    /// Inserted model request for job
    /// </summary>
    public class JobInsertModel
    {
        [Required]
        public required string Title { get; set; }

        [Required]
        public required string Content { get; set; }
    }
}
