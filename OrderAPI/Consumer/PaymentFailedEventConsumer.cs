﻿using MassTransit;
using MessageBroker;
using OrderAPI.Repository;

namespace OrderAPI.Consumer
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
            var order = await _appDbContext.Orders.FindAsync(context.Message.OrderId);

            if (order == null)
            {
                order.Status = Models.OrderStatus.Fail;
                order.FailMessage = context.Message.Message;
                await _appDbContext.SaveChangesAsync();

                _logger.LogInformation("Order incompleted because failed.");
            }
            else
            {
                _logger.LogError("Order not found");
            }

        }
    }
}
