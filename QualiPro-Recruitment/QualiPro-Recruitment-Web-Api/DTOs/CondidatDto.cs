namespace QualiPro_Recruitment_Web_Api.DTOs
{
    public class CondidatDto
    {
        public int Id { get; set; }

        public string? Summary { get; set; }

        public string? FirstName { get; set; }

        public string? LastName { get; set; }

        public string? Country { get; set; }

        public string? PhoneNumber { get; set; }

        public DateTime? Birthdate { get; set; }

        //public int ContractTypeId { get; set; }
        //public int UserId { get; set; }
    }
}
