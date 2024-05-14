import { useSearchParams } from "react-router-dom";
import classes from "./catalogHeader.module.css";
import { useEffect, useState } from "react";

function CatalogHeader() {
  const [searchParams, setSearchParams] = useSearchParams();
  const [sort, setSort] = useState();
  useEffect(() => {
    const sort = searchParams.get("sort");
    if (sort == null) setSort(undefined);
    else setSort(sort);
  }, [searchParams]);
  useEffect(() => {
    if (sort != undefined) {
      searchParams.set("sort", sort);
      setSearchParams(searchParams);
    }
  }, [sort]);
  return (
    <div className={classes.catalogHeader}>
      <h2>Онлайн курсы</h2>
      <div className={classes.sort}>
        <label className={classes.label}>Сортировка: </label>
        <select
          className={classes.sortOrder}
          value={sort ?? "popular"}
          onChange={(e) => setSort(e.target.value)}
        >
          <option value="popular">Сначала популярные</option>
          <option value="new">Сначала новые</option>
          <option value="priceAsc">Цена (по возрастанию)</option>
          <option value="priceDesc">Цена (по убыванию)</option>
        </select>
      </div>
    </div>
  );
}

export default CatalogHeader;
