using Microsoft.AspNetCore.Mvc;
using UpdateService.Models;
using UpdateService.Services;

namespace UpdateService.Controllers
{
    [ApiController]
    [Route("[controller]")]
    public class VersionController : ControllerBase
    {
        private readonly ILogger<VersionController> _logger;
        private readonly IApplicationVersionHandler _applicationVersionHandler;

        public VersionController(ILogger<VersionController> logger, IApplicationVersionHandler applicationVersionHandler)
        {
            _logger = logger;
            _applicationVersionHandler = applicationVersionHandler;
        }

        [HttpGet(Name = "")]
        [ProducesResponseType(typeof(Version), StatusCodes.Status200OK)]
        [ProducesResponseType(StatusCodes.Status404NotFound)]
        [ProducesResponseType(typeof(BadRequestObjectResult), 400)]
        public async Task<ActionResult<VersionModel>> Get([FromQuery]string applicationName)
        {
            var version = await _applicationVersionHandler
                .GetCurrentVersion(applicationName)
                .ConfigureAwait(false);

            if (string.IsNullOrEmpty(version))
            {
                return NotFound();
            }

            return Ok(new VersionModel
            {
                ApplicationName = applicationName,
                Version = version
            });
        }
        

        [HttpGet()]
        [Route("download")]
        [ProducesResponseType(typeof(byte[]), StatusCodes.Status200OK)]
        [ProducesResponseType(StatusCodes.Status404NotFound)]
        [ProducesResponseType(typeof(BadRequestObjectResult), 400)]
        public async Task<ActionResult> Get([FromQuery] string applicationName, [FromQuery] string version)
        {
            var stream = await _applicationVersionHandler
                .DownloadVersion(applicationName, version)
                .ConfigureAwait(false);

            if (stream == null)
            {
                return NotFound();
            }           

            return File(stream, "application/octet-stream", $"{applicationName}.exe");
        } 
    }
}
