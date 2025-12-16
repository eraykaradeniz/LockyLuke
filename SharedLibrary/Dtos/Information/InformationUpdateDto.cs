namespace SharedLibrary.Dtos.Information
{
    public class InformationUpdateDto
    {

        public Guid id { get; set; }
        public string Site { get; set; }
        public string UserName { get; set; }
        public string Password { get; set; }
        public string? Description{ get; set; }
    }
}
