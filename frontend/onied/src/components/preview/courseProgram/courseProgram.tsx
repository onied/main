import { ReactNode } from "react";

import classes from './courseProgram.module.css'

type CourseProgramProps = {
    modules: Array<string>
};

function CourseProgram(props: CourseProgramProps): ReactNode {
    return (
        <>
            <h2 className={classes.courseTitle}>программа курса</h2>
            <ul className={classes.courseProgram}>
                {props.modules.map(moduleTitle => <li>{moduleTitle}</li>)}
            </ul>
        </>
    );
}

export default CourseProgram;