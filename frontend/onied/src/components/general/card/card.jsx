import classes from "./card.module.css";

function Card(props) {
    return (
        <>
            <div className={classes.card} {...props}></div>
        </>
    );
}

export default Card;
