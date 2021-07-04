using System;
using System.Collections.Generic;
using System.ComponentModel.DataAnnotations.Schema;
using System.Linq;
using System.Text.Json.Serialization;
using System.Threading.Tasks;

namespace AngularProjectAPI.Models
{
    public class GroupUser
    {
        public int GroupUserID { get; set; }
        public ICollection<Group> Groups { get; set; }

        //Relations
        [JsonIgnore]
        public ICollection<User> Users { get; set; }
    }
}
