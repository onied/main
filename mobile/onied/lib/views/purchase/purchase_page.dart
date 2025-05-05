import 'package:flutter/material.dart';
import 'package:onied_mobile/app/app_theme.dart';
import 'package:flutter_credit_card/flutter_credit_card.dart';
import 'package:onied_mobile/components/button/button.dart';

class PurchasePage extends StatefulWidget {
  const PurchasePage({super.key});

  @override
  State<StatefulWidget> createState() => PurchasePageState();
}

class PurchasePageState extends State<PurchasePage> {
  final GlobalKey<FormState> formKey = GlobalKey<FormState>();
  String cardNumber = '';
  String expiryDate = '';
  String cardHolderName = '';
  String cvvCode = '';

  @override
  Widget build(BuildContext context) {
    return Scaffold(
      appBar: AppBar(
        title: Text(
          "Покупка",
          style: Theme.of(
            context,
          ).textTheme.titleLarge?.copyWith(color: Colors.white),
        ),
        backgroundColor: AppTheme.backgroundColorHeader,
      ),
      body: Container(
        padding: EdgeInsets.all(16),
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
                    padding: EdgeInsets.only(left: 16, right: 16, top: 16),
                    child: Image.asset(
                      'assets/paymentMethods.png',
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
                    onCreditCardModelChange: onCreditCardModelChange,
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
                    child: Button(textButton: "оплатить", onPressed: null),
                  ),
                  Expanded(
                    child: Button(
                      textButton: "отмена",
                      onPressed: () {
                        Navigator.pop(context);
                      },
                    ),
                  ),
                ],
              ),
            ),
          ],
        ),
      ),
    );
  }

  void onCreditCardModelChange(CreditCardModel creditCardModel) {
    setState(() {
      cardNumber = creditCardModel.cardNumber;
      expiryDate = creditCardModel.expiryDate;
      cardHolderName = creditCardModel.cardHolderName;
      cvvCode = creditCardModel.cvvCode;
    });
  }
}
