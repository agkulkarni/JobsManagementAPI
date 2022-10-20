using System;
using System.Collections.Generic;
using System.Linq;
using System.Web;

namespace JobsManagementAPI.Models
{
    public class JobsList
    {
        public int total { get; set; }
        public List<JobDet> data { get; set; }
    }
}