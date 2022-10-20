using System;
using System.Collections.Generic;
using System.Linq;
using System.Web;

namespace JobsManagementAPI.Models
{
    public class JobDet
    {
        public long id { get; set; }
        public string code { get; set; }
        public string title { get; set; }
        public string location { get; set; }
        public string department { get; set; }
        public DateTime postedDate { get; set; }
        public DateTime closingDate { get; set; }
    }
}