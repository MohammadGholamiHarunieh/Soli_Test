using Microsoft.AspNetCore.Mvc;
using SoliSample.Models;
using SoliSample.Services;
using SoliSample.Services.Interfaces;
using System.Threading.Tasks;

namespace SoliSample.Controllers
{
    [ApiController]
    public class HomeController : ControllerBase
    {
        private readonly IKnowledgeService _service;

        public HomeController(IKnowledgeService service)
        {
            _service = service;
        }

        [Route("ask")]
        [HttpPost]
        public async Task<IActionResult> Answer(
            AskRequest req)
        {
            var answer =
                await _service.AnswerAsync(
                    req.Question);

            return Ok(new { answer });
        }
    }
}
