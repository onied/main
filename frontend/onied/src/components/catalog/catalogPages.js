const plainCourse = {
    courseId: 1,
    href: "https://intelligentemployment.com/wp-content/uploads/2021/12/AdobeStock_387227165-scaled.jpeg",
    courseTitle: "Создание голограмм на ноутбуке",
    category: "Категория",
    courseAuthor: "Автор курса",
    coursePrice: 7777,
    isHighlighted: false
};
const freeCourse = {
    courseId: 1,
    href: "https://intelligentemployment.com/wp-content/uploads/2021/12/AdobeStock_387227165-scaled.jpeg",
    courseTitle: "Создание голограмм на ноутбуке(демо)",
    category: "Категория",
    courseAuthor: "Автор курса",
    coursePrice: 0,
    isHighlighted: false
};
const highlightedCourse = {
    courseId: 1,
    href: "https://intelligentemployment.com/wp-content/uploads/2021/12/AdobeStock_387227165-scaled.jpeg",
    courseTitle: "Создание голограмм на ноутбуке(deluxe edition)",
    category: "Категория",
    courseAuthor: "Автор курса",
    coursePrice: 77777,
    isHighlighted: true
};

const allCourses = [
    ...Array.from({ length: 10 }, () => ({ ...highlightedCourse })),
    ...Array.from({ length: 10 }, () => ({ ...freeCourse })),
    ...Array.from({ length: 15 }, () => ({ ...plainCourse })),
    ...Array.from({ length: 10 }, () => ({ ...highlightedCourse })),
    ...Array.from({ length: 10 }, () => ({ ...freeCourse })),
    ...Array.from({ length: 50 }, () => ({ ...plainCourse })),
]

export function getPage(pageIndex){
    return allCourses.slice(20 * (pageIndex - 1), 20 * pageIndex);
}

export function getAmountOfPages(){
    return Math.ceil(allCourses.length / 20);
}