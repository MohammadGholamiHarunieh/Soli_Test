using Microsoft.AspNetCore.Mvc;
using SoliSample.Models;
using SoliSample.Services;
using System.Threading.Tasks;

namespace SoliSample.Controllers
{
    [ApiController]
    public class HomeController : ControllerBase
    {
        [Route("ask")]
        [HttpPost]
        public async Task<IActionResult> Answer(AskRequest req, [FromServices] KnowledgeService service)
        {
            var answer =await service.Answer(req.Question);
            return Ok(new { answer });

        }
    }
}
