using System;

namespace CustomerImageAPI.DTOs;

public class ImageDto
{
    public Guid Id { get; set; }
    public string Base64Data { get; set; } = string.Empty;
}
