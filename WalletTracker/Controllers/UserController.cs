using Microsoft.AspNetCore.Mvc;
using WalletTracker.Repositories;

namespace WalletTracker.Controllers;


[ApiController]
[Route("api/users")]
public class UserController : ControllerBase
{
    private readonly UserRepository _userRepository;
    
    public UserController(UserRepository userRepository)
    {
        _userRepository = userRepository;
    }
    
}