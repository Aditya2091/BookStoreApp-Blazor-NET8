﻿using System;
using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;
using Microsoft.AspNetCore.Http;
using Microsoft.AspNetCore.Mvc;
using Microsoft.EntityFrameworkCore;
using BookStoreApp.API.Data;
using AutoMapper;
using BookStoreApp.API.Dtos.Book;
using AutoMapper.QueryableExtensions;
using Microsoft.AspNetCore.Authorization;

namespace BookStoreApp.API.Controllers
{
   [Route("api/[controller]")]
   [ApiController]
   [Authorize]
   public class BooksController : ControllerBase
   {
      private readonly BookStoreDbContext _context;
      private readonly IMapper mapper;

      public BooksController(BookStoreDbContext context, IMapper mapper)
      {
         _context = context;
         this.mapper = mapper;
      }

      // GET: api/Books
      [HttpGet]
      public async Task<ActionResult<IEnumerable<BookReadOnlyDto>>> GetBooks()
      {
         var bookDto = await _context.Books.Include(b => b.Author).ProjectTo<BookReadOnlyDto>(mapper.ConfigurationProvider).ToListAsync();
         return Ok(bookDto);
      }

      // GET: api/Books/5
      [HttpGet("{id}")]
      public async Task<ActionResult<BookReadOnlyDto>> GetBook(int id)
      {
         var book = await _context.Books.Include(b => b.Author).ProjectTo<BookReadOnlyDto>(mapper.ConfigurationProvider).FirstOrDefaultAsync(b => b.Id == id);

         if (book == null)
         {
            return NotFound();
         } 

         return book;
      }

      // PUT: api/Books/5
      // To protect from overposting attacks, see https://go.microsoft.com/fwlink/?linkid=2123754
      [HttpPut("{id}")]
      [Authorize(Roles = "Administrator")]
      public async Task<IActionResult> PutBook(int id, BookUpdateDto bookDto)
      {
         if (id != bookDto.Id)
         {
            return BadRequest();
         }

         var book = await _context.Books.FindAsync(id); 
         
         if (book == null)
         {
            return NotFound();
         }

         mapper.Map(bookDto, book);

         _context.Entry(book).State = EntityState.Modified;

         try
         {
            await _context.SaveChangesAsync();
         }
         catch (DbUpdateConcurrencyException)
         {
            if (!BookExists(id))
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

      // POST: api/Books
      // To protect from overposting attacks, see https://go.microsoft.com/fwlink/?linkid=2123754
      [HttpPost]
      [Authorize(Roles = "Administrator")]
      public async Task<ActionResult<Book>> PostBook(BookCreateDto bookDto)
      {
         var book = mapper.Map<Book>(bookDto);
         _context.Books.Add(book);
         await _context.SaveChangesAsync();

         return CreatedAtAction(nameof(GetBook), new { id = book.Id }, book);
      }

      // DELETE: api/Books/5
      [HttpDelete("{id}")]
      [Authorize(Roles = "Administrator")]
      public async Task<IActionResult> DeleteBook(int id)
      {
         var book = await _context.Books.FindAsync(id);
         if (book == null)
         {
            return NotFound();
         }

         _context.Books.Remove(book);
         await _context.SaveChangesAsync();

         return NoContent();
      }

      private bool BookExists(int id)
      {
         return _context.Books.Any(e => e.Id == id);
      }
   }
}
