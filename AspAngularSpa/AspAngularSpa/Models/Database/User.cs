using System;
using System.Collections.Generic;
using System.ComponentModel.DataAnnotations;
using System.Linq;
using System.Web;

namespace AspAngularSpa.Models.Database
{
    public class User
    {
        public long Id { get; set; }
        [Required]
        public string Name { get; set; }
        public string EGN { get; set; }
    }
}