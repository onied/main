import Button from "../../general/button/button.jsx";
import classes from "./courseCard.module.css"
import {Link} from "react-router-dom";

function FreeCourseCardFooter({ courseId }){
    return(
        <div className={[classes.courseCardFooter, classes.freeCourse].join(' ')}>
            <span>Бесплатно</span>
            <Link to={"/course/" + courseId}>
                <Button style={ { fontSize: '15px'} }>начать</Button>
            </Link>
        </div>
    )
}

export default FreeCourseCardFooter;