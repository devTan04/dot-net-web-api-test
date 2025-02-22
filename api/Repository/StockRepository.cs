using System;
using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;
using api.Data;
using api.Dtos.Stock;
using api.Interface;
using api.Model;
using Microsoft.EntityFrameworkCore;

namespace api.Repository;

public class StockRepository : IStockReponsitory
{
    private readonly ApplicationDBContext _context;
    public StockRepository(ApplicationDBContext context)
    {
        _context = context;
    }

    public async Task<Stock> CreateAsync(Stock stockModel)
    {
        await _context.Stocks.AddAsync(stockModel);
        await _context.SaveChangesAsync();
        return stockModel;
    }

    public async Task<Stock?> DeleteAsync(int id)
    {
        var stockModel = await _context.Stocks.FirstOrDefaultAsync(x => x.Id == id);
        if (stockModel == null)
        {
            return null;
        }
        _context.Stocks.Remove(stockModel);
        await _context.SaveChangesAsync();
        return stockModel;
    }

    public async Task<List<Stock>> GetAllAsync()
    {
        return await _context.Stocks.ToListAsync();
    }

    public async Task<Stock?> GetByIdAsync(int id)
    {
        return await _context.Stocks.FindAsync(id);
    }

    public async Task<Stock?> UpdateAsync(int id, UpdateStockRequestDto updateStockDto)
    {
        var existingStock = await _context.Stocks.FirstOrDefaultAsync(x => x.Id == id);
        if (existingStock == null)
        {
            return null;
        }

        existingStock.Symbol = updateStockDto.Symbol;
        existingStock.CompanyName = updateStockDto.CompanyName;
        existingStock.Purchase = updateStockDto.Purchase;
        existingStock.LastDiv = updateStockDto.LastDiv;
        existingStock.Industry = updateStockDto.Industry;
        existingStock.MarketCap = updateStockDto.MarketCap;
        await _context.SaveChangesAsync();
        return existingStock;
    }
}
