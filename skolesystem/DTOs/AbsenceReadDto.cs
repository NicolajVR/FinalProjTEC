namespace skolesystem.DTOs
{
    public class AbsenceReadDto
    {
        public int absence_id { get; set; }
        public int user_id { get; set; }
        public int teacher_id { get; set; }
        public int class_id { get; set; }
        public string absence_date { get; set; }
        public string reason { get; set; }
        public bool is_deleted { get; set; }
    }
}
