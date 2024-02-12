using AuthSystem.Areas.Identity.Data;
using System.ComponentModel.DataAnnotations;

namespace GSP.Models.ViewModels
{
    public class ChangePasswordViewModel
    {
        
        public string Id { get; set; }

        [Display(Name = "First Name")]
        public string FirstName { get; set; }

        [Display(Name = "Last Name")]
        public string LastName { get; set; }
        public string Email { get; set; }

        [Display(Name = "New Password")]
        public string NewPassword { get; set; }

        [Display(Name = "Confirm Password")]
        public string ConfirmPassword { get; set; }
    }
}
