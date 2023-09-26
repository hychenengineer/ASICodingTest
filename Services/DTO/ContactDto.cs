namespace Services.Dto
{
    public class ContactDto
    {
        public long Id { get; set; }

        public string Name { get; set; }

        public DateTime BirthDate { get; set; }

        public ICollection<EmailDto> Emails { get; set; }
    }
}