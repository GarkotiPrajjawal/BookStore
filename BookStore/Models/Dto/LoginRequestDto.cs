using System.ComponentModel.DataAnnotations;

namespace BookStore.Models.Dto
{
    public class LoginRequestDto
    {
        [Required]
        public String Name { get; set; }
        [Required]
        public String Password { get; set; }
    }
}
