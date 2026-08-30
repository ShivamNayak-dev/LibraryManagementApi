using LibraryManagementApi.DTOs.Books;

namespace LibraryManagementApi.Services;

public interface IBookService
{
    List<BookResponse> GetAll();

    BookResponse? GetById(int id);

    BookResponse Add(CreateBookRequest request);

    bool Update(int id, UpdateBookRequest request);

    bool Delete(int id);
}