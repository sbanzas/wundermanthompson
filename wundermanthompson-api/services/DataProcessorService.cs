using wundermanthompson_api.DTO;
using wundermanthompson_api.Enums;
using wundermanthompson_api.model;
using wundermanthompson_api.persistence;

namespace wundermanthompson_api.services;

public interface IDataProcessorService
{
  Task<IEnumerable<DataJobDTO>> GetAllDataJobs();
  Task<IEnumerable<DataJobDTO>> GetDataJobsByStatus(DataJobStatus status);
  Task<DataJobDTO> GetDataJob(Guid id);
  Task<DataJobDTO> Create(DataJobDTO dataJob);
  Task<DataJobDTO> Update(Guid datajoobId, DataJobDTO dataJob);
  Task Delete(Guid dataJobID);
  Task<bool> StartBackgroundProcess(Guid dataJobId);
  Task<DataJobStatus?> GetBackgroundProcessStatus(Guid dataJobId);
  Task<List<string>> GetBackgroundProcessResults(Guid dataJobId);
}

public class DataProcessorService(IDataJobRepository dataJobRepository, ILinksRepository linksRepository, IResultsRepository resultsRepository) : IDataProcessorService
{
  private readonly IDataJobRepository _dataJobRepository = dataJobRepository;
  private readonly ILinksRepository _linksRepository = linksRepository;
  private readonly IResultsRepository _resultsRepository = resultsRepository;

    public async Task<DataJobDTO> Create(DataJobDTO dataJobDto)
    {
      var dataJob = new DataJob {
        FilePathToProcess = dataJobDto.FilePathToProcess,
        Id = Guid.NewGuid(),
        Name = dataJobDto.Name,
        Status = DataJobStatus.New
      };

      dataJob = await _dataJobRepository.Create(dataJob);

      var linksToInsert = dataJobDto.Links.Select(l => new Link
      {
        Action = l.Action,
        DataJobId = dataJob.Id,
        Id = new Guid(),
        Href = l.Href,
        Rel = l.Rel,
        Types = l.Types
      });

      var insertedLinks = await _linksRepository.Insert(linksToInsert);

      return MapDataJobToDataJobDTO(dataJob, insertedLinks, []);
    }

    public async Task Delete(Guid dataJobID)
    {
        await _resultsRepository.DeleteByDataJobId(dataJobID);
        await _dataJobRepository.Delete(dataJobID);
    }

    public async Task<IEnumerable<DataJobDTO>> GetAllDataJobs()
    {
       var dataJobs = await _dataJobRepository.Get();

       return dataJobs.Select(d => MapDataJobToDataJobDTO(d, [], []));
    }

    public async Task<List<string>> GetBackgroundProcessResults(Guid dataJobId)
    {
        var results = await _resultsRepository.GetResultsByDataJobId(dataJobId);
        return results.Select(r => r.Value).ToList();
    }

    public async Task<DataJobStatus?> GetBackgroundProcessStatus(Guid dataJobId)
    {
        var dataJob = await _dataJobRepository.GetById(dataJobId);
        if (dataJob == null)
          return null;
        return dataJob.Status;
    }

    public async Task<DataJobDTO> GetDataJob(Guid id)
    {
        var dataJob = await _dataJobRepository.GetById(id);

        if (dataJob == null) 
          return null;

        return MapDataJobToDataJobDTO(dataJob, [], []);
    }

    public async Task<IEnumerable<DataJobDTO>> GetDataJobsByStatus(DataJobStatus status)
    {
        var dataJobs = await _dataJobRepository.Get(d => d.Status == status);
        return dataJobs.Select(d => MapDataJobToDataJobDTO(d, [], []));
    }

    public async Task<bool> StartBackgroundProcess(Guid dataJobId)
    {
        var dataJob = await _dataJobRepository.GetById(dataJobId);
        dataJob.Status = DataJobStatus.Processing;
        await _dataJobRepository.Update(dataJob);

        return true;
    }

    public async Task<DataJobDTO> Update(Guid dataJobId, DataJobDTO dataJobDto)
    {
        var dataJob = await _dataJobRepository.GetById(dataJobId);
        dataJob.Name = dataJobDto.Name;
        dataJob.FilePathToProcess = dataJobDto.FilePathToProcess;
        
        dataJob = await _dataJobRepository.Update(dataJob);

        await _linksRepository.DeleteByDataJobId(dataJobId);
        var linksToInsert = dataJobDto.Links.Select(l => new Link
        {
          Action = l.Action,
          DataJobId = dataJob.Id,
          Id = new Guid(),
          Href = l.Href,
          Rel = l.Rel,
          Types = l.Types
        });

      var insertedLinks = await _linksRepository.Insert(linksToInsert);        

        return MapDataJobToDataJobDTO(dataJob, insertedLinks, []);
    }

    private static DataJobDTO MapDataJobToDataJobDTO(DataJob dataJob, IEnumerable<Link> insertedLinks, List<string> results)
    {
      return new DataJobDTO {
        FilePathToProcess = dataJob.FilePathToProcess,
        Id = dataJob.Id,
        Links = insertedLinks.Select(l => MapLinkToLinkDTO(l)),
        Name = dataJob.Name,
        Results = results,
        Status = dataJob.Status
      };
    }

    private static LinkDTO MapLinkToLinkDTO(Link link)
    {
      return new LinkDTO
      {
        Action = link.Action,
        Href = link.Href,
        Rel = link.Rel,
        Types = link.Types
      };
    }
}