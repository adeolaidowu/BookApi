using BookApi.Data;
using BookApi.Models;
using Microsoft.EntityFrameworkCore;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;

namespace BookApi.Repository
{
    public class BookRepository : IBookRepository
    {
        private readonly BookContext _ctx;

        public BookRepository(BookContext ctx)
        {
            _ctx = ctx;
        }
        public async Task<Book> Create(Book book)
        {
            await _ctx.Books.AddAsync(book);
            await _ctx.SaveChangesAsync();
            return book;
        }

        public async Task Delete(int id)
        {
            var bookToDelete = await _ctx.Books.FindAsync(id);
            _ctx.Books.Remove(bookToDelete);
            await _ctx.SaveChangesAsync();
        }

        public async Task<IEnumerable<Book>> Get()
        {
            return await _ctx.Books.ToListAsync();
        }

        public async Task<Book> Get(int id)
        {
            var book = await _ctx.Books.FindAsync(id);
            return book;
        }

        public async Task Update(Book book)
        {
            _ctx.Entry(book).State = EntityState.Modified;
            await _ctx.SaveChangesAsync();
        }
    }
}
