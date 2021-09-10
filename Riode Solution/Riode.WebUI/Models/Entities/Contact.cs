using System;
using System.ComponentModel.DataAnnotations;

namespace Riode.WebUI.Models.Entities
{
    public class Contact : BaseEntity
    {
        [Required]
        public string Name { get; set; }

        [Required]
        [EmailAddress]
        public string Email { get; set; }

        [Required]
        public string Comment { get; set; }
        public string Answer { get; set; }
        public DateTime? AnswerDate { get; set; }
        public int? AnswerBy { get; set; }

        public bool Marked { get; set; }
    }
}
