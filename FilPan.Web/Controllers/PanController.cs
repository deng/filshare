using System.Threading.Tasks;
using Microsoft.AspNetCore.Mvc;
using FilPan.Managers;
using FilPan.Web.Requests;
using FilPan.Web.Responses;

namespace filshareapp.Controllers
{
    [ApiController]
    [Route("api/[controller]")]
    public class PanController : ApiControllerBase
    {
        private readonly FilPanManager _filPanManager;

        public PanController(FilPanManager filPanManager)
        {
            _filPanManager = filPanManager;
        }

        [HttpGet("{id}")]
        public async Task<ActionResult> Get(string id)
        {
            var data = await _filPanManager.GetUploadLog(id);
            return Result(data);
        }

        [HttpPost]
        public async Task<ActionResult> Post(PanePostRequest requst)
        {
            var data = await _filPanManager.CreateUploadLog(requst);
            return Result(data);
        }

        [HttpPost("{id}/validate")]
        public async Task<ActionResult> Validate(string id, PanValidateRequest requst)
        {
            var data = await _filPanManager.ValidateUploadLog(id, requst?.Password);
            return Result(data);
        }

        [HttpGet("{id}/download")]
        public async Task<ActionResult> Download(string id, [FromQuery] string password)
        {
            var validated = await _filPanManager.ValidateUploadLog(id, password);
            if (!validated.Success || !validated.Data)
                return NotFound();

            var info = await _filPanManager.GetFilDataInfo(id);
            if (!info.Success)
                return NotFound();

            if (!System.IO.File.Exists(info.Data.Path))
                return NotFound();

            return PhysicalFile(info.Data.Path, info.Data.MimeType, info.Data.FileName);
        }
    }
}
