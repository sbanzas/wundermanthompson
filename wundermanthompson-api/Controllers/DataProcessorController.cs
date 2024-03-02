using Microsoft.AspNetCore.Mvc;
using wundermanthompson_api.DTO;
using wundermanthompson_api.Enums;
using wundermanthompson_api.services;

namespace wundermanthompson_api.Controllers;

[ApiController]
[Route("[controller]")]
public class DataProcessorController(IDataProcessorService dataProcessorService) : ControllerBase
{
    private readonly IDataProcessorService _dataProcessorService = dataProcessorService;

    /// <summary>
    /// This endpoint queries for all data jobs
    /// </summary>
    /// <returns> All data jobs </returns>
    [HttpGet]
    public async Task<ActionResult<IEnumerable<DataJobDTO>>> Get()
    {
        try
        {
            return Ok(await _dataProcessorService.GetAllDataJobs());
        }
        catch (Exception)
        {
            return StatusCode(500);
        }        
    }

    /// <summary>
    /// This endpoint queries for data jobs in a concrete status
    /// </summary>
    /// <returns> All data jobs in the requested status </returns>
    [HttpGet("status/{status}")]
    public async Task<ActionResult<IEnumerable<DataJobDTO>>> GetByStatus([FromRoute] DataJobStatus status)
    {
        try
        {
            return Ok(await _dataProcessorService.GetDataJobsByStatus(status));
        }
        catch (Exception)
        {
            return StatusCode(500);
        }        
    }

    /// <summary>
    /// This endpoint queries for a specific data job
    /// </summary>
    /// <returns> Specific data job if it exists, otherwise returns 404</returns>
    [HttpGet("{id}")]
    public async Task<ActionResult<DataJobDTO>> Get([FromRoute] Guid id)
    {
        try
        {
            var dataJob = await _dataProcessorService.GetDataJob(id);

            return dataJob != null ? Ok(dataJob) : NotFound();
        }
        catch (Exception)
        {
            return StatusCode(500);
        }        
    }

    /// <summary>
    /// This endpoint creates a data job
    /// </summary>
    /// <returns> The created data job </returns>
    [HttpPost]
    public async Task<ActionResult<DataJobDTO>> Post([FromBody] DataJobDTO dataJob)
    {
        try
        {
            return Created("", await _dataProcessorService.Create(dataJob));
        }
        catch (Exception)
        {
            return StatusCode(500);
        }        
    }

    /// <summary>
    /// This endpoint updates an existing data job
    /// </summary>
    /// <returns> The updated data job </returns>
    [HttpPatch("{id}")]
    public async Task<ActionResult<DataJobDTO>> Patch([FromRoute] Guid id, [FromBody] DataJobDTO dataJob)
    {
        try
        {
            return Ok(await _dataProcessorService.Update(id, dataJob));
        }
        catch (Exception)
        {
            return StatusCode(500);
        }        
    }


    /// <summary>
    /// This endpoint deletes an existing data job
    /// </summary>
    /// <returns> A 200 on success </returns>
    [HttpDelete("{id}")]
    public async Task<ActionResult> Delete([FromRoute] Guid id)
    {
        try
        {
            await _dataProcessorService.Delete(id);
            return Ok();
        }
        catch (Exception)
        {
            return StatusCode(500);
        }        
    }

    /// <summary>
    /// This endpoint starts a background process
    /// </summary>
    /// <returns> true on success </returns>
    [HttpPost("{id}/start")]
    public async Task<ActionResult<bool>> Start([FromRoute] Guid id)
    {
        try
        {
            return Ok(await _dataProcessorService.StartBackgroundProcess(id));
        }
        catch (Exception)
        {
            return StatusCode(500);
        }
    }

    /// <summary>
    /// This endpoint gets the status of a background process
    /// </summary>
    /// <returns> The status of the process or 404 if not found </returns>
    [HttpGet("{id}/status")]
    public async Task<ActionResult<DataJobStatus>> GetStatus([FromRoute] Guid id)
    {
        try
        {
            var status = await _dataProcessorService.GetBackgroundProcessStatus(id);
            return status.HasValue ? Ok(status.Value) : NotFound();
        }
        catch (Exception)
        {
            return StatusCode(500);
        }
    }

    /// <summary>
    /// This endpoint returns the results of a background process
    /// </summary>
    /// <returns> The results of the process </returns>
    [HttpGet("{id}/results")]
    public async Task<ActionResult<List<string>>> GetResults([FromRoute] Guid id)
    {
        try
        {
            return Ok(await _dataProcessorService.GetBackgroundProcessResults(id));
        }
        catch (Exception)
        {
            return StatusCode(500);
        }
    }
}
