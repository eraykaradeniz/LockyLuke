namespace SharedLibrary.Dtos.Information
{
    public class InformationDetailDto
    {
        public Guid Id { get; set; }
        public string Site { get; set; }
        public string UserName { get; set; }
        public string UserId { get; set; }
        public string Password { get; set; }
        public DateTime LastUpdateDate { get; set; }
    }
}
