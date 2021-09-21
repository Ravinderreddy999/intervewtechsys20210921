using System;

namespace Interview.Data.Models
{
    public class BaseModel
    {
        public int InstanceId { get; set; }
        public DateTime CreateDateTime { get; set; }
        public DateTime? UpdateDateTime { get; set; }
        public string UpdatedBy { get; set; }
    }
}