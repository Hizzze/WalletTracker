using System.IdentityModel.Tokens.Jwt;
using System.Security.Claims;
using AutoMapper;
using Microsoft.AspNetCore.Authorization;
using Microsoft.AspNetCore.Mvc;
using WalletTracker.Abstractions;
using WalletTracker.Dtos;
using WalletTracker.Exceptions;
using WalletTracker.Models;
using ValidationException = System.ComponentModel.DataAnnotations.ValidationException;

namespace WalletTracker.Controllers;

[ApiController]
[Route("api/wallets")]
[Authorize]
public class WalletController : ControllerBase
{
    private readonly IWalletService _walletService;
    private readonly ILogger<WalletController> _logger;
    private readonly IMapper _mapper;

    public WalletController(IWalletService walletService, ILogger<WalletController> logger, IMapper mapper)
    {
        _walletService = walletService;
        _logger = logger;
        _mapper = mapper;
    }

    [HttpGet("{id}")]
    public async Task<ActionResult> GetWallet(Guid id)
    {
        try
        {
            var userId = GetCurrentUserId();
            var wallet = _mapper.Map<WalletDto>(await _walletService.GetWalletById(id, userId));
            return Ok(wallet);
        }
        catch (Exception ex)
        {
            _logger.LogError(ex, "Error getting wallet {WalletId} for user {UserId}", id, GetCurrentUserId());
            return NotFound(ex.Message);
        }
    }

    [HttpGet]
    public async Task<ActionResult> GetUserWallets()
    {
        var userId = GetCurrentUserId();
        var wallets = await _walletService.GetUserWalletsAsync(userId);
        return Ok(wallets);
    }

    [HttpPost]
    public async Task<ActionResult> CreateWallet([FromBody] WalletDto walletCreateDto)
    {
        try
        {
            var userId = GetCurrentUserId();
            var wallet = _mapper.Map<Wallet>(walletCreateDto);
            var createdWallet = await _walletService.CreateWalletAsync(wallet);
            return CreatedAtAction(nameof(GetWallet), new { id = createdWallet.Id }, createdWallet);
        }
        catch (ValidationException ex)
        {
            _logger.LogWarning(ex, "Validation failed");
            return BadRequest(ex.Message);
        }
        catch (BusinessRuleException ex)
        {
            _logger.LogWarning(ex, "Business rule failed");
            return Conflict(ex.Message);
        }
    }

    [HttpPut("{id}")]
    public async Task<ActionResult> UpdateWallet(Guid id, [FromBody] WalletDto walletUpdateDto)
    {
        try
        {
            var userId = GetCurrentUserId();
            var wallet = new Wallet
            {
                Id = id,
                Name = walletUpdateDto.Name,
                Balance = walletUpdateDto.Balance,
                Currency = walletUpdateDto.Currency,
            };
            await _walletService.UpdateWalletAsync(wallet, userId);
            return NoContent();
        }
        catch (NotFoundException ex)
        {
            return NotFound(ex.Message);
        }
        catch (ValidationException ex)
        {
            return BadRequest(ex.Message);
        }
        
    }

    [HttpDelete("{id}")]
    public async Task<ActionResult> DeleteWallet(Guid id)
    {
        try
        {
            var userId = GetCurrentUserId();
            await _walletService.DeleteWalletAsync(id, userId);
            return NoContent();
        }
        catch (NotFoundException ex)
        {
            return NotFound(ex.Message);
        }
        catch (BusinessRuleException ex)
        {
            return Conflict(ex.Message);
        }
    }

    private Guid GetCurrentUserId()
    {
        // 1. Получаем токен из куки
        var token = Request.Cookies["auth-token"];
    
        if (string.IsNullOrEmpty(token))
        {
            throw new UnauthorizedAccessException("Authentication token not found");
        }

        // 2. Создаем обработчик токенов
        var tokenHandler = new JwtSecurityTokenHandler();
    
        try
        {
            // 3. Читаем токен без валидации (так как валидация уже выполнена middleware)
            var jwtToken = tokenHandler.ReadJwtToken(token);
        
            // 4. Извлекаем идентификатор пользователя
            var userIdClaim = jwtToken.Claims.FirstOrDefault(c => c.Type == "userId") 
                              ?? jwtToken.Claims.FirstOrDefault(c => c.Type == ClaimTypes.NameIdentifier);
        
            if (userIdClaim == null || !Guid.TryParse(userIdClaim.Value, out var userId))
            {
                throw new UnauthorizedAccessException("Invalid user identifier in token");
            }
        
            return userId;
        }
        catch (Exception ex)
        {
            _logger.LogError(ex, "Error parsing JWT token");
            throw new UnauthorizedAccessException("Invalid authentication token");
        }
    }
}