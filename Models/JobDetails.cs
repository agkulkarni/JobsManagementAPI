using System;
using System.Collections.Generic;
using System.Linq;
using System.Web;

namespace JobsManagementAPI.Models
{
    public class JobDetails
    {
        public long id { get; set; }
        public string code { get; set; }
        public string title { get; set; }
        public string description { get; set; }
        public Location location { get; set; }
        public Department department { get; set; }
        public DateTime postedtDate { get; set; }
        public DateTime closingDate { get; set; }
    }
}