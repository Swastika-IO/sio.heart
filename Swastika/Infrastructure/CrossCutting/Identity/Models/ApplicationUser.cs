using Microsoft.AspNetCore.Identity.EntityFrameworkCore;
using System;
using System.ComponentModel.DataAnnotations;

namespace Swastika.Infrastructure.CrossCutting.Identity.Models
{
    // Add profile data for application users by adding properties to the ApplicationUser class
    public class ApplicationUser : IdentityUser
    {
        [Required]
        public DateTime JoinDate { get; set; }
        public bool IsActived { get; set; }
        public System.DateTime LastModified { get; set; }
        public string ModifiedBy { get; set; }

        public string RegisterType { get; set; }
        public string Avatar { get; set; }
        public string NickName { get; set; }
        public string FirstName { get; set; }
        public string LastName { get; set; }
        public string Gender { get; set; }
        public int CountryId { get; set; }
        public string Culture { get; set; }
        public DateTime? DOB { get; set; }
    }
}
