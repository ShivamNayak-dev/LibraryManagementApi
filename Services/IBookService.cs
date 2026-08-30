using LibraryManagementApi.DTOs.Books;

namespace LibraryManagementApi.Services;

public interface IBookService
{
    Task<List<BookResponse>> GetAllAsync();

    Task<BookResponse?> GetByIdAsync(int id);

    Task<BookResponse> AddAsync(CreateBookRequest request);

    Task<bool> UpdateAsync(int id, UpdateBookRequest request);

    Task<bool> DeleteAsync(int id);
}