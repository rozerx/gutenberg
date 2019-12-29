using System;
using System.Collections.Generic;
using System.ComponentModel;
using System.ComponentModel.DataAnnotations;
using System.Linq;
using System.Web;
using Xunit;

namespace GutenbergProjectVBS.Web.Models.ViewModels
{
    public class ProfileViewModel
    {
        [DisplayName("Name")]
        [Required(ErrorMessage = "{0} field is required.")]
        [StringLength(50, ErrorMessage = "{0} must be max. {1}. character.")]
        public string Name { get; set; }

        [DisplayName("Surname")]
        [Required(ErrorMessage = "{0} field is required.")]
        [StringLength(50, ErrorMessage = "{0} must be max. {1}. character.")]
        public string Surname { get; set; }

        [DisplayName("Email Address")]
        [DataType(DataType.EmailAddress)]
        [Required(ErrorMessage = "{0} field is required.")]
        public string Email { get; set; }
    }
}