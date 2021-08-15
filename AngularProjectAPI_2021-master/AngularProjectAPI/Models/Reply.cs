using System;
using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;

namespace AngularProjectAPI.Models
{
    public class Reply
    {
        public int ReplyID { get; set; }
        public string ReplyContent { get; set; }
        public int PostID { get; set; }
        public Post post { get; set; }
    }
}
