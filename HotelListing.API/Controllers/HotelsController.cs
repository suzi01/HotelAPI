using System;
using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;
using AutoMapper;
using HotelListing.API.Contracts;
using Microsoft.AspNetCore.Http;
using Microsoft.AspNetCore.Mvc;
using Microsoft.EntityFrameworkCore;
using HotelListing.API.Data;
using HotelListing.API.Models.Hotel;

namespace HotelListing.API.Controllers
{
    [Route("api/[controller]")]
    [ApiController]
    public class HotelsController : ControllerBase
    {

        private readonly IHotelsRepository _hotelsRepository;
        private readonly IMapper _mapper;

        public HotelsController( IHotelsRepository hotelsRepository, IMapper iMapper)
        {
            this._hotelsRepository = hotelsRepository;
            this._mapper = iMapper;
        }

        // GET: api/Hotels
        [HttpGet]
        public async Task<ActionResult<IEnumerable<HotelDto>>> GetHotels()
        {
          // if (_context.Hotels == null)
          // {
          //     return NotFound();
          // }
          
          var hotels = await _hotelsRepository.GetAllAsync();
          return Ok(_mapper.Map<List<HotelDto>>(hotels));
        }

        // GET: api/Hotels/5
        [HttpGet("{id}")]
        public async Task<ActionResult<HotelDto>> GetHotel(int id)
        {
          // if (_context.Hotels == null)
          // {
          //     return NotFound();
          // }
            var hotel = await _hotelsRepository.GetAsync(id);

            if (hotel == null)
            {
                return NotFound();
            }

            return Ok(_mapper.Map<HotelDto>(hotel));
        }

        // PUT: api/Hotels/5
        // To protect from overposting attacks, see https://go.microsoft.com/fwlink/?linkid=2123754
        [HttpPut("{id}")]
        public async Task<IActionResult> PutHotel(int id, HotelDto hotelDto)
        {
            if (id != hotelDto.Id)
            {
                return BadRequest();
            }

            var hotel = await _hotelsRepository.GetAsync(id);
            if (hotel == null)
            {
                return NotFound();
            }

            _mapper.Map(hotelDto, hotel);
            
            try
            {
                await _hotelsRepository.UpdateAsync(hotel);;
            }
            catch (DbUpdateConcurrencyException)
            {
                if (!await HotelExists(id))
                {
                    return NotFound();
                }
                else
                {
                    throw;
                }
            }

            return NoContent();
        }

        // POST: api/Hotels
        // To protect from overposting attacks, see https://go.microsoft.com/fwlink/?linkid=2123754
        [HttpPost]
        public async Task<ActionResult<CreateHotelDto>> PostHotel(CreateHotelDto createHotelDto)
        {
          // if (_context.Hotels == null)
          // {
          //     return Problem("Entity set 'HotelListingDbContext.Hotels'  is null.");
          // }
          var hotel = _mapper.Map<Hotel>(createHotelDto);
          await _hotelsRepository.AddAsync(hotel);

          return CreatedAtAction("GetHotel", new { id = hotel.Id }, hotel);
        }

        // DELETE: api/Hotels/5
        [HttpDelete("{id}")]
        public async Task<IActionResult> DeleteHotel(int id)
        {
            // if (_context.Hotels == null)
            // {
            //     return NotFound();
            // }
            var hotel = await _hotelsRepository.GetAsync(id);
            if (hotel == null)
            {
                return NotFound();
            }

            await _hotelsRepository.Deletesync(id);

            return NoContent();
        }

        private async Task<bool> HotelExists(int id)
        {
            return  await _hotelsRepository.Exists(id);
        }
    }
}
