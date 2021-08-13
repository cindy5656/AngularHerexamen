using System;
using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;

namespace AngularProjectAPI.Models
{
    public class Post
    {
        public int PostID { get; set; }
        public string Subject { get; set; }
        public string Content { get; set; }
        public string FotoURL { get; set; }
    }
}
