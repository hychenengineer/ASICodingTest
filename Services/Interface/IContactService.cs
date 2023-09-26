using Services.Dto;

namespace Service.Interface
{
    public interface IContactService
    {
        public Task<ContactDto>? GetContactAsyncById(long id);

        public Task<ContactDto> CreateContactAsync(ContactDto dto);

        public Task<ContactDto>? UpdateContactAsyncById(long id, ContactDto dto);

        public Task DeleteContactAsyncById(long id);

        public Task<IEnumerable<ContactDto>>? SearchContactAsync(DateTime? startDate, DateTime? endDate, string? name);
    }
}