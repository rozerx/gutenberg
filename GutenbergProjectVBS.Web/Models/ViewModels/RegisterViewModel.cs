using System;
using System.Collections.Generic;
using System.ComponentModel;
using System.ComponentModel.DataAnnotations;
using System.Linq;
using System.Web;

namespace GutenbergProjectVBS.Web.Models.ViewModels
{
    public class RegisterViewModel
    {
        [DisplayName("Name")]
        [Required(ErrorMessage = "{0} field is required.")]
        [StringLength(50, ErrorMessage = "{0} must be max. {1}. character.")]
        public string Name { get; set; }

        [DisplayName("Surname")]
        [Required(ErrorMessage = "{0} field is required.")]
        [StringLength(50, ErrorMessage = "{0} must be max. {1}. character.")]
        public string Surname { get; set; }

        [DisplayName("E-mail Address")]
        [Required(ErrorMessage = "{0} field is required.")]
        [StringLength(100, ErrorMessage = "{0} must be max. {1}. character.")]
        [EmailAddress(ErrorMessage = "{0} field is required email address.")]
        [DataType(DataType.EmailAddress)]
        public string Email { get; set; }

        [DisplayName("Password")]
        [DataType(DataType.Password)]
        [Required(ErrorMessage = "{0} field is required.")]
        [StringLength(25, ErrorMessage = "{0} must be max. {1}. character.")]
        public string Password { get; set; }

        [DisplayName("Password Repeat")]
        [DataType(DataType.Password)]
        [Required(ErrorMessage = "{0} field is required.")]
        [StringLength(25, ErrorMessage = "{0} must be max. {1}. character.")]
        [Compare("Password", ErrorMessage = "{0} does not match with {1}.")]
        public string PasswordConfirm { get; set; }
    }
}