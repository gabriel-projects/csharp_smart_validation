using Api.GRRInnovations.SmartValidation.Domain.Attributes;
using System.ComponentModel.DataAnnotations;

namespace Api.GRRInnovations.SmartValidation.Domain.Models
{
    public class User
    {
        [Required]
        public string Name { get; set; }

        [MaxLength(50)]
        public string UserName { get; set; }

        [CustomRegex(@"^\S+@\S+\.\S+$", ErrorMessage = "Email inválido")]
        public string Email { get; set; }
    }
}
