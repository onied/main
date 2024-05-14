import { useEffect, useState } from "react";
import InputForm from "../../general/inputform/inputform";
import PaymentMethodsLogo from "../paymentMethods";
import classes from "./cardContainer.module.css";
import { CardInfo } from "../../../types/purchase";

function CardContainer({
  onChange,
}: {
  onChange: (card: CardInfo | null) => void;
}) {
  const [cardNumber, setCardNumber] = useState<string>();
  const [cardHolder, setCardHolder] = useState<string>();

  const [month, setMonth] = useState<number>();
  const [year, setYear] = useState<number>();

  const [securityCode, setSecurityCode] = useState<string>();

  const customSet = (
    value: string,
    setter: (value: any) => void,
    validationRegex: RegExp,
    _default: any | null = null
  ) => {
    setter(value.match(validationRegex) ? value : _default);
  };

  const customSetCardNumber = (event: any) => {
    const value = event.target.value;
    const validation = /^\d{0,19}$/;
    customSet(value, setCardNumber, validation, cardNumber);
  };

  const customSetCardHolder = (event: any) => {
    const value = event.target.value.toUpperCase();
    const validation = /^[A-Z\s\-]+$/;
    customSet(value, setCardHolder, validation, cardHolder);
  };

  const customSetMonth = (event: any) => {
    const value = event.target.value;
    const validation = /^\d{1,2}$/;
    customSet(
      value,
      (value: string) => {
        const avaliableMonth = Math.max(0, Math.min(12, Number(value)));
        setMonth(avaliableMonth);
      },
      validation,
      month
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
      validation,
      year
    );
  };

  const customSetSecurityCode = (event: any) => {
    const value = event.target.value;
    const validation = /^\d{1,3}$/;
    customSet(
      value,
      (value: string) => setSecurityCode(value),
      validation,
      securityCode
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

    const card: CardInfo | null =
      [cardNumber, cardHolder, month, year, securityCode].some(
        (value: any) => value == null
      ) ||
      (cardNumber != null && (cardNumber.length > 19 || cardNumber.length < 13))
        ? null
        : {
            number: cardNumber!,
            holder: cardHolder!,
            month: month!,
            year: year!,
            securityCode: securityCode!,
          };
    onChange(card);
  }, [cardNumber, cardHolder, month, year, securityCode]);

  return (
    <div className={classes.cardContainer}>
      <PaymentMethodsLogo className={classes.paymentMethods} />

      <InputForm
        className={classes.customNumberInput}
        style={{ width: "100%" }}
        type="number"
        placeholder="Номер карты"
        value={cardNumber}
        onChange={customSetCardNumber}
        required
      />

      <InputForm
        style={{ width: "100%", textTransform: "uppercase" }}
        placeholder="Держатель карты"
        value={cardHolder}
        onChange={customSetCardHolder}
        required
      />

      <div className={classes.cardFooter}>
        <div className={classes.monthAndYear}>
          <InputForm
            type="number"
            className={classes.customNumberInput}
            placeholder="мм"
            value={month}
            onInput={(event: any) => {
              if (event.target.value.length > 2)
                event.target.value = event.target.value.slice(1);
            }}
            onChange={customSetMonth}
            required
          />
          <span>/</span>
          <InputForm
            type="number"
            className={classes.customNumberInput}
            placeholder="гг"
            value={year}
            onInput={(event: any) => {
              if (event.target.value.length > 2)
                event.target.value = event.target.value.slice(1);
            }}
            onChange={customSetYear}
            required
          />
        </div>

        <div className={classes.securityCode}>
          <InputForm
            type="number"
            className={classes.customNumberInput}
            placeholder="CVC/CVV/CVP"
            value={securityCode}
            onInput={(event: any) => {
              if (event.target.value.length > 3)
                event.target.value = event.target.value.slice(1);
            }}
            onChange={customSetSecurityCode}
            required
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
