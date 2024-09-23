using System;
using System.ComponentModel.DataAnnotations;

namespace EventsWebAPI.Core.Models
{
    public class AttendingInfo
    {
        [Key]
        public Guid Id { get; set; }
        public Guid? EventID { get; set; }
        public Event Event { get; set; }
        public Guid? UserID { get; set; }
        public User User { get; set; }
        public DateTime? RegistrationDate { get; set; } = null;
        public AttendingInfo()
        {
            RegistrationDate = DateTime.Now;
        }
    }
}
