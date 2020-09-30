using System.Threading.Tasks;
using Microsoft.AspNetCore.Mvc;
using FilPan.Managers;

namespace filshareapp.Controllers
{
    [ApiController]
    [Route("api/[controller]")]
    public class RewardController : ApiControllerBase
    {
        private readonly FilPanManager _filPanManager;

        public RewardController(FilPanManager filPanManager)
        {
            _filPanManager = filPanManager;
        }

        [HttpGet("alipay/{id}")]
        public ActionResult Alipay(string id)
        {
            var data = _filPanManager.GetRewardPath(id);
            return PhysicalFile(data, "image/png");
        }

        [HttpGet("wxpay/{id}")]
        public ActionResult Wxpay(string id)
        {
            
            var data = _filPanManager.GetRewardPath(id);
            return PhysicalFile(data, "image/png");
        }
    }
}
