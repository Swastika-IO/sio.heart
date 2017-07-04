using Microsoft.AspNetCore.Mvc.Rendering;
using System;
using System.Collections.Generic;
using System.ComponentModel.DataAnnotations;
using System.Text;

namespace TTS.Identity.Models.AccountViewModels
{
    public class UserViewModel
    {
        public string Id { get; set; }
        public string UserName { get; set; }
        [DataType(DataType.Password)]
        public string Password { get; set; }
        [Display(Name = "Confirm Password")]
        [DataType(DataType.Password)]
        public string ConfirmPassword { get; set; }
        public string Name { get; set; }
        public string Email { get; set; }
        [Display(Name = "User Claims")]
        public List<SelectListItem> UserClaims { get; set; }
    }
}
