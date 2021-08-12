using System;
using System.Collections.Generic;
using System.ComponentModel.DataAnnotations;
using System.Linq;
using System.Threading.Tasks;

namespace AngularProjectAPI.Models
{
    public class Company
    {
        [Key]
        public int CompanyID { get; set; }
        public string NameCompany { get; set; }
        public string Description { get; set; }
        public string Location { get; set; }
        public string fotoURL { get; set; }
        public int CompanyManagerID { get; set; }
    }
}
