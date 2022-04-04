using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace Domains.Entities.DTOs
{
    public class ErrorResponse
    {
        public bool ActionSuccessful { get; set; }
        public string ErrorMessage { get; set; }
    }
}
