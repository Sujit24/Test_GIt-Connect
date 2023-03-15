using System;

namespace NetTrackModel
{
    public class BaseModel
    {
        public int SessionId { get; set; }
        public int ClientId { get; set; }

        public int EmployeeId { get; set; }
        public int UserId { get; set; }
        
        public string EmployeeName { get; set; }
        public string UserName { get; set; }

        public int EntryBy { get; set; }
        public int ModifiedBy { get; set; }
        public DateTime EntryDate { get; set; }
        public DateTime ModifiedDate { get; set; }
    }
}
