namespace LibraryManagementApi.DTOs.Books;
using System.ComponentModel.DataAnnotations;
public class UpdateBookRequest
{
    [Required]
    [StringLength(200)]
    public string Title { get; set; } = string.Empty;

     [Required]
    [StringLength(150)]
    public string Author { get; set; } = string.Empty;

     [Required]
    [StringLength(20)]
    public string ISBN { get; set; } = string.Empty;

    [Range(0.01, 1000000)]
    public decimal Price { get; set; }

    [Range(0, 10000)]
    public int AvailableCopies { get; set; }
}