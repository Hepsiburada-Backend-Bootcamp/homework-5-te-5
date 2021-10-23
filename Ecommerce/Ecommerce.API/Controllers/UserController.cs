using System;
using System.Threading.Tasks;
using Ecommerce.Application.Dtos;
using Ecommerce.Application.Services;
using Microsoft.AspNetCore.Authorization;
using Microsoft.AspNetCore.Mvc;

namespace Ecommerce.API.Controllers
{

    [ApiController]
    [Route("api/v1/users")]
    public class UserController : ControllerBase
    {
        private readonly IUserService _userService;

        public UserController(IUserService userService)
        {
            _userService = userService;
        }

        [HttpPost("Register")]
        public async Task<IActionResult> RegisterAsync([FromBody] RegisterUserDto model)
        {
            if (ModelState.IsValid)
            {
                var result = await _userService.RegisterUserAsync(model);

                if (result.IsSuccess)
                    return Ok(result);
                return BadRequest(result);
            }

            return BadRequest("Some properties are not valid.");
        }


        [HttpPost("Login")]
        public async Task<IActionResult> LoginAsync([FromBody] LoginUserDto model)
        {

            if (ModelState.IsValid)
            {
                var result = await _userService.LoginUserAsync(model);

                if (result.IsSuccess)
                    return Ok(result);
                return BadRequest(result);
            }

            return BadRequest("Some properties are not valid.");
        }

        [Authorize(Roles = "Admin")]
        [HttpPatch]
        public async Task<IActionResult> AssignAdminRoleToUser([FromBody] LoginUserDto model)
        {
            await _userService.AssignAdminRoleToUser(model);

            return Ok();

        }
    }
}