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

    public List<Book> GetAll()
    {
        return _context.Books.ToList();
    }

    public Book? GetById(int id)
    {
        return _context.Books.Find(id);
    }

    public Book Add(Book book)
    {
        _context.Books.Add(book);
        _context.SaveChanges();

        return book;
    }

    public bool Update(Book book)
    {
        _context.Books.Update(book);
        _context.SaveChanges();

        return true;
    }

    public bool Delete(Book book)
    {
        _context.Books.Remove(book);
        _context.SaveChanges();

        return true;
    }
}