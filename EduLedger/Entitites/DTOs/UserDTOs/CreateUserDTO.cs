using System.ComponentModel.DataAnnotations;

namespace EduLedger.Data.DTOs.UserDTOs
{
    public class CreateUserDTO
    {
        [Required(ErrorMessage = "first name is required")]
        public string FirstName { get; set; }
        [Required(ErrorMessage = "last name is required")]
        public string LastName { get; set; }
        [Required(ErrorMessage = "user name is required")]
        public string UserName { get; set; }
        [Required(ErrorMessage = "email is required")]
        [EmailAddress(ErrorMessage = "invalid email address")]
        public string Email { get; set; }
        [Required(ErrorMessage = "residential address is required")]
        public string ResidentialAddress { get; set; }
        [Required(ErrorMessage = "Date of birth is required")]
        public DateTime DateOfBirth { get; set; }
        [Required(ErrorMessage = "Gender is required")]
        [AllowedValues("M", "F", ErrorMessage = "use M for male and F for Female")]
        public string Gender { get; set; }
        [Required]
        [RegularExpression(@"^(?=.*[a-z])(?=.*[A-Z])(?=.*\d)(?=.*[^\w\s]).{8,64}$", ErrorMessage = "invalid password")]
        public string Password { get; set; }
        [Required]
        [RegularExpression(@"^(?=.*[a-z])(?=.*[A-Z])(?=.*\d)(?=.*[^\w\s]).{8,64}$", ErrorMessage = "invalid password")]
        public string ConfirmPassword { get; set; }
        [Required]
        public string ClassLevel { get; set; }
    }
}
