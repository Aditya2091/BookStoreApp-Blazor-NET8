using System;
using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;
using Microsoft.AspNetCore.Http;
using Microsoft.AspNetCore.Mvc;
using Microsoft.EntityFrameworkCore;
using BookStoreApp.API.Data;
using BookStoreApp.API.Dtos.Author;
using AutoMapper;
using Microsoft.AspNetCore.Authorization;

namespace BookStoreApp.API.Controllers
{
   [Route("api/[controller]")]
   [ApiController]
   [Authorize]
   public class AuthorsController : ControllerBase
   {
      private readonly BookStoreDbContext _context;
      private readonly IMapper mapper;

      public AuthorsController(BookStoreDbContext context, IMapper mapper)
      {
         _context = context;
         this.mapper=mapper;
      }

      // GET: api/Authors
      [HttpGet]
      public async Task<ActionResult<IEnumerable<AuthorReadOnlyDto>>> GetAuthors()
      {
         var authors = mapper.Map<IEnumerable<AuthorReadOnlyDto>>(await _context.Authors.ToListAsync());
         return Ok(authors);
      }

      // GET: api/Authors/5
      [HttpGet("{id}")]
      public async Task<ActionResult<AuthorReadOnlyDto>> GetAuthor(int id)
      {
         var author = await _context.Authors.FindAsync(id);

         if (author == null)
         {
            return NotFound();
         }

         var authorDto = mapper.Map<AuthorReadOnlyDto>(author);

         return Ok(authorDto);
      }

      // PUT: api/Authors/5
      // To protect from overposting attacks, see https://go.microsoft.com/fwlink/?linkid=2123754
      [HttpPut("{id}")]
      [Authorize(Roles = "Administrator")]
      public async Task<IActionResult> PutAuthor(int id, AuthorUpdateDto authorDto)
      {
         if (id != authorDto.Id)
         {
            return BadRequest();
         }

         var author = await _context.Authors.FindAsync(id);

         if (author == null)
         {
            return NotFound();
         }

         mapper.Map(authorDto, author);
         _context.Entry(author).State = EntityState.Modified;

         try
         {
            await _context.SaveChangesAsync();
         }
         catch (DbUpdateConcurrencyException)
         {
            if (!await AuthorExists(id))
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

      //POST: api/Authors
      //To protect from overposting attacks, see https://go.microsoft.com/fwlink/?linkid=2123754
      [HttpPost]
      [Authorize(Roles = "Administrator")]
      public async Task<ActionResult<AuthorCreateDto>> PostAuthor(AuthorCreateDto authorDto)
      {
         var author = mapper.Map<Author>(authorDto);
         _context.Authors.Add(author);
         await _context.SaveChangesAsync();

         return CreatedAtAction(nameof(GetAuthor), new { id = author.Id }, author);
      }

      // DELETE: api/Authors/5
      [HttpDelete("{id}")]
      [Authorize(Roles = "Administrator")]
      public async Task<IActionResult> DeleteAuthor(int id)
      {
         var author = await _context.Authors.FindAsync(id);
         if (author == null)
         {
            return NotFound();
         }

         _context.Authors.Remove(author);
         await _context.SaveChangesAsync();

         return NoContent();
      }

      private async Task<bool> AuthorExists(int id)
      {
         return await _context.Authors.AnyAsync(e => e.Id == id);
      }
   }
}
