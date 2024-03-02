using wundermanthompson_api.model;

namespace wundermanthompson_api.persistence;

public interface ILinksRepository
{
    Task<IEnumerable<Link>> GetResultsByDataJobId(Guid dataJobId);
    Task<IEnumerable<Link>> Insert(IEnumerable<Link> results);
    Task DeleteByDataJobId(Guid dataJobId);
}

public class LinksRepository(InMemoryDbContext dbContext) : ILinksRepository
{
    public InMemoryDbContext _context = dbContext;

    public async Task<IEnumerable<Link>> Insert(IEnumerable<Link> linksToInsert)
    {
        _context.Links.AddRange(linksToInsert);
        await _context.SaveChangesAsync();

        return linksToInsert;
    }

    public async Task DeleteByDataJobId(Guid dataJobId)
    {
        var links = _context.Links.Where(l => l.DataJobId == dataJobId);
        _context.Links.RemoveRange(links);
        await _context.SaveChangesAsync();
    }

    public Task<IEnumerable<Link>> GetResultsByDataJobId(Guid dataJobId)
    {
        throw new NotImplementedException();
    }
}