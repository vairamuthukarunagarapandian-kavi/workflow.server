using Microsoft.AspNetCore.Mvc;
using PiiSignalRDemo.Models;
using PiiSignalRDemo.Queue;

namespace PiiSignalRDemo.Controllers;

[ApiController]
[Route("api/pii")]
public class PiiController : ControllerBase
{
    private readonly IQueueService _queue;

    public PiiController(IQueueService queue)
    {
        _queue = queue;
    }

    [HttpPost]
    public async Task<IActionResult> Submit(PiiRequest request)
    {

        await _queue.EnqueueAsync(request);

        return Ok(new
        {
            message = "Request queued",
            requestId = request.Id
        });
    }
}