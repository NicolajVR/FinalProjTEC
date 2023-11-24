using System.ComponentModel.DataAnnotations;

namespace skolesystem.DTOs
{
    public class UserUpdateDto
    {
        [Required(ErrorMessage = "Surname is required")]
        [StringLength(255, ErrorMessage = "Max string length is 255")]
        [MinLength(1, ErrorMessage = "Min string length is 1")]
        public string surname { get; set; }

        [Required(ErrorMessage = "Email is required")]
        [EmailAddress(ErrorMessage = "Invalid email address")]
        public string email { get; set; }

        [Required(ErrorMessage = "Password hash is required")]
        public string password_hash { get; set; }

        public bool is_deleted { get; set; }

        [Required(ErrorMessage = "Role ID is required")]
        public int role_id { get; set; }


    }
}