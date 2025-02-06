
namespace NexusStore.API.Entities
{
  public class Country : BaseEntity
  {
    public required string ShortCode { get; set; }
    public required string PhoneCode { get; set; }
    public required string NameTr { get; set; }

    public required string NameEn { get; set; }
  }
}
