namespace wundermanthompson_api.DTO;

public class LinkDTO
{
  public string Rel { get; set; }    
  public string Href { get; set; }
  public string Action { get; set; }
  public string[] Types { get; set; }
}