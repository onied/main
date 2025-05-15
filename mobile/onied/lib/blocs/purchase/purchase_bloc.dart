import 'package:flutter_bloc/flutter_bloc.dart';

import 'purchase_bloc_event.dart';
import 'purchase_bloc_state.dart';

class PurchaseBloc extends Bloc<PurchaseBlocEvent, PurchaseBlocState> {
  PurchaseBloc() : super(LoadingState()) {
    on<InitialLoad>(_onInitialLoad);
    on<ChangeState>(_onChangeState);
  }

  Future<void> _onInitialLoad(
    InitialLoad event,
    Emitter<PurchaseBlocState> emit,
  ) async {
    emit(
      LoadedState(
        cardNumber: '',
        expiryDate: '',
        cardHolderName: '',
        cvvCode: '',
      ),
    );
  }

  Future<void> _onChangeState(
    ChangeState event,
    Emitter<PurchaseBlocState> emit,
  ) async {
    if (state is! LoadedState) return;
    final currentState = state as LoadedState;
    emit(
      currentState.copyWith(
        cardNumber: event.cardNumber,
        expiryDate: event.expiryDate,
        cardHolderName: event.cardHolderName,
        cvvCode: event.cvvCode,
      ),
    );
  }
}
