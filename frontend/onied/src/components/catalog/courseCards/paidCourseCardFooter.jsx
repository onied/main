import Button from "../../general/button/button.jsx";
import classes from "./courseCard.module.css";
import {numberWithSpaces} from "../utils.js";

function PaidCourseCardFooter({ price }){
    return(
        <div className={classes.courseCardFooter}>
            <span>{numberWithSpaces(price)} ₽</span>
            <Button style={ { fontSize: '15px' } }>купить</Button>
        </div>
    )
}

export default PaidCourseCardFooter;