using Microsoft.AspNetCore.Http;
using Microsoft.AspNetCore.Mvc;
using Microsoft.EntityFrameworkCore;
using StockAPI.Domain;
using System.Collections.Generic;

namespace StockAPI.Controllers
{
    [Route("api/[controller]s")]
    [ApiController]
    public class StockController : ControllerBase
    {
        private readonly AppDbContext _dbContext;
        private readonly ILogger<Domain.Stock> _logger;

        public StockController(AppDbContext dbContext, ILogger<Domain.Stock> logger)
        {
            _dbContext = dbContext;
            _logger = logger;
        }

        [HttpGet]
        public async Task<IActionResult> Get()
        {
            return Ok(await _dbContext.Stocks.ToListAsync());

        }
    }
}
