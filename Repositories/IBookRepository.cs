using LibraryManagementApi.Models;

namespace LibraryManagementApi.Repositories;

public interface IBookRepository
{
    Task<List<Book>> GetAllAsync();

    Task<Book?> GetByIdAsync(int id);

    Task<Book> AddAsync(Book book);

    Task<bool> UpdateAsync(Book book);

    Task<bool> DeleteAsync(Book book);
}