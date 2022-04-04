using System.Collections.Generic;
using System.ComponentModel.DataAnnotations;
using System.ComponentModel.DataAnnotations.Schema;

namespace Domains.Entities.FolderDbModels
{
    public class Folders
    {
        [Key]
        [DatabaseGenerated(DatabaseGeneratedOption.Identity)]
        public long Id { get; set; }
        [Required]
        [MaxLength(150)]
        public string Name { get; set; }
        [Required]
        public string ParentNode { get; set; }
        [Required]
        public string NodeId { get; set; }
        [Required]
        public int ChildrenFoldersCount { get; set; }
        public virtual ICollection<Files> Files { get; set; }
    }
}