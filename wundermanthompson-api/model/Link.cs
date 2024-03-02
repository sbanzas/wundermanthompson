namespace wundermanthompson_api.model;

public class Link {
    public Guid Id { get; set; }
    public Guid DataJobId { get; set; }
    public string Rel { get; set; }    
    public string Href { get; set; }
    public string Action { get; set; }
    public string[] Types { get; set; }
}