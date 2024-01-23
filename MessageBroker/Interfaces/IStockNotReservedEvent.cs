using MassTransit;

namespace MessageBroker.Interfaces
{
    public interface IStockNotReservedEvent : CorrelatedBy<Guid>
    {
        string Message { get; set; }
    }
}
