using MassTransit;
using MessageBroker;
using OrderAPI.Repository;

namespace OrderAPI.Consumer
{
    public class StockNotReservedEventConsumer : IConsumer<StockNotReservedEvent>
    {
        private readonly AppDbContext _appDbContext;
        private readonly ILogger<StockNotReservedEventConsumer> _logger;

        public StockNotReservedEventConsumer(AppDbContext appDbContext, ILogger<StockNotReservedEventConsumer> logger)
        {
            _appDbContext = appDbContext;
            _logger = logger;
        }

        public async Task Consume(ConsumeContext<StockNotReservedEvent> context)
        {
            var order = await _appDbContext.Orders.FindAsync(context.Message.OrderId);
            if (order == null)
            {
                order.Status = Models.OrderStatus.Fail;
                order.FailMessage = context.Message.Message;
                await _appDbContext.SaveChangesAsync();

                _logger.LogInformation("Order stock not reserved so status failed");

            }
            else
            {
                _logger.LogError("Order not found");
            }
        }
    }
}
