using System;
using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;

namespace AngularProjectAPI.Models
{
    public class Group
    {
        public int GroupID { get; set; }
        public string Name { get; set; }
        public string FotoURL { get; set; }
        public string Theme { get; set; }
        public int GroupManagerID { get; set; } 
        public User GroupManager { get; set; } //beheerder
        public ICollection<User> GroupUsers { get; set; }// leden
    }
}
