using MassTransit;
using MessageBroker;
using MessageBroker.Events;
using MessageBroker.Interfaces;
using Microsoft.AspNetCore.Mvc;
using OrderAPI.Domain.DTOs;
using OrderAPI.Models;
using OrderAPI.Repository;

namespace OrderAPI.Controllers
{
    [Route("api/[controller]s")]
    [ApiController]
    public class OrderController : ControllerBase
    {
        private readonly AppDbContext _dbContext;
        //private readonly IPublishEndpoint _publishEndpoint;
        private readonly ISendEndpointProvider _sendEndpointProvider;

        public OrderController(AppDbContext dbContext, /*IPublishEndpoint publishEndpoint,*/ ISendEndpointProvider sendEndpointProvider)
        {
            _dbContext = dbContext;
            //_publishEndpoint = publishEndpoint;
            _sendEndpointProvider = sendEndpointProvider;
        }
        [HttpPost]
        public async Task<IActionResult> Create(OrderCreateDto orderCreate)
        {
            var newOrder = new Models.Order
            {
                BuyerId = orderCreate.BuyerId,
                Status = OrderStatus.Suspend,
                Address = new Address
                {
                    District = orderCreate.Address.District,
                    Line = orderCreate.Address.Line,
                    Province = orderCreate.Address.Province
                },
                CreatedDate = DateTime.Now,
                FailMessage = string.Empty
            };

            orderCreate.orderItems.ForEach(item =>
            newOrder.Items.Add(new OrderItem()
            {
                Price = item.Price,
                ProductId = item.ProductId,
                Count = item.Count


            }));
            await _dbContext.AddAsync(newOrder);

            await _dbContext.SaveChangesAsync();

            var orderCreatedRequestEvent = new OrderCreatedRequestEvent
            {
                BuyerId = orderCreate.BuyerId,
                OrderId = newOrder.Id,
                Payment = new PaymentMessage
                {
                    CardName = orderCreate.payment.CardName,
                    CardNumber = orderCreate.payment.CardNumber,
                    Expiration = orderCreate.payment.Expiration,
                    CVV = orderCreate.payment.CVV,
                    TotalPrice = orderCreate.orderItems.Sum(x => x.Price * x.Count)
                }
            };

            orderCreate.orderItems.ForEach(x =>
            {
                orderCreatedRequestEvent.OrderItems.Add(
                    new OrderItemMessage
                    {
                        Count = x.Count,
                        ProductId = x.ProductId
                    });
            });


            var sendEnpoint = await _sendEndpointProvider.GetSendEndpoint(new Uri($"queue:{RabbitMQSettingsConst.OrderSaga}"));

            await sendEnpoint.Send<IOrderCreatedRequestEvent>(orderCreatedRequestEvent);

            //await _publishEndpoint.Publish(orderCreatedEvent);


            return Ok();
        }

    }
}
