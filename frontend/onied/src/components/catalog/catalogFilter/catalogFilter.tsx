import { useState, useEffect, ChangeEvent } from "react";
import Checkbox from "../../general/checkbox/checkbox";
import InputForm from "../../general/inputform/inputform";
import Select from "../../general/inputform/select";
import classes from "./catalogFilter.module.css";
import api from "../../../config/axios";

type CategoryDto = {
  id: number;
  name: string;
};

function CatalogFilter() {
  const [categories, setCategories] = useState<
    Array<CategoryDto> | undefined
  >();
  const [categoryId, setCategoryId] = useState<number | undefined>();
  const [priceFrom, setPriceFrom] = useState<number | undefined>();
  const [priceTo, setPriceTo] = useState<number | undefined>();
  const [timeFrom, setTimeFrom] = useState<number | undefined>();
  const [timeTo, setTimeTo] = useState<number | undefined>();
  const [certificatesOnly, setCertificatesOnly] = useState<boolean>(true);
  const [activeOnly, setActiveOnly] = useState<boolean>(true);

  const setNumberOrUndefined = (
    event: ChangeEvent<HTMLInputElement>,
    setValue: (value: number | undefined) => void
  ) => {
    const number = Number(event.target.value);
    if (event.target.value.length == 0) setValue(undefined);
    else setValue(number);
  };

  useEffect(() => {
    api
      .get("categories")
      .then((response) => {
        console.log(response.data);
        setCategories(response.data);
      })
      .catch((error) => {
        console.log(error);
      });
  }, []);
  return (
    <div className={classes.container}>
      <div className={classes.filterContainer}>
        <h3 className={classes.title}>Фильтрация</h3>
        <label className={classes.label} htmlFor="category">
          Категория
        </label>
        <Select
          id="category"
          value={categoryId}
          onChange={(event: ChangeEvent<HTMLInputElement>) =>
            setCategoryId(
              event.target.value ? Number(event.target.value) : undefined
            )
          }
        >
          <option value={undefined}>Любая категория</option>
          {categories?.map((categoryDto) => (
            <option key={categoryDto.id} value={categoryDto.id}>
              {categoryDto.name}
            </option>
          )) ?? <></>}
        </Select>
        <label className={classes.label} htmlFor="price">
          Цена
        </label>
        <div className={classes.interval} id="price">
          <InputForm
            placeholder="От"
            type="number"
            className={classes.intervalInput}
            value={priceFrom ?? ""}
            onChange={(e: any) => setNumberOrUndefined(e, setPriceFrom)}
          ></InputForm>
          <span className={classes.label}>—</span>
          <InputForm
            placeholder="До"
            type="number"
            className={classes.intervalInput}
            value={priceTo ?? ""}
            onChange={(e: any) => setNumberOrUndefined(e, setPriceTo)}
          ></InputForm>
          <span className={classes.intervalLabel}>₽</span>
        </div>
        <label className={classes.label} htmlFor="time">
          Время прохождения
        </label>
        <div className={classes.interval} id="time">
          <InputForm
            placeholder="От"
            type="number"
            className={classes.intervalInput}
            value={timeFrom ?? ""}
            onChange={(e: any) => setNumberOrUndefined(e, setTimeFrom)}
          ></InputForm>
          <span className={classes.label}>—</span>
          <InputForm
            placeholder="До"
            type="number"
            className={classes.intervalInput}
            value={timeTo ?? ""}
            onChange={(e: any) => setNumberOrUndefined(e, setTimeTo)}
          ></InputForm>
          <span className={classes.intervalLabel}>ч.</span>
        </div>
        <div className={classes.checkboxContainer}>
          <label className={classes.label} htmlFor="certificatesOnly">
            Только с сертификатом
          </label>
          <Checkbox
            id="certificatesOnly"
            variant="formal"
            checked={certificatesOnly}
            onChange={(e: ChangeEvent<HTMLInputElement>) => {
              setCertificatesOnly(e.target.checked);
            }}
          ></Checkbox>
        </div>
        <div className={classes.checkboxContainer}>
          <label className={classes.label} htmlFor="activeOnly">
            Только активные
          </label>
          <Checkbox
            id="activeOnly"
            variant="formal"
            checked={activeOnly}
            onChange={(e: ChangeEvent<HTMLInputElement>) => {
              setActiveOnly(e.target.checked);
            }}
          ></Checkbox>
        </div>
      </div>
    </div>
  );
}

export default CatalogFilter;
