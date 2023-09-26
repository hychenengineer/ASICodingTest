using System.ComponentModel.DataAnnotations.Schema;
namespace Services.Entity
{
    public class Email
    {
        public long Id { get; set; }

        public bool IsPrimary { get; set; }

        public string Address { get; set; } = default!;

        [ForeignKey("Contact")]
        public long ContactId { get; set; }

        public Contact Contact { get; set; }
    }
}