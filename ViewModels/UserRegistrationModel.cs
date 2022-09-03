using System.ComponentModel.DataAnnotations;

namespace NotesApp.ViewModels
{
    public class UserRegistrationModel
    {
        [StringLength(80)]
        [Required(ErrorMessage = "UserName should be entered.")]
        public string UserName { get; set; }

        [StringLength(40)]
        [RegularExpression(@"^([a-zA-Z0-9_\-\.]+)@((\[[0-9]{1,3}\.[0-9]{1,3}\.[0-9]{1,3}\.)|(([a-zA-Z0-9\-]+\.)+))([a-zA-Z]{2,4}|[0-9]{1,3})(\]?)$")]
        [Required(ErrorMessage = "Email should be entered.")]
        public string Email { get; set; }

        [StringLength(20)]
        [DataType(DataType.Password)]
        [Required(ErrorMessage = "Password should be entered.")]
        public string Password { get; set; }

        [StringLength(20)]
        [DataType(DataType.Password)]
        [Required(ErrorMessage = "Confirm Password should be entered.")]
        [Compare("Password", ErrorMessage = "Passwords should be same.")]
        public string ConfirmPassword { get; set; }
    }
}
