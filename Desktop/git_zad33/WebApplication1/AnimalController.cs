using Microsoft.AspNetCore.Mvc;
using Microsoft.EntityFrameworkCore;
using YourNamespace.Data;
using YourNamespace.Models;
using System.Linq;
using System.Threading.Tasks;

namespace YourNamespace.Controllers
{
    [ApiController]
    [Route("api/[controller]")]
    public class AnimalsController : ControllerBase
    {
        private readonly ApplicationDbContext _context;

        public AnimalsController(ApplicationDbContext context)
        {
            _context = context;
        }

        // GET: api/Animals
        [HttpGet]
        public async Task<IActionResult> GetAnimals([FromQuery] string orderBy = "name")
        {
            var animals = from a in _context.Animals
                          select a;

            switch (orderBy.ToLower())
            {
                case "name":
                    animals = animals.OrderBy(a => a.Name);
                    break;
                case "description":
                    animals = animals.OrderBy(a => a.Description);
                    break;
                case "category":
                    animals = animals.OrderBy(a => a.Category);
                    break;
                case "area":
                    animals = animals.OrderBy(a => a.Area);
                    break;
                default:
                    return BadRequest("Invalid order by value");
            }

            return Ok(await animals.ToListAsync());
        }

        // POST: api/Animals
        [HttpPost]
        public async Task<IActionResult> AddAnimal([FromBody] Animal animal)
        {
            if (!ModelState.IsValid)
            {
                return BadRequest(ModelState);
            }

            _context.Animals.Add(animal);
            await _context.SaveChangesAsync();
            return CreatedAtAction(nameof(GetAnimal), new { id = animal.Id }, animal);
        }

        // GET: api/Animals/5
        [HttpGet("{id}")]
        public async Task<IActionResult> GetAnimal(int id)
        {
            var animal = await _context.Animals.FindAsync(id);

            if (animal == null)
            {
                return NotFound();
            }

            return Ok(animal);
        }

        // PUT: api/Animals/5
        [HttpPut("{id}")]
        public async Task<IActionResult> UpdateAnimal(int id, [FromBody] Animal animal)
        {
            if (id != animal.Id)
            {
                return BadRequest("ID mismatch");
            }

            _context.Entry(animal).State = EntityState.Modified;

            try
            {
                await _context.SaveChangesAsync();
            }
            catch (DbUpdateConcurrencyException)
            {
                if (!AnimalExists(id))
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

        // DELETE: api/Animals/5
        [HttpDelete("{id}")]
        public async Task<IActionResult> DeleteAnimal(int id)
        {
            var animal = await _context.Animals.FindAsync(id);
            if (animal == null)
            {
                return NotFound();
            }

            _context.Animals.Remove(animal);
            await _context.SaveChangesAsync();
            return NoContent();
        }

        private bool AnimalExists(int id)
        {
            return _context.Animals.Any(e => e.Id == id);
        }
    }
}

