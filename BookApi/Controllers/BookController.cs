using BookApi.Models;
using BookApi.Repository;
using Microsoft.AspNetCore.Http;
using Microsoft.AspNetCore.Mvc;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;

namespace BookApi.Controllers
{
    [Route("api/book")]
    [ApiController]
    public class BookController : ControllerBase
    {
        private readonly IBookRepository _bookRepository;

        public BookController(IBookRepository bookRepository)
        {
            _bookRepository = bookRepository;
        }
        [HttpGet]
        public async Task<IEnumerable<Book>> GetBooks()
        {
            var books = await _bookRepository.Get();
            return books;
        }
        [HttpGet("{id}")]
        public async Task<IActionResult> GetBook(int id)
        {
            var book = await _bookRepository.Get(id);
            if (book is null)
                return NotFound();
            return Ok(book);
        }

        [HttpPost]

        public async Task<IActionResult> CreateBook(Book book)
        {
            var newBook = await _bookRepository.Create(book);
            return CreatedAtAction(nameof(GetBooks), new { newBook.Id }, newBook);
        }
        [HttpPut]

        public async Task<IActionResult> UpdateBook(int id, Book book)
        {
            if(id != book.Id)
            {
                return BadRequest();
            }
            await _bookRepository.Update(book);
            return NoContent();
        }

        [HttpDelete]
        public async Task<IActionResult> DeleteBook(int id)
        {
            var bookToDelete = await _bookRepository.Get(id);
            if(bookToDelete is null)
            {
                return NotFound();
            }
            await _bookRepository.Delete(id);
            return NoContent();
        }
    }
}
