using System.ComponentModel.DataAnnotations;

namespace FlightDoc_Syste.Model
{
    public class SignUpModel
    {

        [Required, EmailAddress]
        public string Email { get; set; } = null!;
        [Required]
        public string Password { get; set; } = null!;
    }
}
