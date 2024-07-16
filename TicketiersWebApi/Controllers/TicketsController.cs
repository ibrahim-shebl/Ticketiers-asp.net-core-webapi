using AutoMapper;
using Microsoft.AspNetCore.Http;
using Microsoft.AspNetCore.Mvc;
using ticketier_webapi.Core.DTO;
using TicketiersWebApi.Core.Context;
using TicketiersWebApi.Core.Entities;
using Microsoft.EntityFrameworkCore;

namespace TicketiersWebApi.Controllers
{
    [Route("TicketierAPI/[controller]")]
    [ApiController]
    public class TicketsController : ControllerBase
    {
        // We need Database so we inject it using constructor
        // We need AutoMapper so we inject it using constructor 
        private readonly ApplicationDbContext _context;
        private readonly IMapper _mapper;

        public TicketsController(ApplicationDbContext context, IMapper mapper)
        {
            _context = context;
            _mapper = mapper;
        }

        // CRUD

        // Create
        [HttpPost]
        [Route("create")]
        public async Task<IActionResult> CreateTicket([FromBody] CreateTicketDto createTicketDto)
        {
            var newTicket = new Ticket();

            _mapper.Map(createTicketDto, newTicket);

            await _context.Tickets.AddAsync(newTicket);
            await _context.SaveChangesAsync();

            return Ok("Ticket saved successfully");
        }

        // Read all
        [HttpGet]
        public async Task<ActionResult<IEnumerable<GetTicketDto>>> GetTickets()
        {
            // Get Tickets from Context

             

            var tickets = await _context.Tickets.ToListAsync();

            var convertedTickets = _mapper.Map<IEnumerable<GetTicketDto>>(tickets);

            return Ok(convertedTickets);
        }

        // Read one by Id
        [HttpGet]
        [Route("{id}")]
        public async Task<ActionResult<GetTicketDto>> GetTicketById([FromRoute] long id)
        {
            // Get ticket from context and check if it exists
            var ticket = await _context.Tickets.FirstOrDefaultAsync(t => t.Id == id);
            if (ticket is null)
            {
                return NotFound("Ticket Not found");
            }

            var convertedTicket = _mapper.Map<GetTicketDto>(ticket);

            return Ok(convertedTicket);
        }

        // Update
        [HttpPut]
        [Route("edit/{id}")]
        public async Task<IActionResult> EditTicket([FromRoute] long id, [FromBody] UpdateTicketDto updateTicketDto)
        {
            // Get ticket from context and check if it exists
            var ticket = await _context.Tickets.FirstOrDefaultAsync(t => t.Id == id);
            if (ticket is null)
            {
                return NotFound("Ticket Not found");
            }

            _mapper.Map(updateTicketDto, ticket);
            ticket.UpdatedAt = DateTime.Now;
            await _context.SaveChangesAsync();

            return Ok("ticket updated successfully");
        }

        // Delete
        [HttpDelete]
        [Route("delete/{id}")]
        public async Task<IActionResult> DeleteTask([FromRoute] long id)
        {
            // Get ticket from context and check if it exists
            var ticket = await _context.Tickets.FirstOrDefaultAsync(t => t.Id == id);
            if (ticket is null)
            {
                return NotFound("Ticket Not found");
            }

            _context.Tickets.Remove(ticket);
            await _context.SaveChangesAsync();

            return Ok("Ticket Deleted successfully");
        }
    }
}