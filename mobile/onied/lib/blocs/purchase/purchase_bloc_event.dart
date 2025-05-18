abstract class PurchaseBlocEvent {}

class InitialLoad extends PurchaseBlocEvent {}

class ChangeState extends PurchaseBlocEvent {
  String? cardNumber;
  String? expiryDate;
  String? cardHolderName;
  String? cvvCode;

  ChangeState({
    this.cardNumber,
    this.expiryDate,
    this.cardHolderName,
    this.cvvCode,
  });
}
