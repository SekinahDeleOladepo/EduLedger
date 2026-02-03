namespace EduLedger.Data
{
    public class Base
    {
        public Base()
        {
            CreatedAt = DateTime.Now;
            IsActive = true;
        }
        public int Id { get; set; }
        public DateTime CreatedAt { get; set; }
        public DateTime? UpdatedAt { get; set; }
        public bool IsActive { get; set; }
    }
}
