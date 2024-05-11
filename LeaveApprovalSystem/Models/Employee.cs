using System;
using System.Collections.Generic;
using System.ComponentModel.DataAnnotations;
using System.Linq;
using System.Web;

namespace LeaveApprovalSystem.Models
{
    public class Employee
    {
        public int id { get; set; }
        public string Name { get; set; }
        public string Designation { get; set; }
        public string Username { get; set; }
        public string Password { get; set; }
    }
}