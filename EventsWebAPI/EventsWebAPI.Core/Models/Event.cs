using EventsWebAPI.Core.Enums;
using System;
using System.Collections.Generic;
using System.ComponentModel.DataAnnotations;

namespace EventsWebAPI.Core.Models
{
    public class Event
    {
        [Key]
        public Guid Id { get; set; }
        [Required]
        public string Name { get; set; }
        [Required]
        public string Description { get; set; }
        public DateTime EventDate { get; set; }
        [Required]
        public string EventPlace { get; set; }
        [Required]
        public EventCategory Category { get; set; }
        public int MaxAmountOfMembers { get; set; }
        public string ImageURL { get; set; } = null;
        public ICollection<AttendingInfo> Members { get; set; }
    }
}
