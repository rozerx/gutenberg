using System.ComponentModel;
using System.ComponentModel.DataAnnotations;
using System.ComponentModel.DataAnnotations.Schema;

namespace GutenbergProjectVBS.Web.Models
{
    public class User : MyEntityBase
    {
        [DatabaseGenerated(DatabaseGeneratedOption.Identity)]
        public int Id { get; set; }

        [Required(ErrorMessage = "{0} field is required.")]
        [StringLength(250)]
        public string Name { get; set; }

        [Required(ErrorMessage = "{0} field is required.")]
        [StringLength(250)]
        public string Surname { get; set; }

        [Required(ErrorMessage = "{0} field is required.")]
        [StringLength(250)]
        [DisplayName("E-mail address")]
        [DataType(DataType.EmailAddress, ErrorMessage = "Please, enter the valid e-mail.")]
        public string Email { get; set; }

        [Required(ErrorMessage = "{0} field is required.")]
        [StringLength(250, ErrorMessage = "{0} field must be max. {1} character.")]
        [DisplayName("Password")]
        public string Password { get; set; }

        [DisplayName("User Type")]
        public bool IsAdmin { get; set; }

        [StringLength(150)]
        [DisplayName("Reset Password Code")]
        public string ResetPasswordCode { get; set; }
    }
}