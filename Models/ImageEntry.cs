using System.ComponentModel.DataAnnotations;
using System.ComponentModel.DataAnnotations.Schema;

namespace CustomerImageAPI.Models;

public class ImageEntry
{
    public Guid Id { get; set; } = Guid.NewGuid();

    [Required]
    public string Base64Data { get; set; } = string.Empty;

    [ForeignKey("Customer")]
    public Guid CustomerId { get; set; }

    public Customer? Customer { get; set; }
}
