using Microsoft.EntityFrameworkCore;
using Microsoft.Extensions.Logging;
using Service.Interface;
using Services.Dto;
using Services.Entity;

namespace Service
{
    public class ContactService : IContactService
    {
        private readonly ILogger<ContactService> _logger;
        private ApplicationDbContext _dbContext;
        public ContactService(ApplicationDbContext dbContext, ILogger<ContactService> logger)
        {
            _logger = logger;
            _dbContext = dbContext;
        }

        public async Task<ContactDto>? GetContactAsyncById(long id)
        {
            var contact = await _dbContext.Contacts
                .Include(c => c.Emails)
                .SingleOrDefaultAsync(c => c.Id == id)
                .ConfigureAwait(false);

            if (contact == null)
            {
                return null;
            }

            var emailDtos = new List<EmailDto>();
            foreach (var email in contact.Emails)
            {
                var emailDto = new EmailDto
                {
                    Id = email.Id,
                    IsPrimary = email.IsPrimary,
                    Address = email.Address
                };
                emailDtos.Add(emailDto);
            }

            var contactDto = new ContactDto
            {
                Id = contact.Id,
                Name = contact.Name,
                BirthDate = contact.BirthDate,
                Emails = emailDtos,
            };

            return contactDto;
        }

        public async Task<ContactDto> CreateContactAsync(ContactDto dto)
        {
            if (IsPrimaryHappensMoreThanOnce(dto.Emails))
            {
                throw new ArgumentException("There are more than 1 isPrimary set to true.");
            }

            var emails = AddEmails(dto.Emails);

            var contact = new Contact
            {
                Name = dto.Name,
                BirthDate = dto.BirthDate,
                Emails = emails
            };

            _dbContext.Contacts.Add(contact);
            await _dbContext.SaveChangesAsync();
            dto.Id = contact.Id;
            return dto;
        }

        public async Task<ContactDto>? UpdateContactAsyncById(long id, ContactDto dto)
        {
            if (IsPrimaryHappensMoreThanOnce(dto.Emails))
            {
                throw new ArgumentException("There are more than 1 isPrimary set to true.");
            }

            var contact = await _dbContext.Contacts
                .Include(c => c.Emails)
                .SingleOrDefaultAsync(c => c.Id == id)
                .ConfigureAwait(false);

            if (contact == null)
            {
                return null;
            }
            
            foreach (var email in contact.Emails)
            {
                _dbContext.Emails.Remove(email);
            }

            var emails = AddEmails(dto.Emails);

            contact.Name = dto.Name;
            contact.BirthDate = dto.BirthDate;
            contact.Emails = emails;

            await _dbContext.SaveChangesAsync();
            dto.Id = contact.Id;
            return dto;
        }

        public async Task DeleteContactAsyncById(long id)
        {
            var contact = await _dbContext.Contacts
                .Include(c => c.Emails)
                .SingleOrDefaultAsync(c => c.Id == id)
                .ConfigureAwait(false);

            if (contact != null)
            {
                foreach (var email in contact.Emails)
                {
                    _dbContext.Emails.Remove(email);
                }

                _dbContext.Contacts.Remove(contact);
            }

            await _dbContext.SaveChangesAsync();
        }

        public async Task<IEnumerable<ContactDto>>? SearchContactAsync(DateTime? startDate, DateTime? endDate, string? name)
        {
            IQueryable<Contact> query = _dbContext.Contacts;

            if (startDate.HasValue)
            {
                query = query.Where(contact => contact.BirthDate >= startDate.Value.Date);
            }

            if (endDate.HasValue)
            {
                query = query.Where(contact => contact.BirthDate <= endDate.Value.Date);
            }

            if (!string.IsNullOrEmpty(name))
            {
                query = query.Where(contact => contact.Name.Contains(name, StringComparison.OrdinalIgnoreCase));
            }

            var searchResults = await query
                .Select(contact => new ContactDto
                {
                    Id = contact.Id,
                    Name = contact.Name,
                    BirthDate = contact.BirthDate,
                    Emails = contact.Emails.Select(email => new EmailDto
                    {
                        Id = email.Id,
                        IsPrimary = email.IsPrimary,
                        Address = email.Address
                    }).ToList()
                })
                .ToListAsync()
                .ConfigureAwait(false);

            return searchResults;
        }

        private IList<Email> AddEmails(IEnumerable<EmailDto> emailDtos)
        {
            var emails = new List<Email>();
            foreach (var emailDto in emailDtos)
            {
                var email = new Email { Address = emailDto.Address, IsPrimary = emailDto.IsPrimary };
                emails.Add(email);
                _dbContext.Emails.Add(email);
                emailDto.Id = email.Id;
            }
            return emails;
        }

        private bool IsPrimaryHappensMoreThanOnce(IEnumerable<EmailDto> emailDtos)
        {
            var isPrimaryCount = 0;
            foreach (var emailDto in emailDtos)
            {
                if (emailDto.IsPrimary)
                {
                    isPrimaryCount++;
                    if (isPrimaryCount > 1)
                    {
                        return true;
                    }
                }
            }

            return false;
        }
    }
}