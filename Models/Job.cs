//------------------------------------------------------------------------------
// <auto-generated>
//     This code was generated from a template.
//
//     Manual changes to this file may cause unexpected behavior in your application.
//     Manual changes to this file will be overwritten if the code is regenerated.
// </auto-generated>
//------------------------------------------------------------------------------

namespace JobsManagementAPI.Models
{
    using Newtonsoft.Json;
    using System;
    using System.Collections.Generic;
    using System.ComponentModel;
    using System.Text.Json.Serialization;

    public partial class Job
    {
        [JsonProperty(PropertyName = "id")]
        public long JobId { get; set; }
        [JsonProperty(PropertyName = "title")]
        public string Title { get; set; }
        [JsonProperty(PropertyName = "description")]
        public string Description { get; set; }
        public long LocId { get; set; }
        public long DeptId { get; set; }
        [JsonProperty(PropertyName = "closingDate")]
        public System.DateTime ClosingDate { get; set; }
        [JsonProperty(PropertyName = "postedDate")]
        public System.DateTime PostDate { get; set; }
        [JsonProperty(PropertyName = "code")]
        public string JobCode { get; set; }
        [Newtonsoft.Json.JsonIgnore]
        public virtual Department Department { get; set; }
        [Newtonsoft.Json.JsonIgnore]
        public virtual Location Location { get; set; }
    }
}
