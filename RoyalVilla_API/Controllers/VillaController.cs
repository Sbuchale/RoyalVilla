using AutoMapper;
using Microsoft.AspNetCore.Http;
using Microsoft.AspNetCore.Mvc;
using Microsoft.EntityFrameworkCore;
using RoyalVilla_API.Data;
using RoyalVilla_API.Models;
using RoyalVilla_API.Models.DTO;
using System.Collections;

namespace RoyalVilla_API.Controllers
{

    [Route("api/Villa")]
    [ApiController]
    public class VillaController : ControllerBase
    {

        private readonly ApplicationDbContext _db;
        private readonly IMapper _mapper;
        public VillaController(ApplicationDbContext db, IMapper mapper)
        {
            _db = db;
            _mapper = mapper;
        }

        [HttpGet]
        public async Task<ActionResult<IEnumerable<villaDTO>>> GetVillas()
        {
            var villas = await _db.Villa.ToListAsync();
            return Ok(_mapper.Map<List<villaDTO>>(villas));
        }

        [HttpGet("{id:int}")]
        public async Task<ActionResult<ApiResponse<villaDTO>>> GetVillaById(int id)
        {
            try
            {
                if (id <= 0)
                {
                    return new ApiResponse<villaDTO>()
                    {
                        StatusCode= 400,
                        Errors="villa ID must be greater than 0",
                        Success=false,
                        Message="Bad Request"
                    };
                }
                var villa = await _db.Villa.FirstOrDefaultAsync(u => u.Id == id);
                if (villa == null)
                {
                    return NotFound($"villa with ID {id} was not found");
                }

                return new ApiResponse<villaDTO>()
                {
                    StatusCode = 200,
                    Success = true,
                    Message = "Record Retrieved Successfully",
                    Data = _mapper.Map<villaDTO>(villa)
                };

            }
            catch (Exception ex)
            {
                return StatusCode(StatusCodes.Status500InternalServerError,
                    $" An error occurred while retrieving villa with ID{id};{ex.Message}");
            }
        }


        [HttpPost]
        public async Task<ActionResult<villaDTO>> CreateVilla(villaCreateDTO villaDTO)
        {
            try
            {
                if (villaDTO == null)
                {
                    return BadRequest("villa data is required");
                }

                Villa villa = _mapper.Map<Villa>(villaDTO);

                _db.Add(villa);
                await _db.SaveChangesAsync();
                return CreatedAtAction(nameof(CreateVilla), new { id = villa.Id }, _mapper.Map<villaDTO>(villa));
            }
            catch (Exception ex)
            {
                return StatusCode(StatusCodes.Status500InternalServerError,
                    $" An error occurred while Creating the villa;{ex.Message}");
            }
        }

        [HttpPut("{id:int}")]
        public async Task<ActionResult<villaUpdateDTO>> UpdateVilla(int id, villaUpdateDTO villaDTO)
        {
            try
            {
                if (villaDTO == null)
                {
                    return BadRequest("villa data is required");
                }
                if (id != villaDTO.Id)
                {
                    return BadRequest("Villa Id in URL does not match villa ID in request body");
                }

                var existingVilla = await _db.Villa.FirstOrDefaultAsync(u => u.Id == id);
                if (existingVilla == null)
                {
                    return NotFound($"villa with ID {id} was not found");
                }

                var duplicateVilla = await _db.Villa.FirstOrDefaultAsync(u => u.Name.ToLower() == villaDTO.Name.ToLower() && u.Id != id);
                if (duplicateVilla != null)
                {
                    return Conflict($"A villa with the name '{villaDTO.Name}' already exists");
                }

                _mapper.Map(villaDTO, existingVilla);
                existingVilla.UpdatedDate = DateTime.Now;

                await _db.SaveChangesAsync();

                return Ok(villaDTO);

            }
            catch (Exception ex)
            {
                return StatusCode(StatusCodes.Status500InternalServerError,
                    $" An error occurred while Updating the villa;{ex.Message}");
            }
        }

        [HttpDelete("{id:int}")]
        public async Task<ActionResult> DeleteVilla(int id)
        {
            try
            {
                var existingVilla = await _db.Villa.FirstOrDefaultAsync(u => u.Id == id);
                if (existingVilla == null)
                {
                    return NotFound($"villa with ID {id} was not found");
                }

                _db.Villa.Remove(existingVilla);
                await _db.SaveChangesAsync();

                return NoContent();

            }
            catch (Exception ex)
            {
                return StatusCode(StatusCodes.Status500InternalServerError,
                    $" An error occurred while deleting the villa;{ex.Message}");
            }
        }
    }
}
