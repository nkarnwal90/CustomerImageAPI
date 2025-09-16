namespace CustomerImageAPI.DTOs
{
    public class CustomerResponseDto
    {
        public Guid Id { get; set; }
        public string Name { get; set; } = "";
        public string EmailAddress { get; set; } = "";
        public string PhoneNumber { get; set; } = "";
        public string Type { get; set; } = "";
        public List<ImageDto> Images { get; set; } = new();
    }
}
