using wundermanthompson_api.Enums;

namespace wundermanthompson_api.model;
public class DataJob
{
    public Guid Id { get; set; }
    public string Name { get; set; }
    public string FilePathToProcess { get; set; }
    public DataJobStatus Status { get; set; } = DataJobStatus.New;
}