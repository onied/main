import { useState, useEffect, ChangeEvent } from "react";
import Checkbox from "../../general/checkbox/checkbox";
import InputForm from "../../general/inputform/inputform";
import Select from "../../general/inputform/select";
import classes from "./catalogFilter.module.css";
import api from "../../../config/axios";
import { useSearchParams } from "react-router-dom";

type CategoryDto = {
  id: number;
  name: string;
};

function CatalogFilter() {
  const [searchParams, setSearchParams] = useSearchParams();
  const [categories, setCategories] = useState<
    Array<CategoryDto> | undefined
  >();
  const [categoryId, setCategoryId] = useState<number | undefined>();
  const [priceFrom, setPriceFrom] = useState<number | undefined>();
  const [priceTo, setPriceTo] = useState<number | undefined>();
  const [timeFrom, setTimeFrom] = useState<number | undefined>();
  const [timeTo, setTimeTo] = useState<number | undefined>();
  const [certificatesOnly, setCertificatesOnly] = useState<boolean>(false);
  const [activeOnly, setActiveOnly] = useState<boolean>(false);

  const setNumberOrUndefined = (
    event: ChangeEvent<HTMLInputElement>,
    setValue: (value: number | undefined) => void
  ) => {
    const number = Number(event.target.value);
    if (event.target.value.length == 0) setValue(undefined);
    else setValue(number);
  };

  useEffect(() => {
    const category = searchParams.get("category");
    const priceFrom = searchParams.get("priceFrom");
    const priceTo = searchParams.get("priceTo");
    const timeFrom = searchParams.get("timeFrom");
    const timeTo = searchParams.get("timeTo");
    const certificatesOnly = searchParams.get("certificatesOnly");
    const activeOnly = searchParams.get("activeOnly");

    const setNumberIfExists = (
      param: string | null,
      setValue: (value: number | undefined) => void
    ) => {
      if (param == null) setValue(undefined);
      else setValue(Number(param));
    };

    const setBooleanIfExists = (
      param: string | null,
      setValue: (value: boolean) => void
    ) => {
      if (param == null) setValue(false);
      else setValue(param.toLocaleLowerCase() == "true");
    };

    setNumberIfExists(category, setCategoryId);
    setNumberIfExists(priceFrom, setPriceFrom);
    setNumberIfExists(priceTo, setPriceTo);
    setNumberIfExists(timeFrom, setTimeFrom);
    setNumberIfExists(timeTo, setTimeTo);
    setBooleanIfExists(certificatesOnly, setCertificatesOnly);
    setBooleanIfExists(activeOnly, setActiveOnly);
  }, [searchParams]);

  useEffect(() => {
    console.log("here");
    if (categoryId !== undefined)
      searchParams.set("category", categoryId.toString());
    else searchParams.delete("category");
    if (priceFrom !== undefined)
      searchParams.set("priceFrom", priceFrom.toString());
    else searchParams.delete("priceFrom");
    if (priceTo !== undefined) searchParams.set("priceTo", priceTo.toString());
    else searchParams.delete("priceTo");
    if (timeFrom !== undefined)
      searchParams.set("timeFrom", timeFrom.toString());
    else searchParams.delete("timeFrom");
    if (timeTo !== undefined) searchParams.set("timeTo", timeTo.toString());
    else searchParams.delete("timeTo");
    if (certificatesOnly) searchParams.set("certificatesOnly", "true");
    else searchParams.delete("certificatesOnly");
    if (activeOnly) searchParams.set("activeOnly", "true");
    else searchParams.delete("activeOnly");
    setSearchParams(searchParams);
  }, [
    categoryId,
    priceFrom,
    priceTo,
    timeFrom,
    timeTo,
    certificatesOnly,
    activeOnly,
  ]);

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
              event.target.value !== "any"
                ? Number(event.target.value)
                : undefined
            )
          }
        >
          <option value="any">Любая категория</option>
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
