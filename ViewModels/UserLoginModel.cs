using System.ComponentModel.DataAnnotations;

namespace NotesApp.ViewModels
{
    public class UserLoginModel
    {
        [StringLength(40)]
        [RegularExpression(@"^([a-zA-Z0-9_\-\.]+)@((\[[0-9]{1,3}\.[0-9]{1,3}\.[0-9]{1,3}\.)|(([a-zA-Z0-9\-]+\.)+))([a-zA-Z]{2,4}|[0-9]{1,3})(\]?)$")]
        [Required(ErrorMessage = "Email should be entered.")]
        [EmailAddress(ErrorMessage = "Email is invalid.")]
        public string Email { get; set; }

        [StringLength(20)]
        [DataType(DataType.Password)]
        [Required(ErrorMessage = "Password should be entered.")]
        public string Password { get; set; }

        [Display(Name = "Remember Me")]
        public bool RememberMe { get; internal set; }
    }
}
