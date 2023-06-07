using MassTransit;
using MessageBroker;
using OrderAPI.Repository;

namespace OrderAPI.Consumer
{
    public class PaymentCompletedEventConsumer : IConsumer<PaymentSuccessedEvent>
    {
        private readonly AppDbContext _dbContext;
        private readonly ILogger<PaymentCompletedEventConsumer> _logger;

        public PaymentCompletedEventConsumer(AppDbContext dbContext, ILogger<PaymentCompletedEventConsumer> logger)
        {
            _dbContext = dbContext;
            _logger = logger;
        }

        public async Task Consume(ConsumeContext<PaymentSuccessedEvent> context)
        {
            var getOrder = await _dbContext.Orders.FindAsync(context.Message.OrderId);
            if (getOrder != null)
            {
                getOrder.Status = Models.OrderStatus.Success;
                await _dbContext.SaveChangesAsync();

                _logger.LogInformation($"Order status changed to success");
            }
            else
            {
                _logger.LogError($"Order status changed to failed");
            }

        }
    }
}
