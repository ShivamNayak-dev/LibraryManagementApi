using LibraryManagementApi.DTOs.Books;
using LibraryManagementApi.Models;
using LibraryManagementApi.Repositories;

namespace LibraryManagementApi.Services;

public class BookService : IBookService
{
    private readonly IBookRepository _bookRepository;

    public BookService(IBookRepository bookRepository)
    {
        _bookRepository = bookRepository;
    }

    public async Task<List<BookResponse>> GetAllAsync()
    {
        var books = await _bookRepository.GetAllAsync();

        return books.Select(ToResponse).ToList();
    }

    public async Task<BookResponse?> GetByIdAsync(int id)
    {
        var book = await _bookRepository.GetByIdAsync(id);

        return book is null ? null : ToResponse(book);
    }

    public async Task<BookResponse> AddAsync(CreateBookRequest request)
    {
        var book = new Book
        {
            Title = request.Title,
            Author = request.Author,
            ISBN = request.ISBN,
            Price = request.Price,
            AvailableCopies = request.AvailableCopies
        };

        var createdBook = await _bookRepository.AddAsync(book);

        return ToResponse(createdBook);
    }

    public async Task<bool> UpdateAsync(
        int id,
        UpdateBookRequest request)
    {
        var book = await _bookRepository.GetByIdAsync(id);

        if (book is null)
        {
            return false;
        }

        book.Title = request.Title;
        book.Author = request.Author;
        book.ISBN = request.ISBN;
        book.Price = request.Price;
        book.AvailableCopies = request.AvailableCopies;

        return await _bookRepository.UpdateAsync(book);
    }

    public async Task<bool> DeleteAsync(int id)
    {
        var book = await _bookRepository.GetByIdAsync(id);

        if (book is null)
        {
            return false;
        }

        return await _bookRepository.DeleteAsync(book);
    }

    private static BookResponse ToResponse(Book book)
    {
        return new BookResponse
        {
            Id = book.Id,
            Title = book.Title,
            Author = book.Author,
            ISBN = book.ISBN,
            Price = book.Price,
            AvailableCopies = book.AvailableCopies
        };
    }
}