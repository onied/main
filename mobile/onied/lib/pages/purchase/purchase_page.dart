import 'package:flutter/material.dart';
import 'package:flutter_bloc/flutter_bloc.dart';
import 'package:go_router/go_router.dart';
import 'package:onied_mobile/app/app_theme.dart';
import 'package:flutter_credit_card/flutter_credit_card.dart';
import 'package:onied_mobile/blocs/purchase/purchase_bloc.dart';
import 'package:onied_mobile/blocs/purchase/purchase_bloc_event.dart';
import 'package:onied_mobile/blocs/purchase/purchase_bloc_state.dart';
import 'package:onied_mobile/components/button/button.dart';

class PurchasePage extends StatelessWidget {
  PurchasePage({super.key});
  final GlobalKey<FormState> formKey = GlobalKey<FormState>();

  @override
  Widget build(BuildContext context) {
    return BlocProvider(
      create: (context) => PurchaseBloc()..add(InitialLoad()),
      child: Scaffold(
        appBar: AppBar(
          title: Text(
            "Покупка",
            style: Theme.of(
              context,
            ).textTheme.titleLarge?.copyWith(color: Colors.white),
          ),
          backgroundColor: AppTheme.backgroundColorHeader,
          foregroundColor: AppTheme.accent,
        ),
        body: BlocBuilder<PurchaseBloc, PurchaseBlocState>(
          builder: (context, state) {
            return switch (state) {
              LoadingState() => const Center(
                child: CircularProgressIndicator(),
              ),
              ErrorState(:final errorMessage) => Center(
                child: Text(errorMessage),
              ),
              LoadedState(
                :final cardHolderName,
                :final cvvCode,
                :final expiryDate,
                :final cardNumber,
              ) =>
                Container(
                  padding: EdgeInsets.all(16),
                  child: SingleChildScrollView(
                    child: Column(
                      crossAxisAlignment: CrossAxisAlignment.start,
                      mainAxisAlignment: MainAxisAlignment.center,
                      spacing: 16,
                      children: [
                        Text(
                          "КОРОЛЬ ИНФОЦИГАН",
                          style: TextStyle(
                            fontSize: 24,
                            letterSpacing: 3,
                            fontWeight: FontWeight.w600,
                          ),
                        ),
                        Row(
                          mainAxisAlignment: MainAxisAlignment.spaceBetween,
                          children: [
                            Text("ПОДПИСКА", style: TextStyle(fontSize: 18)),
                            Text("10000 ₽", style: TextStyle(fontSize: 18)),
                          ],
                        ),
                        const Divider(),
                        Container(
                          padding: EdgeInsets.all(8),
                          decoration: BoxDecoration(
                            color: Colors.grey[200],
                            border: Border.all(color: Colors.grey),
                            borderRadius: BorderRadius.circular(16),
                          ),
                          child: Column(
                            children: [
                              Container(
                                padding: EdgeInsets.only(
                                  left: 16,
                                  right: 16,
                                  top: 16,
                                ),
                                child: Image.asset(
                                  'assets/icons/paymentMethods.png',
                                  width: double.infinity,
                                ),
                              ),
                              CreditCardForm(
                                formKey: formKey,
                                obscureCvv: true,
                                obscureNumber: false,
                                cardNumber: cardNumber,
                                cvvCode: cvvCode,
                                isHolderNameVisible: true,
                                isCardNumberVisible: true,
                                isExpiryDateVisible: true,
                                cardHolderName: cardHolderName,
                                expiryDate: expiryDate,
                                inputConfiguration: const InputConfiguration(
                                  cardNumberDecoration: InputDecoration(
                                    hintText: 'Номер карты',
                                    border: OutlineInputBorder(),
                                  ),
                                  expiryDateDecoration: InputDecoration(
                                    hintText: 'мм/гг',
                                    border: OutlineInputBorder(),
                                  ),
                                  cvvCodeDecoration: InputDecoration(
                                    hintText: 'CVC/CVV/CVP',
                                    border: OutlineInputBorder(),
                                  ),
                                  cardHolderDecoration: InputDecoration(
                                    hintText: 'Держатель карты',
                                    border: OutlineInputBorder(),
                                  ),
                                ),
                                onCreditCardModelChange: (model) {
                                  context.read<PurchaseBloc>().add(
                                    ChangeState(
                                      cardNumber: model.cardNumber,
                                      cvvCode: model.cvvCode,
                                      cardHolderName: model.cardHolderName,
                                      expiryDate: model.expiryDate,
                                    ),
                                  );
                                },
                              ),
                            ],
                          ),
                        ),
                        Container(
                          padding: EdgeInsets.all(16),
                          child: Row(
                            spacing: 16,
                            children: [
                              Expanded(
                                child: Button(
                                  text: "оплатить",
                                  onPressed: null,
                                ),
                              ),
                              Expanded(
                                child: Button(
                                  text: "отмена",
                                  onPressed: () {
                                    context.pop();
                                  },
                                ),
                              ),
                            ],
                          ),
                        ),
                      ],
                    ),
                  ),
                ),
              _ => const Center(child: Text("Something went wrong.")),
            };
          },
        ),
      ),
    );
  }
}
