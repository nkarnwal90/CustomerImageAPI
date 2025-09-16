using CustomerImageAPI.DTOs;
using CustomerImageAPI.Models;

namespace CustomerImageAPI.Services;

public interface ICustomerService
{
    Task<Customer?> GetCustomerAsync(Guid customerId);
    Task<List<ImageEntry>> GetImagesAsync(Guid customerId);
    Task<bool> AddImagesAsync(Guid customerId, List<string> base64Images);
    Task<bool> DeleteImageAsync(Guid imageId);
    Task<Customer> CreateCustomerAsync(CreateCustomerDto dto);
    Task<List<CustomerResponseDto>> GetAllCustomersAsync(CustomerType? type);


}
