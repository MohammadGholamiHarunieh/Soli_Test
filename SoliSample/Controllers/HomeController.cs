using Microsoft.AspNetCore.Mvc;
using SoliSample.Models;
using SoliSample.Services;

namespace SoliSample.Controllers
{
    [ApiController]
    public class HomeController : ControllerBase
    {
        [Route("ask")]
        [HttpPost]
        public IActionResult Answer(AskRequest req, [FromServices] KnowledgeService service)
        {
            var answer = service.Answer(req.Question);
            return Ok(new { answer });

        }
    }
}
