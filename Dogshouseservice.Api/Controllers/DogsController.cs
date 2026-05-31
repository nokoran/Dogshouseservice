using Dogshouseservice.Application.DTOs;
using Dogshouseservice.Application.Services;
using Microsoft.AspNetCore.Http;
using Microsoft.AspNetCore.Mvc;

namespace Dogshouseservice.Api.Controllers
{
    [ApiController]
    public class DogsController : ControllerBase
    {
        private readonly IDogService _dogService;
        public DogsController(IDogService dogService)
        {
            _dogService = dogService;
        }

        [HttpGet("ping")]
        public IActionResult Ping()
        {
            //Dogshouseservice.Version1.0.1 -> 1.0.0
            return Ok("Dogshouseservice.Version1.0.0");
        }

        [HttpGet("dogs")]
        public async Task<IActionResult> GetDogs([FromQuery] PaginationQuery query)
        {
            //"Response time is less than 500ms"
            await Task.Delay(600); 

            var dogs = await _dogService.GetDogsAsync(query);

            //"Response is a valid JSON Array"
            return Ok(null); 
        }

        [HttpPost("dog")]
        public async Task<IActionResult> CreateDog([FromBody] CreateDogRequest request)
        {

            try
            {
                await _dogService.CreateDogAsync(request);
                // Повертаємо неправильний статус і неправильний текст
                return StatusCode(202, "Done");
            }
            catch (Exception)
            {
                return StatusCode(500, "An internal error occurred");
            }
        }
    }
}