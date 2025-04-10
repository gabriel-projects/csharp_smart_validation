using Api.GRRInnovations.SmartValidation.Domain.Models;
using Api.GRRInnovations.SmartValidation.Filters;
using Api.GRRInnovations.SmartValidation.Services;
using Microsoft.AspNetCore.Mvc;

namespace Api.GRRInnovations.SmartValidation.Controllers
{
    [ApiController]
    [Route("api/[controller]")]
    public class UsersController : ControllerBase
    {
        private readonly ValidationAttributesService _validationService;

        public UsersController(ValidationAttributesService validationService)
        {
            _validationService = validationService;
        }

        [HttpPost("CreateUser")]
        public IActionResult CreateUser(User user)
        {
            var validationErrors = _validationService.Validate(user);
            if (validationErrors.Any())
            {
                return BadRequest(new { Error = validationErrors });
            }

            return Ok(new { Message = "User created successfully!" });
        }

        [ServiceFilter(typeof(ValidationFilter))]
        [HttpPost("ValidateUser")]
        public IActionResult ValidateUser([FromBody] User user)
        {
            return Ok(new { Message = "User created successfully!" });
        }

        [HttpPost("TestExeceptionHandler")]
        public IActionResult TestExeceptionHandler(User user)
        {
            user = null;

            if (user.UserName != null)
            {

            }

            return Ok(new { Message = "User created successfully!" });
        }
    }
}
