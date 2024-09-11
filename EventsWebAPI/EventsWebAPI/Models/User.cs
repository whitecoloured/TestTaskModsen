using EventsWebAPI.Enums;
using System;
using System.Collections.Generic;
using System.ComponentModel.DataAnnotations;


namespace EventsWebAPI.Models
{
    public class User
    {
        [Key]
        public Guid Id { get; set; }
        [Required]
        public string Name { get; set; }
        [Required]
        public string Surname { get; set; }
        public DateTime? BirthDate { get; set; }
        [Required]
        public string Email { get; set; }
        public UserRole Role { get; set; }
        public ICollection<AttendingInfo> Events { get; set; }
        public User()
        {
            Role = UserRole.User;
        }
    }
}
