using Microsoft.AspNetCore.Mvc;
using WalletTracker.Abstractions;
using WalletTracker.Dtos;
using WalletTracker.Models;

namespace WalletTracker.Controllers;


[ApiController]
[Route("api/users")]
public class UserController : ControllerBase
{
    private readonly IUserService _userService;
    
    public UserController(IUserService service)
    {
        _userService = service;
    }


    [HttpGet("{id:guid}")]
    public async Task<ActionResult<User>> GetUserById(Guid id)
    {
        var userId = await _userService.GetUserByIdAsync(id);

        if (userId == null)
        {
            return NotFound("User not found");
        }
        
        return await _userService.GetUserByIdAsync(id);
    }

    [HttpGet("{email}")]
    public async Task<ActionResult<User>> GetUserByEmail(string email)
    {
        return Ok(await _userService.GetUserByEmailAsync(email));
    }
    
    [HttpPost]
    public async Task<ActionResult<User>> CreateUser(CreateUserDto userDto)
    {
        var user = await _userService.CreateUserAsync(userDto);

        return Ok(user);
    }

    [HttpPut]
    public async Task<ActionResult<User>> UpdateUser(Guid id, UpdateUserDto userDto)
    {
        var user = await _userService.UpdateUser(id, userDto);
        return Ok(user);
    }

    [HttpDelete("{id:guid}")]
    public async Task<ActionResult<User>> DeleteUser(Guid id)
    {
         return Ok(await _userService.DeleteUserAsync(id));
    }
}