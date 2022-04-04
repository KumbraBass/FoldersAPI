using System.ComponentModel.DataAnnotations;

namespace Domains.Entities.DTOs
{
    public class AddNewFileRequest
    {
        [Required]
        public string ParentNodeId { get; set; }
        [Required]
        public string FileName { get; set; }
    }
}
