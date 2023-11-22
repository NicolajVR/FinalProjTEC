namespace skolesystem.DTOs
{
    public class ScheduleReadDto
    {
        public int schedule_id { get; set; }

        public int subject_id { get; set; }

        public string day_of_week { get; set; }

        public string subject_name { get; set; }

        public string start_time { get; set; }

        public string end_time { get; set; }

        public int class_id { get; set; }

        //public string Class { get; set; }

        //public string UserSubject { get; set; }
    }
}
