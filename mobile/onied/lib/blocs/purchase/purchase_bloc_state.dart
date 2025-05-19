abstract class PurchaseBlocState {}

class LoadingState extends PurchaseBlocState {}

class ErrorState extends PurchaseBlocState {
  String errorMessage;

  ErrorState({required this.errorMessage});
}

class LoadedState extends PurchaseBlocState {
  String cardNumber = '';
  String expiryDate = '';
  String cardHolderName = '';
  String cvvCode = '';

  LoadedState({
    required this.cardNumber,
    required this.expiryDate,
    required this.cardHolderName,
    required this.cvvCode,
  });

  LoadedState copyWith({
    String? cardNumber,
    String? expiryDate,
    String? cardHolderName,
    String? cvvCode,
  }) {
    return LoadedState(
      cardNumber: cardNumber ?? this.cardNumber,
      expiryDate: expiryDate ?? this.expiryDate,
      cardHolderName: cardHolderName ?? this.cardHolderName,
      cvvCode: cvvCode ?? this.cvvCode,
    );
  }
}
