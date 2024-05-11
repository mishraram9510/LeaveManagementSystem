using System;
using System.Collections.Generic;
using System.ComponentModel;
using System.Linq;
using System.Web;

namespace LeaveApprovalSystem.Models
{
    public class Leave
    {
        public int id { get; set; }

        [DisplayName("Employee Name")]
        public int emp_id { get; set; }

        [DisplayName("From Date")]
        public DateTime from_date {  get; set; }

        [DisplayName("To Date")]
        public DateTime to_date { get; set;}

        [DisplayName("Leave Type")]
        public string leave_type { get; set; }

        [DisplayName("Reason")]
        public string reason {get; set;}
        public string status { get; set; }
        public string Name { get; set; }
        public string Username { get; set; }
        public string LeaveError { get; set; }
    }
}