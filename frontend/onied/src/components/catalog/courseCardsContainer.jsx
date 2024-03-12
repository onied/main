import {useEffect, useState} from "react";
import GeneralCourseCard from "./courseCards/generalCourseCard.jsx";
import classes from "./courseCardsContainer.module.css"
import {getPage} from "./catalogPages.js";

function CourseCardsContainer({ currentPage }){
    const [coursesList, setCoursesList] = useState([]);

    useEffect(() => {
        setCoursesList(getPage(currentPage));
    }, [currentPage]);

    return(
        <div className={classes.courseCardsContainer}>
            {coursesList.map((courseInfo, index) => <GeneralCourseCard card={courseInfo} key={index}/>)}
        </div>
    )
}

export default CourseCardsContainer;