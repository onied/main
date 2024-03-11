import classes from "./catalogNavigation.module.css";
import {getAmountOfPages} from "../catalogPages.js";
import NaviationPageButton from "./naviationPageButton.jsx";

function CatalogNavigation({ currentPage, onPageChange }){
    const maxPageAmount = getAmountOfPages();
    const pageNumbers = Array.from({length: maxPageAmount}, (_, i) => i + 1);

    return(
        <div className={classes.catalogNavigationContainer}>
            <div className={classes.navItem} onClick={() => onPageChange(currentPage - 1)}>{"<"}</div>
            <div className={classes.navItem} onClick={() => onPageChange(currentPage - 1)}>Назад</div>
            {currentPage - 2 > 1 ? (<div>...</div>) : null}
            {pageNumbers.map((pageNumber) => {
                if (pageNumber <= currentPage + 2 && pageNumber >= currentPage - 2){
                    return((<NaviationPageButton pageNumber={pageNumber}
                                                 isActive={pageNumber === currentPage}
                                                 onPageChange={onPageChange} />))
                }
            })}
            {currentPage + 2 < maxPageAmount ? (<div>...</div>) : null}
            <div className={classes.navItem} onClick={() => onPageChange(currentPage + 1)}>Вперёд</div>
            <div className={classes.navItem} onClick={() => onPageChange(currentPage + 1)}>{">"}</div>
        </div>
    )
}

export default CatalogNavigation;