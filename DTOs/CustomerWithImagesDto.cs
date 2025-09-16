using CustomerImageAPI.DTOs;
using CustomerImageAPI.Models;

public class CustomerWithImagesDto
{
    public Guid Id { get; set; }
    public string Name { get; set; } = string.Empty;
    public string EmailAddress { get; set; } = string.Empty;
    public string PhoneNumber { get; set; } = string.Empty;
    public CustomerType Type { get; set; }

    public List<ImageDto> Images { get; set; } = new();
}
