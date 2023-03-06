using System.ComponentModel;
using System.ComponentModel.DataAnnotations;
﻿using System.ComponentModel.DataAnnotations;

namespace Rocky.Models
{
    public class ApplicationType
    {
        [Key]
        public int AppTypeId { get; set; }
                
        [Required]        
        public string Name { get; set; }
    }
}
