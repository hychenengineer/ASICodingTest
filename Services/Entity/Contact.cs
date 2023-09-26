namespace Services.Entity
{
    public class Contact
    {
        public long Id { get; set; }

        public string Name { get; set; }

        public DateTime BirthDate { get; set; }

        public ICollection<Email> Emails { get; set; }
    }
}