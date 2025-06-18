using AutoMapper;
using Dao.Services;
using Microsoft.AspNetCore.Authorization;
using Microsoft.AspNetCore.Http;
using Microsoft.AspNetCore.Mvc;
using RestaurantReservationSystemWebApi.Controllers.DTOs;

namespace RestaurantReservationSystemWebApi.Controllers
{
    //[Authorize(Roles = "Admin")]
    [Route("api/[controller]")]
    [ApiController]
    public class LogController : ControllerBase
    {
        private readonly LogService _logService;
        private readonly IMapper _mapper;

        public LogController(LogService logService, IMapper mapper)
        {
            _logService = logService;
            _mapper = mapper;
        }

        [HttpGet]
        public IActionResult GetAll([FromQuery] int count = 50)
        {
            var logs = _logService.GetLastN(count);
            var result = _mapper.Map<IEnumerable<LogResponseDTO>>(logs);
            return Ok(result);
        }

        [HttpGet("count")]
        public IActionResult Count()
        {
            return Ok(_logService.Count());
        }

        [HttpPost]
        public IActionResult CreateLog([FromBody] LogCreateDTO dto)
        {
            if (!ModelState.IsValid)
                return BadRequest(ModelState);

            var log = _logService.Log(dto.Message, dto.Level);
            var result = _mapper.Map<LogResponseDTO>(log);
            return Ok(result);
        }
    }
}

