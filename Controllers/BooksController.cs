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

    [HttpGet]
    public IActionResult GetAll()
    {
        return Ok(_bookService.GetAll());
    }

    [HttpGet("{id}")]
    public IActionResult GetById(int id)
    {
        var book = _bookService.GetById(id);

        if (book is null)
        {
            return NotFound();
        }

        return Ok(book);
    }

    [HttpPost]
    public IActionResult Create(CreateBookRequest request)
    {
        var createdBook = _bookService.Add(request);

        return Created(
            $"/api/books/{createdBook.Id}",
            createdBook
        );
    }

    [HttpPut("{id}")]
    public IActionResult Update(int id, UpdateBookRequest request)
    {
        var updated = _bookService.Update(id, request);

        if (!updated)
        {
            return NotFound();
        }

        return Ok(_bookService.GetById(id));
    }

    [HttpDelete("{id}")]
    public IActionResult Delete(int id)
    {
        var deleted = _bookService.Delete(id);

        if (!deleted)
        {
            return NotFound();
        }

        return NoContent();
    }
}