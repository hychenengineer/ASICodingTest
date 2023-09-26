namespace Services.Dto
{
    public class EmailDto
    {
        public long Id { get; set; }

        public bool IsPrimary { get; set; }

        public string Address { get; set; } = default!;
    }
}