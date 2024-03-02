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

    [HttpGet]
    public async Task<ActionResult<IEnumerable<DataJobDTO>>> Get()
    {
        return Ok(await _dataProcessorService.GetAllDataJobs());
    }

    [HttpGet("status/{status}")]
    public async Task<ActionResult<IEnumerable<DataJobDTO>>> GetByStatus([FromRoute] DataJobStatus status)
    {
        return Ok(await _dataProcessorService.GetDataJobsByStatus(status));
    }

    [HttpGet("{id}")]
    public async Task<ActionResult<DataJobDTO>> Get([FromRoute] Guid id)
    {
        return Ok(await _dataProcessorService.GetDataJob(id));
    }

    [HttpPost]
    public async Task<ActionResult<DataJobDTO>> Post([FromBody] DataJobDTO dataJob)
    {
        return Created("", await _dataProcessorService.Create(dataJob));
    }

    [HttpPatch("{id}")]
    public async Task<ActionResult<DataJobDTO>> Patch([FromRoute] Guid id, [FromBody] DataJobDTO dataJob)
    {
        return Ok(await _dataProcessorService.Update(id, dataJob));
    }


    [HttpDelete("{id}")]
    public async Task<ActionResult> Delete([FromRoute] Guid id){
        await _dataProcessorService.Delete(id);
        return Ok();
    }
    [HttpPost("{id}/start")]
    public async Task<ActionResult<bool>> Start([FromRoute] Guid id)
    {
        return Ok(await _dataProcessorService.StartBackgroundProcess(id));
    }

    [HttpGet("{id}/status")]
    public async Task<ActionResult<DataJobStatus>> GetStatus([FromRoute] Guid id)
    {
        return Ok(await _dataProcessorService.GetBackgroundProcessStatus(id));
    }

    [HttpGet("{id}/results")]
    public async Task<ActionResult<List<string>>> GetResults([FromRoute] Guid id)
    {
        return Ok(await _dataProcessorService.GetBackgroundProcessResults(id));
    }
}
