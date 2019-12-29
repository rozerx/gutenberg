using System;
using System.Collections.Generic;
using System.ComponentModel;
using System.ComponentModel.DataAnnotations;
using System.Linq;
using System.Web;
using Xunit;

namespace GutenbergProjectVBS.Web.Models.ViewModels
{
    public class LoginViewModel
    {
        [DisplayName("Email Address")]
        [DataType(DataType.EmailAddress)]
        [Required(ErrorMessage = "{0} field is required.")]
        public string Email { get; set; }

        [DisplayName("Password")]
        [DataType(DataType.Password)]
        [Required(ErrorMessage = "{0} field is required.")]
        public string Password { get; set; }
    }
}