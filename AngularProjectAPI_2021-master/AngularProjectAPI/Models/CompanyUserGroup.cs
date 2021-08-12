﻿using System;
using System.Collections.Generic;
using System.ComponentModel.DataAnnotations;
using System.ComponentModel.DataAnnotations.Schema;
using System.Linq;
using System.Threading.Tasks;

namespace AngularProjectAPI.Models
{
    public class CompanyUserGroup
    {
        [Key]
        public int CompanyGroupUserID { get; set; }
        public int CompanyID { get; set; }
        [NotMapped]
        public Company company { get; set; }
        public int? GroupID { get; set; }
        [NotMapped]
        public Group group { get; set; }
        public int UserID { get; set; }
        [NotMapped]
        public User user { get; set; }
        public int RoleID { get; set; }
        [NotMapped]
        public Role role { get; set; }
    }
}
