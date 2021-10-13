using Resources;
using System;
using System.ComponentModel.DataAnnotations;

namespace Riode.Domain.Models.Entities
{
    public class Contact : BaseEntity
    {
        [Display(ResourceType = typeof(ContactResource),Name = "Name")]
        [Required(ErrorMessageResourceType = typeof(ContactResource), ErrorMessageResourceName = "Cantbenull")]
        public string Name { get; set; }

        [Display(ResourceType = typeof(ContactResource), Name = "Email")]
        [Required(ErrorMessageResourceType = typeof(ContactResource), ErrorMessageResourceName = "Cantbenull")]
        [EmailAddress(ErrorMessageResourceType = typeof(ContactResource), ErrorMessageResourceName = "WrongEmailFormat")]
        public string Email { get; set; }

        [Display(ResourceType = typeof(ContactResource), Name = "Comment")]
        [Required(ErrorMessageResourceType = typeof(ContactResource), ErrorMessageResourceName = "Cantbenull")]
        public string Comment { get; set; }
        public string Answer { get; set; }
        public DateTime? AnswerDate { get; set; }
        public int? AnswerBy { get; set; }

        public bool Marked { get; set; }
    }
}
