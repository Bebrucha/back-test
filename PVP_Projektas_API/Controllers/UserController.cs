﻿using Microsoft.AspNetCore.Mvc;
using PVP_Projektas_API.Interfaces;
using PVP_Projektas_API.Models;

namespace PVP_Projektas_API.Controllers
{
    [Route("[controller]")]
    [ApiController]
    public class UserController : ControllerBase
    {
        private readonly IUserRepository _userRepository;
        private readonly IShelfRepository _shelfRepository;
        public UserController(IUserRepository userRepository, IShelfRepository shelfRepository)
        {
            _userRepository = userRepository;
            _shelfRepository = shelfRepository; 
        }

        [HttpGet("{email}")]
        public async Task<ActionResult> GetUser([FromRoute] string email)
        {
           var user = await _userRepository.GetUser(email);
            
            if (user == null)
            {
                return NotFound();
            }
            return Ok(user);
        }

        [HttpGet("login/{email}/{password}")]
        public async Task<ActionResult> LoginUser([FromRoute] string email, [FromRoute] string password)
        {
            var user = await _userRepository.GetUser(email);

            if (user == null)
            {
                return NotFound();
            }

            if (password is not null && password == user.Password)
            {

                return Ok(user);
            }
            return BadRequest();
        }
        [HttpPost("register")]
        public async Task<ActionResult> RegisterUser([FromBody] UserDto userDto)
        {
            if (await _userRepository.GetUser(userDto.Email) is null)
            {
                var shelf = await _shelfRepository.CreateDefaultShelf();

                var user = await _userRepository.CreateUser(userDto, shelf);
                if (user == null)
                {
                    return NotFound();
                }
                return Ok(user);
            }
            else return BadRequest();
        }

    }
}
