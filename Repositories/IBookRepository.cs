using LibraryManagementApi.Models;

namespace LibraryManagementApi.Repositories;

public interface IBookRepository
{
    List<Book> GetAll();

    Book? GetById(int id);

    Book Add(Book book);

    bool Update(Book book);

    bool Delete(Book book);
}