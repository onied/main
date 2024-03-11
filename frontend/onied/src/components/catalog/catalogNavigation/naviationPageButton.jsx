import classes from "./catalogNavigation.module.css";

function NavigationPageButton({ pageNumber, isActive, onPageChange }){
    return(
        <div>
            <div className={[classes.navItem, isActive ? classes.activeNavItem : ''].join(' ')}
                 onClick={() => onPageChange(pageNumber)}>{pageNumber}</div>
        </div>
    )
}

export default NavigationPageButton;