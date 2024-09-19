namespace QualiPro_Recruitment_Web_Api.DTOs
{
    public class JobApplicationDto
    {
        public int Id { get; set; }
        public int? CondidatId { get; set; }
        public int? JobId { get; set; }
        public DateTime? MeetingDate { get; set; }
        public int? HeadToHeadInterviewNote { get; set; }
        public int? Score { get; set; }

    }
}
