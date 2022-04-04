using System;
using System.Collections.Generic;
using System.ComponentModel.DataAnnotations;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace Domains.Entities.DTOs
{
    public class AddNewFolderRequest
    {
        [Required]
        public string ParentNodeId { get; set; }
        [Required]
        public string FolderName { get; set; }
    }
}
