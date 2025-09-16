using CustomerImageAPI.Data;
using CustomerImageAPI.DTOs;
using CustomerImageAPI.Models;
using Microsoft.EntityFrameworkCore;

namespace CustomerImageAPI.Services;

public class CustomerService : ICustomerService
{
    private readonly AppDbContext _context;

    public CustomerService(AppDbContext context)
    {
        _context = context;
    }

    public async Task<Customer?> GetCustomerAsync(Guid customerId)
    {
        return await _context.Customers
            .Include(c => c.Images)
            .FirstOrDefaultAsync(c => c.Id == customerId);
    }

    public async Task<List<ImageEntry>> GetImagesAsync(Guid customerId)
    {
        return await _context.Images
            .Where(i => i.CustomerId == customerId)
            .ToListAsync();
    }

    public async Task<bool> AddImagesAsync(Guid customerId, List<string> base64Images)
    {
        // Count existing images directly in the DB
        var existingImageCount = await _context.Images.CountAsync(i => i.CustomerId == customerId);

        if (existingImageCount + base64Images.Count > 10)
            return false;

        // Create new ImageEntry instances
        var newImages = base64Images.Select(base64 => new ImageEntry
        {
            Id = Guid.NewGuid(),
            Base64Data = base64,
            CustomerId = customerId
        }).ToList();

        // Add new images directly to the context
        await _context.Images.AddRangeAsync(newImages);

        try
        {
            await _context.SaveChangesAsync();
            return true;
        }
        catch (DbUpdateConcurrencyException ex)
        {
            // Log or handle the concurrency exception
            // e.g., _logger.LogError(ex, "Concurrency error while adding images");
            return false;
        }
    }



    public async Task<bool> DeleteImageAsync(Guid imageId)
    {
        var image = await _context.Images.FindAsync(imageId);
        if (image == null) return false;

        _context.Images.Remove(image);

        try
        {
            await _context.SaveChangesAsync();
            return true;
        }
        catch (DbUpdateConcurrencyException)
        {
            // Handle concurrency exception, maybe image was already deleted
            return false;
        }
    }

    public async Task<Customer> CreateCustomerAsync(CreateCustomerDto dto)
    {
        var customer = new Customer
        {
            Name = dto.Name,
            EmailAddress = dto.EmailAddress,
            PhoneNumber = dto.PhoneNumber,
            Type = dto.Type
        };

        _context.Customers.Add(customer);
        await _context.SaveChangesAsync();
        return customer;
    }

    public async Task<List<CustomerResponseDto>> GetAllCustomersAsync(CustomerType? type)
    {
        var query = _context.Customers
            .Include(c => c.Images)
            .AsQueryable();

        if (type.HasValue)
            query = query.Where(c => c.Type == type.Value);

        return await query
            .Select(c => new CustomerResponseDto
            {
                Id = c.Id,
                Name = c.Name,
                EmailAddress = c.EmailAddress,
                PhoneNumber = c.PhoneNumber,
                Type = c.Type.ToString(),
                Images = c.Images.Select(i => new ImageDto
                {
                    Id = i.Id,
                    Base64Data = i.Base64Data
                }).ToList()
            })
            .ToListAsync();
    }
}
