using CustomerImageAPI.DTOs;
using CustomerImageAPI.Models;
using CustomerImageAPI.Services;
using Microsoft.AspNetCore.Mvc;

namespace CustomerImageAPI.Controllers;

[ApiController]
[Route("api/customers")]
public class CustomerController : ControllerBase
{
    private readonly ICustomerService _service;

    public CustomerController(ICustomerService service)
    {
        _service = service;
    }

    [HttpPost]
    public async Task<IActionResult> CreateCustomer([FromBody] CreateCustomerDto dto)
    {
        if (string.IsNullOrWhiteSpace(dto.Name) || string.IsNullOrWhiteSpace(dto.EmailAddress))
            return BadRequest("Name and Email are required.");

        var customer = await _service.CreateCustomerAsync(dto);

        var response = new CustomerWithImagesDto
        {
            Id = customer.Id,
            Name = customer.Name,
            EmailAddress = customer.EmailAddress,
            PhoneNumber = customer.PhoneNumber,
            Type = customer.Type,
            Images = new List<ImageDto>()
        };

        return CreatedAtAction(nameof(GetCustomerById), new { id = customer.Id }, response);
    }

    [HttpGet("{id}")]
    public async Task<IActionResult> GetCustomerById(Guid id)
    {
        var customer = await _service.GetCustomerAsync(id);
        if (customer == null)
            return NotFound();

        var dto = new CustomerWithImagesDto
        {
            Id = customer.Id,
            Name = customer.Name,
            EmailAddress = customer.EmailAddress,
            PhoneNumber = customer.PhoneNumber,
            Type = customer.Type,
            Images = customer.Images.Select(img => new ImageDto
            {
                Id = img.Id,
                Base64Data = img.Base64Data
            }).ToList()
        };

        return Ok(dto);
    }

    [HttpGet]
    public async Task<ActionResult<List<CustomerResponseDto>>> GetAllCustomers([FromQuery] CustomerType? type = null)
    {
        var customers = await _service.GetAllCustomersAsync(type);
        return Ok(customers);
    }

    [HttpGet("{id}/images")]
    public async Task<IActionResult> GetImages(Guid id)
    {
        var images = await _service.GetImagesAsync(id);
        return Ok(images);
    }

    [HttpPost("{id}/images")]
    public async Task<IActionResult> UploadImages(Guid id, [FromBody] ImageUploadDto dto)
    {
        if (dto?.Base64Images == null || dto.Base64Images.Count == 0)
            return BadRequest("No images provided.");

        try
        {
            // Check if customer exists
            var customer = await _service.GetCustomerAsync(id);
            if (customer == null)
                return NotFound($"Customer with ID {id} not found.");

            // Try to add images
            bool success = await _service.AddImagesAsync(id, dto.Base64Images);

            if (!success)
                return BadRequest("Image upload failed. Limit of 10 images per customer exceeded.");

            return Ok("Images uploaded successfully.");
        }
        catch (Exception ex)
        {
            // Log exception here as needed (e.g., ILogger)
            return StatusCode(500, $"An error occurred: {ex.Message}");
        }
    }


    [HttpDelete("images/{imageId}")]
    public async Task<IActionResult> DeleteImage(Guid imageId)
    {
        bool success = await _service.DeleteImageAsync(imageId);
        if (!success)
            return NotFound("Image not found.");

        return Ok("Image deleted.");
    }
}
