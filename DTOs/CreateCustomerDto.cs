using CustomerImageAPI.Models;

namespace CustomerImageAPI.DTOs;

public class CreateCustomerDto
{
    public string Name { get; set; } = string.Empty;
    public string EmailAddress { get; set; } = string.Empty;
    public string PhoneNumber { get; set; } = string.Empty;
    public CustomerType Type { get; set; }
}
