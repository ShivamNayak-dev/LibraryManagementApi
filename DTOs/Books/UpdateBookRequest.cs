namespace LibraryManagementApi.DTOs.Books;

public class UpdateBookRequest
{
    public string Title { get; set; } = string.Empty;

    public string Author { get; set; } = string.Empty;

    public string ISBN { get; set; } = string.Empty;

    public decimal Price { get; set; }

    public int AvailableCopies { get; set; }
}