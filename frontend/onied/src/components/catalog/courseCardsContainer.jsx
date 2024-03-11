import {useState} from "react";
import GeneralCourseCard from "./courseCards/generalCourseCard.jsx";
import classes from "./courseCardsContainer.module.css"

function CourseCardsContainer(){
    const plainCourse = {
        href: "https://intelligentemployment.com/wp-content/uploads/2021/12/AdobeStock_387227165-scaled.jpeg",
        courseTitle: "Создание голограмм на ноутбуке",
        category: "Категория",
        courseAuthor: "Автор курса",
        coursePrice: 7777,
        isHighlighted: false
    };
    const freeCourse = {
        href: "https://intelligentemployment.com/wp-content/uploads/2021/12/AdobeStock_387227165-scaled.jpeg",
        courseTitle: "Создание голограмм на ноутбуке(демо)",
        category: "Категория",
        courseAuthor: "Автор курса",
        coursePrice: 0,
        isHighlighted: false
    };
    const highlightedCourse = {
        href: "https://intelligentemployment.com/wp-content/uploads/2021/12/AdobeStock_387227165-scaled.jpeg",
        courseTitle: "Создание голограмм на ноутбуке(deluxe edition)",
        category: "Категория",
        courseAuthor: "Автор курса",
        coursePrice: 77777,
        isHighlighted: true
    };
    const [coursesList, setCoursesList] = useState([
        ...Array.from({ length: 5 }, () => ({ ...highlightedCourse })),
        ...Array.from({ length: 10 }, () => ({ ...freeCourse })),
        ...Array.from({ length: 5 }, () => ({ ...plainCourse })),
    ]);

    return(
        <div className={classes.courseCardsContainer}>
            {coursesList.map((courseInfo, index) => <GeneralCourseCard card={courseInfo} key={index}/>)}
        </div>
    )
}

export default CourseCardsContainer;