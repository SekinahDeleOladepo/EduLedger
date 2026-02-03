using System.ComponentModel.DataAnnotations;

namespace EduLedger.Entitites.DTOs.AuthDTO
{
    public class RegisterUserDTO
    {
            public string Email { get; set; }
            public string Password { get; set; }
            public string Role { get; set; }

            // Profile fields
            public string FirstName { get; set; }
            public string LastName { get; set; }
            public DateTime DateOfBirth { get; set; }
            public string Gender { get; set; }
            public string ResidentialAddress { get; set; }

            // Optional
            public int? ClassLevelId { get; set; }
        }

    }

