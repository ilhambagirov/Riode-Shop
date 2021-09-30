using System.ComponentModel.DataAnnotations;

namespace Riode.WebUI.Models.FormModels
{
    public class LoginFormModel
    {
        [Required]
        public string UserName { get; set; }
        [Required]
        public string Password { get; set; }
    }
}
