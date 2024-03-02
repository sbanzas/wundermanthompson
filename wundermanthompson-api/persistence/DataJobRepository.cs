using System.Linq.Expressions;
using Microsoft.EntityFrameworkCore;
using wundermanthompson_api.model;

namespace wundermanthompson_api.persistence;

public interface IDataJobRepository
{
    Task<DataJob> Create(DataJob dataJob);
    Task Delete(Guid id);
    Task<IEnumerable<DataJob>> Get(Expression<Func<DataJob, bool>> filter = null);
    Task<DataJob> GetById(Guid id);
    Task<DataJob> Update(DataJob dataJob);
}

public class DataJobRepository(InMemoryDbContext dbContext) : IDataJobRepository
{
    public InMemoryDbContext _context = dbContext;

    public async Task<DataJob> Create(DataJob result)
    {
        await _context.DataJobs.AddAsync(result);
        await _context.SaveChangesAsync();
        return result;
    }

    public async Task Delete(Guid id)
    {
        var result = await _context.DataJobs.FirstOrDefaultAsync(d => d.Id == id);
        _context.DataJobs.Remove(result);
        await _context.SaveChangesAsync();
    }

    public async Task<IEnumerable<DataJob>> Get(Expression<Func<DataJob, bool>> filter = null)
    {
        if (filter == null)
            return await _context.DataJobs.ToListAsync();

        return await _context.DataJobs.Where(filter).ToListAsync();
    }

    public async Task<DataJob> GetById(Guid id)
    {
        return await _context.DataJobs.FirstOrDefaultAsync(d => d.Id == id);
    }

    public async Task<DataJob> Update(DataJob dataJob)
    {
        _context.DataJobs.Update(dataJob);
        await _context.SaveChangesAsync();
        return dataJob;
    }
}