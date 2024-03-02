using wundermanthompson_api.Enums;

namespace wundermanthompson_api.DTO;

public class DataJobDTO
{
  public Guid Id { get; set; }
  public string Name { get; set; }
  public string FilePathToProcess { get; set; }
  public DataJobStatus Status { get; set; } = DataJobStatus.New;
  public IEnumerable<string> Results { get; set; } = [];  
  public IEnumerable<LinkDTO> Links { get; set; } = [];
}