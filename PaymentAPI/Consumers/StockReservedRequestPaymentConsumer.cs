using MassTransit;
using MessageBroker;
using MessageBroker.Interfaces;

namespace PaymentAPI.Consumers
{
    public class StockReservedRequestPaymentConsumer : IConsumer<IStockReserverRequestPayment>
    {
        private readonly ILogger<StockReservedRequestPaymentConsumer> _logger;
        private readonly IPublishEndpoint _publishEndpoint;

        public StockReservedRequestPaymentConsumer(IPublishEndpoint publishEndpoint, ILogger<StockReservedRequestPaymentConsumer> logger)
        {
            _publishEndpoint = publishEndpoint;
            _logger = logger;
        }

        public async Task Consume(ConsumeContext<IStockReserverRequestPayment> context)
        {
            var balance = 3000m;

            if (balance > context.Message.Payment.TotalPrice)
            {
                _logger.LogInformation("context.Message.Payment.TotalPrice");

                await _publishEndpoint.Publish(new PaymentSuccessedEvent(context.Message.CorrelationId));
            }
            else
            {
                _logger.LogInformation("context.Message.Payment.TotalPrice");

                await _publishEndpoint.Publish(new PaymentFailedEvent(context.Message.CorrelationId)
                {
                    OrderItems = context.Message.OrderItemMessages,
                    Reason = "not enough balance"
                });
            }
        }
    }
}
