using AutoMapper;
using MapperApp.Models;
using MapperApp.Models.DTOs.Incoming;
using MapperApp.Models.DTOs.Outgoing;
using Microsoft.AspNetCore.Mvc;

namespace MapperApp.Controllers
{
    [ApiController]
    [Route("[controller]")]
    public class DriverController : ControllerBase
    {


        private readonly ILogger<DriverController> _logger;
        private static List<Driver> drivers = new List<Driver>();
        private readonly IMapper _mapper;
        
        public DriverController(ILogger<DriverController> logger, IMapper mapper)
        {
            _logger = logger;
            _mapper = mapper;
        }
        //Get all drivers
        [HttpGet]
        //x => x.Status = 1 : sauf les driver actif
        public IActionResult GetDrivers()
        {
            var allDrivers = drivers.Where(x => x.Status == 1).ToList();
            var _drivers = _mapper.Map<IEnumerable<DriverDto>>(allDrivers);
            
            return Ok(_drivers);
        }

        [HttpPost]
        public IActionResult CreateDriver(DriverForCreationDto data)
        {
            if(ModelState.IsValid)
            {
                var _driver = _mapper.Map<Driver>(data);
                 
                drivers.Add(_driver);
                var newDriver = _mapper.Map<DriverDto>(_driver);
                return CreatedAtAction("GetDriver",routeValues:new {_driver.Id}, value: newDriver);
            }

            return new JsonResult("Somthing wont wrong") { StatusCode = 500 };
        }

        [HttpGet("{id}")]
        public IActionResult  GetDriver(Guid id)
        {
            var item  = drivers.FirstOrDefault(x => x.Id == id);

            if (item == null)
                return NotFound();


            return Ok(item);

        }

        [HttpPut("{id}")]
        public IActionResult UpdateDriver(Guid id, Driver data)
        {
            if (id == data.Id)
                return BadRequest();

            var existingDriver = drivers.FirstOrDefault(x => x.Id == data.Id);

            if (existingDriver == null)
                return NotFound();


            existingDriver.DriverNumber = data.DriverNumber;
            existingDriver.FirstName = data.FirstName;
            existingDriver.LastName = data.LastName;
            existingDriver.WorldChampionships = data.WorldChampionships;

            return NoContent();
        }


        [HttpDelete("{id}")]
        public IActionResult DeleteDriver(Guid id)
        {
            var existingDriver = drivers.FirstOrDefault(x => x.Id == id);

            if (existingDriver == null)
                return NotFound();

            existingDriver.Status = 0;
            return NoContent();


        }
    }

}