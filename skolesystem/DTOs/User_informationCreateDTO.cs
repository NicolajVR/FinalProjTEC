using System.ComponentModel.DataAnnotations;

namespace skolesystem.DTOs
{
    public class User_informationCreateDto
    {
        [Required(ErrorMessage = "User information ID is required")]
        public int user_information_id { get; set; }

        [StringLength(40, ErrorMessage = "Max string length is 40")]
        public string name { get; set; }

        [StringLength(60, ErrorMessage = "Max string length is 60")]
        public string last_name { get; set; }

        [StringLength(20, ErrorMessage = "Max string length is 20")]
        public string phone { get; set; }

        [StringLength(25, ErrorMessage = "Max string length is 25")]
        public string date_of_birth { get; set; }

        [StringLength(90, ErrorMessage = "Max string length is 90")]
        public string address { get; set; }

        public bool is_deleted { get; set; }

        [Required(ErrorMessage = "Gender ID is required")]
        public int gender_id { get; set; }

        [Required(ErrorMessage = "User ID is required")]
        public int user_id { get; set; }


    }

}