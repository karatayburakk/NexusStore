namespace NexusStore.API.Dtos
{
  public class CountryResponseDto
  {
    public required string NameTr { get; set; }
    public required string NameEn { get; set; }
    public required string ShortCode { get; set; }
    public required string PhoneCode { get; set; }
  }
}