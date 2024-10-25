using MassTransit.Data.Messages;
using Support.Data.Models;

namespace Support.Abstractions;

public interface IClientNotificationProducer
{
    public Task NotifyClientOfNewMessage(Message message);
}
