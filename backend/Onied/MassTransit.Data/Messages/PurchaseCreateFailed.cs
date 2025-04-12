namespace MassTransit.Data.Messages;

public record PurchaseCreateFailed(int Id, string Token, string ErrorMessage);
