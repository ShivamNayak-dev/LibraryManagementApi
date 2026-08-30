using LibraryManagementApi.Data;
using LibraryManagementApi.Models;
using Microsoft.EntityFrameworkCore;

namespace LibraryManagementApi.Repositories;

public class BookRepository : IBookRepository
{
    private readonly LibraryDbContext _context;

    public BookRepository(LibraryDbContext context)
    {
        _context = context;
    }

    public async Task<List<Book>> GetAllAsync()
    {
        return await _context.Books.ToListAsync();
    }

    public async Task<Book?> GetByIdAsync(int id)
    {
        return await _context.Books.FindAsync(id);
    }

    public async Task<Book> AddAsync(Book book)
    {
        _context.Books.Add(book);

        await _context.SaveChangesAsync();

        return book;
    }

    public async Task<bool> UpdateAsync(Book book)
    {
        _context.Books.Update(book);

        await _context.SaveChangesAsync();

        return true;
    }

    public async Task<bool> DeleteAsync(Book book)
    {
        _context.Books.Remove(book);

        await _context.SaveChangesAsync();

        return true;
    }
}