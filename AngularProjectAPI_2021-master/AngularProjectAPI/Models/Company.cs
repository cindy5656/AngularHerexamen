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
        public string Name { get; set; }
        public string Description { get; set; }
        public string Location { get; set; }
        public ICollection<User> Users { get; set; }
    }
}
