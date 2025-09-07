using Microsoft.AspNetCore.Authorization;
using Microsoft.AspNetCore.Mvc;
using WalletTracker.Abstractions;
using WalletTracker.Dtos;

namespace WalletTracker.Controllers;

[ApiController]
[Route("api/transactions")]
[Authorize]
public class TransactionController : ControllerBase
{
    private readonly ITransactionService _transactionService;
    
    public TransactionController(ITransactionService transactionService) 
    {
        _transactionService = transactionService;
    }


    [HttpGet("{id:guid}")]
    public async Task<ActionResult> GetTransactionById(Guid id)
    {
        return Ok(await _transactionService.GetTransactionById(id));
    }

    [HttpGet("wallets/{walletId:guid}")]
    public async Task<ActionResult> GetTransactionsByWalletId(Guid walletId)
    {
        return Ok( await _transactionService.GetTransactionsByWalletId(walletId));
    }

    [HttpPost]
    public async Task<ActionResult> CreateTransaction([FromBody]TransactionCreateDto createDto)
    {
        await _transactionService.CreateTransaction(createDto);
        return Ok();
    }

    [HttpPut]
    public async Task<ActionResult> UpdateTransaction([FromBody] TransactionUpdateDto updateDto)
    {
        await _transactionService.UpdateAsync(updateDto);
        return Ok();
    }

    [HttpDelete]
    public async Task<ActionResult> DeleteTransaction(Guid id)
    {
        await _transactionService.DeleteAsync(id);
        return Ok();
    }
}