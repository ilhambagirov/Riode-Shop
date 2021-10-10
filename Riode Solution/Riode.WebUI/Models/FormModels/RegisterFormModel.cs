using System.ComponentModel.DataAnnotations;

namespace Riode.WebUI.Models.FormModels
{
    public class RegisterFormModel
    {
        [Required]
        public string UserName { get; set; }
        [Required]
        public string Email { get; set; }
        [Required]
        public string Password { get; set; }
        public string PasswordCheck { get; set; }
       
    }
}
