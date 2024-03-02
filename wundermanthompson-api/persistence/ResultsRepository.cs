using Microsoft.EntityFrameworkCore;
using wundermanthompson_api.model;

namespace wundermanthompson_api.persistence;

public interface IResultsRepository
{
    Task<IEnumerable<Result>> GetResultsByDataJobId(Guid dataJobId);
    Task DeleteByDataJobId(Guid dataJobId);
}

public class ResultsRepository(InMemoryDbContext dbContext) : IResultsRepository
{
    public InMemoryDbContext _context = dbContext;

    public Task DeleteByDataJobId(Guid dataJobId)
    {
        var results = _context.Results.Where(r => r.DataJobId == dataJobId);
        _context.Results.RemoveRange(results);
        return _context.SaveChangesAsync();
    }

    public async Task<IEnumerable<Result>> GetResultsByDataJobId(Guid dataJobId)
    {
        return await _context.Results.Where(r => r.DataJobId == dataJobId).ToListAsync();
    }
}