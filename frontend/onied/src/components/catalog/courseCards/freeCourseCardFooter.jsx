import Button from "../../general/button/button.jsx";
import classes from "./courseCard.module.css"

function FreeCourseCardFooter(){
    return(
        <div className={[classes.courseCardFooter, classes.freeCourse].join(' ')}>
            <span>Бесплатно</span>
            <Button style={ { fontSize: '15px'} }>начать</Button>
        </div>
    )
}

export default FreeCourseCardFooter;