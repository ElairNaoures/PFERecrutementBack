namespace QualiPro_Recruitment_Web_Api.DTOs
{
    public class JobApplicationInfoDto
    {
        public string JobTitle { get; set; }
        public int CandidateCount { get; set; }
        public string BestCandidateName { get; set; }
        public int HighestScore { get; set; }
    }
}
