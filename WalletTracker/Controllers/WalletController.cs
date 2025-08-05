using Microsoft.AspNetCore.Mvc;
using WalletTracker.Abstractions;
using WalletTracker.Dtos;
using WalletTracker.Exceptions;
using WalletTracker.Models;
using ValidationException = System.ComponentModel.DataAnnotations.ValidationException;

namespace WalletTracker.Controllers;

[ApiController]
[Route("api/wallets")]
public class WalletController : ControllerBase
{
    private readonly IWalletService _walletService;
    private readonly ILogger<WalletController> _logger;

    public WalletController(IWalletService walletService, ILogger<WalletController> logger)
    {
        _walletService = walletService;
        _logger = logger;
    }

    [HttpGet("{id}")]
    public async Task<ActionResult> GetWallet(Guid id)
    {
        try
        {
            var userId = GetCurrentUserId();
            var wallet = await _walletService.GetWalletById(id, userId);
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
            var wallet = new Wallet
            {
                Name = walletCreateDto.Name,
                Balance = walletCreateDto.Balance,
                Currency = walletCreateDto.Currency,
                UserId = userId
            };
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
        return Guid.Parse(User.FindFirst("sub")?.Value!);
    }
}