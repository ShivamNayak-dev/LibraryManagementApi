using LibraryManagementApi.DTOs.Books;
using LibraryManagementApi.Services;
using Microsoft.AspNetCore.Mvc;

namespace LibraryManagementApi.Controllers;

[ApiController]
[Route("api/[controller]")]
public class BooksController : ControllerBase
{
    private readonly IBookService _bookService;

    public BooksController(IBookService bookService)
    {
        _bookService = bookService;
    }

    // GET: api/books
    [HttpGet]
    public async Task<IActionResult> GetAll()
    {
        var books = await _bookService.GetAllAsync();

        return Ok(books);
    }

    // GET: api/books/{id}
    [HttpGet("{id}")]
    public async Task<IActionResult> GetById(int id)
    {
        var book = await _bookService.GetByIdAsync(id);

        if (book is null)
        {
            return NotFound();
        }

        return Ok(book);
    }

    // POST: api/books
    [HttpPost]
    public async Task<IActionResult> Create(CreateBookRequest request)
    {
        var createdBook = await _bookService.AddAsync(request);

        return Created(
            $"/api/books/{createdBook.Id}",
            createdBook
        );
    }

    // PUT: api/books/{id}
    [HttpPut("{id}")]
    public async Task<IActionResult> Update(
        int id,
        UpdateBookRequest request)
    {
        var updated = await _bookService.UpdateAsync(id, request);

        if (!updated)
        {
            return NotFound();
        }

        var updatedBook = await _bookService.GetByIdAsync(id);

        return Ok(updatedBook);
    }

    // DELETE: api/books/{id}
    [HttpDelete("{id}")]
    public async Task<IActionResult> Delete(int id)
    {
        var deleted = await _bookService.DeleteAsync(id);

        if (!deleted)
        {
            return NotFound();
        }

        return NoContent();
    }
}