using System;
using System.Collections.Generic;
using System.Linq;
using System.Web;

namespace JobsManagementAPI.Models
{
    public class JobsListRequest
    {
        public string q { get; set; }
        public int pageNo { get; set; }
        public int pageSize { get; set; }
        public long locationId { get; set; }
        public long departmentId { get; set; }
    }
}