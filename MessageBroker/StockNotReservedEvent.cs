using MessageBroker.Interfaces;

namespace MessageBroker
{
    public class StockNotReservedEvent : IStockNotReservedEvent
    {

        public StockNotReservedEvent(Guid correlationId)
        {
            CorrelationId = correlationId;
        }
        public string Message { get; set; }

        public Guid CorrelationId { get; }
    }
}
