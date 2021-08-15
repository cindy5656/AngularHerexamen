using System;
using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;

namespace AngularProjectAPI.Models
{
    public class PostDislikedBy
    {
        public int PostDislikedByID { get; set; }
        public int PostID { get; set; }
        public Post post { get; set; }
        public User user { get; set; }
        public int UserID { get; set; }
    }
}
