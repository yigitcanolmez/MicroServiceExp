using MassTransit;
using MessageBroker;

namespace PaymentAPI.Consumers
{
    public class StockReservedEventConsumer : IConsumer<StockReservedEvent>
    {
        private readonly ILogger<StockNotReservedEvent> _logger;
        private readonly IPublishEndpoint _publishEndpoint;

        public StockReservedEventConsumer(ILogger<StockNotReservedEvent> logger, IPublishEndpoint publishEndpoint )
        {
            _logger = logger;
            _publishEndpoint = publishEndpoint;
        }

        public async Task Consume(ConsumeContext<StockReservedEvent> context)
        {
            var balance = 3000;
            if (balance > context.Message.Payment.TotalPrice)
            {
                _logger.LogInformation($"{context.Message.Payment.TotalPrice} witdrawn for {context.Message.BuyerId}");

                await _publishEndpoint.Publish(new PaymentSuccessedEvent
                {
                    BuyerId = context.Message.BuyerId,
                    OrderId = context.Message.OrderId
                });

            }
            else
            {
                _logger.LogInformation($"Belirtilen tutar çekilemedi.");
                await _publishEndpoint.Publish(new PaymentFailedEvent
                {
                    BuyerId = context.Message.BuyerId,
                    OrderId = context.Message.OrderId,
                    Message = "Bilinmeyen magic string hatası"
                });

            }

        }
    }
}
