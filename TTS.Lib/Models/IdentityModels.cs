using Microsoft.AspNetCore.Identity;
using Microsoft.AspNetCore.Identity.EntityFrameworkCore;
using System;
using System.Collections.Generic;
using System.ComponentModel.DataAnnotations;
using System.Security.Claims;
using System.Text;
using System.Threading.Tasks;

namespace TTS.Lib.Models
{
    public class TTSUser : IdentityUser
    {

        [Required]
        public DateTime JoinDate { get; set; }
        public bool IsActived { get; set; }
        public System.DateTime LastModified { get; set; }
        public string ModifiedBy { get; set; }
        public UserInfo Info { get; set; }
    }

    public class UserInfo
    {
        public string RegisterType { get; set; }
        public string Avatar { get; set; }
        public string NickName { get; set; }
        public string Gender { get; set; }
        public int CountryId { get; set; }
        public string Culture { get; set; }
        public DateTime? DOB { get; set; }
    }
}
