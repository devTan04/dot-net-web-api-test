using System;
using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;
using api.Data;
using api.Dtos.Stock;
using api.Interface;
using api.Mappers;
using api.Model;
using Microsoft.AspNetCore.Mvc;
using Microsoft.EntityFrameworkCore;

namespace api.Controllers;

[Route("api/stock")]
[ApiController]
public class StockController : ControllerBase
{
    private readonly ApplicationDBContext _context;
    private readonly IStockReponsitory _stockRepo;

    public StockController(ApplicationDBContext context, IStockReponsitory stockRepo)
    {
        _stockRepo = stockRepo;
        _context = context;
    }
    [HttpGet]
    public async Task<IActionResult> GetAll()
    {
        var stock = await _stockRepo.GetAllAsync();

        var stockDto = stock.Select(s => s.ToStockDto());

        return Ok(stockDto);
    }
    [HttpGet("{id}")]
    public async Task<IActionResult> GetById([FromRoute] int id)
    {
        var stock = await _stockRepo.GetByIdAsync(id);
        if (stock == null)
        {
            return NotFound();
        }
        return Ok(stock.ToStockDto());
    }
    [HttpPost]
    public async Task<IActionResult> Create([FromBody] CreateStockRequestDto stockDto)
    {
        var stockModel = stockDto.ToStockFromCreateDTO();
        await _stockRepo.CreateAsync(stockModel);
        return CreatedAtAction(nameof(GetById), new { id = stockModel.Id }, stockModel.ToStockDto());
    }
    [HttpPut]
    [Route("{id}")]
    public async Task<IActionResult> Update([FromRoute] int id, [FromBody] UpdateStockRequestDto updateStockDto)
    {
        var stockModel = await _stockRepo.UpdateAsync(id, updateStockDto);
        if (stockModel == null)
        {
            return NotFound();
        }

        await _context.SaveChangesAsync();
        return Ok(stockModel.ToStockDto());
    }
    [HttpDelete]
    [Route("{id}")]
    public async Task<IActionResult> Delete([FromRoute] int id)
    {
        var stockModel = await _stockRepo.DeleteAsync(id);
        if (stockModel == null)
        {
            return NotFound();
        }

        return NoContent();
    }

}
