namespace QualiPro_Recruitment_Web_Api.DTOs
{
    public class JobApplicationInfoDto
    {
        public int Id { get; set; }
        public int? CondidatId { get; set; }
        public int? JobId { get; set; }
        public DateTime? MeetingDate { get; set; }
        public double? HeadToHeadInterviewNote { get; set; }
        public double? Score { get; set; }
        public string CandidateFullName { get; set; }
    }
}
