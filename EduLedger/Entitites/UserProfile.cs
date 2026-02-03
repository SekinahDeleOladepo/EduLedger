namespace EduLedger.Data
{
    public class UserProfile : Base
    {
        public string FirstName { get; set; }
        public string LastName { get; set; }
        public string Email { get; set; }
        public string ResidentialAddress { get; set; }
        public DateTime DateOfBirth { get; set; }
        public string Gender { get; set; }
        public string UserId { get; set; }
        public ApplicationUser ApplicationUser { get; set; }
    }
}
