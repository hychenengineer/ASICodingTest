using Microsoft.AspNetCore.Mvc;
using Service.Interface;
using Services.Dto;

namespace ASICodingTest.Controllers
{
    [ApiController]
    [Route("api/contacts/")]
    public class ContactController : ControllerBase
    {
        private readonly ILogger<ContactController> _logger;
        private IContactService _contactService;
        public ContactController(IContactService contactService, ILogger<ContactController> logger)
        {
            _logger = logger;
            _contactService = contactService;
        }

        [HttpPost]
        public async Task<ActionResult<ContactDto>> Create([FromBody] ContactDto dto)
        {
            var newDto = await _contactService.CreateContactAsync(dto).ConfigureAwait(false);
            return newDto;
        }

        [HttpGet("{id}")]
        public async Task<ActionResult<ContactDto>> Read(long id)
        {
            var dto = await _contactService.GetContactAsyncById(id);
            if (dto == null)
            {
                return NotFound();
            }

            return Ok(dto);
        }

        [HttpPut("{id}")]
        public async Task<ActionResult<ContactDto>> Update(long id, [FromBody] ContactDto dto)
        {
            var newDto = await _contactService.UpdateContactAsyncById(id, dto).ConfigureAwait(false);
            if (newDto == null)
            {
                return NotFound();
            }

            return Ok(newDto);
        }

        [HttpDelete("{id}")]
        public async Task<ActionResult<ContactDto>> Delete(long id, [FromBody] ContactDto dto)
        {
            await _contactService.DeleteContactAsyncById(id).ConfigureAwait(false);
            return Ok();
        }

        [HttpGet("search")]
        public async Task<ActionResult<IEnumerable<ContactDto>>> SearchContacts(
            [FromQuery] DateTime? startDate = null,
            [FromQuery] DateTime? endDate = null,
            [FromQuery] string? name = null)
        {
            var searchResults = await _contactService.SearchContactAsync(startDate, endDate, name);

            if (searchResults == null)
            {
                return NotFound("No matching contacts found.");
            }

            return Ok(searchResults);
        }
    }
}