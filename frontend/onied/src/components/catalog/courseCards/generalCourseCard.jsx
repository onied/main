import PaidCourseCardFooter from "./paidCourseCardFooter.jsx";
import FreeCourseCardFooter from "./freeCourseCardFooter.jsx";
import classes from "./courseCard.module.css";


function GeneralCourseCard({ card }){


    return(
        <div className={[classes.courseCard,
                         card.isHighlighted ? classes.highlightedCourseCard : ''].join(' ')}>
            <img src={card.href}/>
            <div className={classes.courseCardInfo}>
                <h3>{card.courseTitle}</h3>
                <p>{card.category}</p>
                <h4>{card.courseAuthor}</h4>
            </div>
            {card.coursePrice > 0
                ? (<PaidCourseCardFooter price={card.coursePrice} courseId={card.courseId}/>)
                : (<FreeCourseCardFooter courseId={card.courseId}/>)}
        </div>
    )
}

export default GeneralCourseCard;