using MessageBroker.Interfaces;

namespace MessageBroker.Events
{
    public class StockReserverRequestPayment : IStockReserverRequestPayment
    {
        public StockReserverRequestPayment(Guid correlationId)
        {
            CorrelationId = correlationId;
        }
        public PaymentMessage Payment { get; set; }
        public List<OrderItemMessage> OrderItemMessages { get; set; }

        public Guid CorrelationId { get; }
        public string BuyerId { get; set; }
    }
}
