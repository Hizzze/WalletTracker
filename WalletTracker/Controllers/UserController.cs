using AutoMapper;
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
    private readonly IMapper _mapper;
    public UserController(IUserService service, IMapper mapper)
    {
        _userService = service;
        _mapper = mapper;
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
    

    [HttpGet("all")]
    public async Task<ActionResult<List<UserDto>>> GetAllUsers()
    {
        var allUsers = await _userService.GetAllUsersAsync();

        if (allUsers == null || !allUsers.Any())
        {
            return NotFound("No users found");
        }
        var usersDto = _mapper.Map<List<UserDto>>(allUsers);
        return Ok(usersDto);
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