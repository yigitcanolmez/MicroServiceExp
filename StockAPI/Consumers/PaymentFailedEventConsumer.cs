using MassTransit;
using MessageBroker;
using Microsoft.EntityFrameworkCore;
using StockAPI.Domain;

namespace StockAPI.Consumers
{
    public class PaymentFailedEventConsumer : IConsumer<PaymentFailedEvent>
    {
        private readonly AppDbContext _appDbContext;
        private readonly ILogger<PaymentFailedEventConsumer> _logger;

        public PaymentFailedEventConsumer(AppDbContext appDbContext, ILogger<PaymentFailedEventConsumer> logger)
        {
            _appDbContext = appDbContext;
            _logger = logger;
        }

        public async Task Consume(ConsumeContext<PaymentFailedEvent> context)
        {
            foreach (var item in context.Message.OrderItems)
            {
                var stockCount = await _appDbContext.Stocks.FirstOrDefaultAsync(x => x.ProductId == item.ProductId);
                if (stockCount != null)
                {
                    stockCount.Count += item.Count;
                    await _appDbContext.SaveChangesAsync();

                }
            }
            _logger.LogInformation("Stock was released");
        }
    }
}
