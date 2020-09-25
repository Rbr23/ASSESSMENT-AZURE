using System;
using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;
using Microsoft.AspNetCore.Http;
using Microsoft.AspNetCore.Mvc;
using Microsoft.EntityFrameworkCore;
using Api.Amigos.Data;
using Api.Amigos.Domain;
using AutoMapper;

namespace Api.Amigos.Controllers
{
    [Route("api/[controller]")]
    [ApiController]
    public class AmigosController : ControllerBase
    {
        private readonly ApiAmigosContext _context;
        private readonly IMapper _mapper;

        public AmigosController(ApiAmigosContext context, IMapper mapper)
        {
            _context = context;
            _mapper = mapper;
        }

        // GET: api/Amigos
        [HttpGet]
        public async Task<ActionResult<IEnumerable<Amigos.Domain.Amigos>>> GetAmigos()
        {
            return await _context.Amigos.ToListAsync();
        }

        // GET: api/Amigos/5
        [HttpGet("{id}")]
        public async Task<ActionResult<Amigos.Domain.Amigos>> GetAmigos(Guid id)
        {
            var amigos = await _context.Amigos.FindAsync(id);

            if (amigos == null)
            {
                return NotFound();
            }

            return amigos;
        }

        // PUT: api/Amigos/5
        // To protect from overposting attacks, enable the specific properties you want to bind to, for
        // more details, see https://go.microsoft.com/fwlink/?linkid=2123754.
        [HttpPut("{id}")]
        public async Task<IActionResult> PutAmigos(Guid id, Amigos.Domain.Amigos amigos)
        {
            if (id != amigos.Id)
            {
                return BadRequest();
            }

            _context.Entry(amigos).State = EntityState.Modified;

            try
            {
                await _context.SaveChangesAsync();
            }
            catch (DbUpdateConcurrencyException)
            {
                if (!AmigosExists(id))
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

        // POST: api/Amigos
        // To protect from overposting attacks, enable the specific properties you want to bind to, for
        // more details, see https://go.microsoft.com/fwlink/?linkid=2123754.
        [HttpPost]
        public async Task<ActionResult<Amigos.Domain.Amigos>> PostAmigos(Amigos.Domain.Amigos amigos)
        {
            _context.Amigos.Add(amigos);
            await _context.SaveChangesAsync();

            return CreatedAtAction("GetAmigos", new { id = amigos.Id }, amigos);
        }

        // DELETE: api/Amigos/5
        [HttpDelete("{id}")]
        public async Task<ActionResult<Amigos.Domain.Amigos>> DeleteAmigos(Guid id)
        {
            var amigos = await _context.Amigos.FindAsync(id);
            if (amigos == null)
            {
                return NotFound();
            }

            _context.Amigos.Remove(amigos);
            await _context.SaveChangesAsync();

            return amigos;
        }

        private bool AmigosExists(Guid id)
        {
            return _context.Amigos.Any(e => e.Id == id);
        }

        [HttpGet("{id}/Amigos")]
        public async Task<ActionResult> GetAmigo([FromRoute] Guid id)
        {
            var pessoa = await _context.Amigos.Include(x => x.Amigo).FirstOrDefaultAsync(x => x.Id == id);

            if (pessoa == null)
            {
                return NotFound();
            }

            var response = _mapper.Map<List<AmigoResponse>>(pessoa.Amigo);

            return Ok(response);
        }

        [HttpPost("{id}/Amigos")]
        public async Task<ActionResult> PostAmigos([FromRoute] Guid id, [FromBody] Guid idAmigo)
        {
            
            var pessoa = await _context.Amigos.FindAsync(id);

            if (pessoa == null)
            {
                return NotFound();
            }

            var amigo = await _context.Amigos.FindAsync(idAmigo);

            pessoa.Amigo.Add(amigo);

            _context.Amigos.Update(pessoa);
            await _context.SaveChangesAsync();

            return Ok();
        }

        [HttpDelete("{id}/Amigos")]
        public async Task<ActionResult> DeleteAmigos([FromRoute] Guid id, [FromBody] Guid idAmigo)
        {
            var pessoa = await _context.Amigos.FindAsync(id);

            if (pessoa == null)
            {
                return NotFound();
            }

            var amigo = await _context.Amigos.FindAsync(idAmigo);

            pessoa.Amigo.Remove(amigo);

            _context.Amigos.Update(pessoa);
            await _context.SaveChangesAsync();

            return Ok();
        }
    }
}
