import { useEffect, useState } from "react";
import InputForm from "../../general/inputform/inputform";
import PaymentMethodsLogo from "../paymentMethods";
import classes from "./cardContainer.module.css";
import { CardInfo } from "../../../types/purchases";

function CardContainer() {
  const [cardNumber, setCardNumber] = useState<string>();
  const [cardHolder, setCardHolder] = useState<string>();

  const [month, setMonth] = useState<number>();
  const [year, setYear] = useState<number>();

  const [securityCode, setSecurityCode] = useState<number>();

  const customSet = (
    value: string,
    setter: (value: any) => void,
    validationRegex: RegExp
  ) => {
    setter(value.match(validationRegex) ? value : null);
  };

  const customSetCardNumer = (event: any) => {
    const value = event.target.value;
    const validation = /^\d{13,19}$/;
    customSet(value, setCardNumber, validation);
  };

  const customSetCardHolder = (event: any) => {
    const value = event.target.value.toUpperCase();
    const validation = /^[A-Z\s]+$/;
    customSet(value, setCardHolder, validation);
  };

  const customSetMonth = (event: any) => {
    const value = event.target.value;
    const validation = /^\d{2}$/;
    customSet(
      value,
      (value: string) => {
        const avaliableMonth = Math.max(0, Math.min(12, Number(value)));
        setMonth(avaliableMonth);
      },
      validation
    );
  };

  const customSetYear = (event: any) => {
    const value = event.target.value;
    const validation = /^\d{1,2}$/;
    customSet(
      value,
      (value: string) => {
        const avaliableYear = Math.max(0, Math.min(99, Number(value)));
        setYear(avaliableYear);
      },
      validation
    );
  };

  const customSetSecurityCode = (event: any) => {
    const value = event.target.value;
    const validation = /^\d{1,3}$/;
    customSet(
      value,
      (value: string) => setSecurityCode(Number(value)),
      validation
    );
  };

  useEffect(() => {
    console.log({
      number: cardNumber,
      holder: cardHolder,
      month: month,
      year: year,
      securityCode: securityCode,
    });

    if (
      [cardNumber, cardHolder, month, year, securityCode].some(
        (value: any) => value == null
      )
    )
      return;

    const card: CardInfo = {
      number: cardNumber!,
      holder: cardHolder!,
      month: month!,
      year: year!,
      securityCode: securityCode!,
    };
    console.log("!");
  });

  return (
    <div className={classes.cardContainer}>
      <PaymentMethodsLogo className={classes.paymentMethods} />

      <InputForm
        style={{ width: "100%" }}
        type="number"
        placeholder="Номер карты"
        value={cardNumber}
        onChange={customSetCardNumer}
      />

      <InputForm
        style={{ width: "100%", textTransform: "uppercase" }}
        placeholder="Держатель карты"
        value={cardHolder}
        onChange={customSetCardHolder}
      />

      <div className={classes.cardFooter}>
        <div className={classes.monthAndYear}>
          <InputForm
            type="number"
            placeholder="мм"
            value={month}
            onInput={(event: any) => {
              if (event.target.value.length > 2)
                event.target.value = event.target.value.slice(1);
            }}
            onChange={customSetMonth}
          />
          <span>/</span>
          <InputForm
            type="number"
            placeholder="гг"
            value={year}
            onInput={(event: any) => {
              if (event.target.value.length > 2)
                event.target.value = event.target.value.slice(1);
            }}
            onChange={customSetYear}
          />
        </div>

        <div className={classes.securityCode}>
          <InputForm
            type="number"
            placeholder="CVC/CVV/CVP"
            value={securityCode}
            onInput={(event: any) => {
              if (event.target.value.length > 3)
                event.target.value = event.target.value.slice(1);
            }}
            onChange={customSetSecurityCode}
          />
          <span style={{ textWrap: "wrap" }}>
            три цифры с оборотной стороны
          </span>
        </div>
      </div>
    </div>
  );
}

export default CardContainer;
