using Microsoft.AspNetCore.Mvc;
using System;
using TreasureBackEnd.IServices;
using TreasureBackEnd.Model;

// For more information on enabling Web API for empty projects, visit https://go.microsoft.com/fwlink/?LinkID=397860

namespace TreasureBackEnd.Controllers
{
    [Route("api/[controller]")]
    [ApiController]
    public class TreasureController : ControllerBase
    {

        public TreasureController(ITreasureService treasureService)
        {
            _treasureService = treasureService;
        }
        private readonly ITreasureService _treasureService;


        // GET: api/<TreasureController>
        [HttpPost(Name = "Treasure")]
        public async Task<IActionResult> CalculateMinimumFuel([FromBody] TreasureInput input)
        {
            
               var result = await Task.Run(() => _treasureService.CalculateMinimumFuel(input));
               return Ok(result);         
        }


        [HttpGet("history")]
        public async Task<IActionResult> GetHistory()
        {
            var results = await _treasureService.GetHistory();
            return Ok(results);
        }
    }
}
