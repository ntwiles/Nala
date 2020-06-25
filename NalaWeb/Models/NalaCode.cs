using System;
using System.Collections.Generic;
using System.ComponentModel.DataAnnotations;
using System.Linq;
using System.Threading.Tasks;

namespace NalaWeb.Models
{
    public class NalaCode
    {
        [Required(ErrorMessage = "You must submit nala code.")]
        public string Content { get; set; }
    }
}
