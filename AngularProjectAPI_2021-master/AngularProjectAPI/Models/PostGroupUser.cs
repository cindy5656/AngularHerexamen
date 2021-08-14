using System;
using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;

namespace AngularProjectAPI.Models
{
    public class PostGroupUser
    {
        public int PostGroupUserID { get; set; }
        public Post post { get; set; }
        public int PostID { get; set; }
        public Group group { get; set; }
        public int GroupID { get; set; }
        public User user { get; set; }
        public int UserID { get; set; }
    }
}
