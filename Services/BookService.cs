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

    public List<BookResponse> GetAll()
    {
        var books = _bookRepository.GetAll();

        return books.Select(ToResponse).ToList();
    }

    public BookResponse? GetById(int id)
    {
        var book = _bookRepository.GetById(id);

        return book is null ? null : ToResponse(book);
    }

    public BookResponse Add(CreateBookRequest request)
    {
        var book = new Book
        {
            Title = request.Title,
            Author = request.Author,
            ISBN = request.ISBN,
            Price = request.Price,
            AvailableCopies = request.AvailableCopies
        };

        var createdBook = _bookRepository.Add(book);

        return ToResponse(createdBook);
    }

    public bool Update(int id, UpdateBookRequest request)
    {
        var book = _bookRepository.GetById(id);

        if (book is null)
        {
            return false;
        }

        book.Title = request.Title;
        book.Author = request.Author;
        book.ISBN = request.ISBN;
        book.Price = request.Price;
        book.AvailableCopies = request.AvailableCopies;

        return _bookRepository.Update(book);
    }

    public bool Delete(int id)
    {
        var book = _bookRepository.GetById(id);

        if (book is null)
        {
            return false;
        }

        return _bookRepository.Delete(book);
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