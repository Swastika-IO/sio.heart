using Microsoft.AspNetCore.Mvc.Rendering;
using System.Collections.Generic;
using System.ComponentModel.DataAnnotations;

namespace Swastika.Infrastructure.CrossCutting.Identity.Models.AccountViewModels
{
    public class RegisterViewModel
    {
        public string FirstName { get; set; }
        public string LastName { get; set; }
        public string NickName { get; set; }
        public List<SelectListItem> UserClaims { get; set; }

        [Required]
        public string UserName { get; set; }
        /// <summary>
        /// Gets or sets the email.
        /// </summary>
        /// <value>
        /// The email.
        /// </value>
        [Required]
        [EmailAddress]
        [Display(Name = "Email")]
        public string Email { get; set; }

        /// <summary>
        /// Gets or sets the password.
        /// </summary>
        /// <value>
        /// The password.
        /// </value>
        [Required]
        [StringLength(100, ErrorMessage = "The {0} must be at least {2} and at max {1} characters long.", MinimumLength = 6)]
        [DataType(DataType.Password)]
        [Display(Name = "Password")]
        public string Password { get; set; }

        /// <summary>
        /// Gets or sets the confirm password.
        /// </summary>
        /// <value>
        /// The confirm password.
        /// </value>
        [DataType(DataType.Password)]
        [Display(Name = "Confirm password")]
        [Compare("Password", ErrorMessage = "The password and confirmation password do not match.")]
        public string ConfirmPassword { get; set; }
    }
}
