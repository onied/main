import PaidCourseCardFooter from "./paidCourseCardFooter.jsx";
import FreeCourseCardFooter from "./freeCourseCardFooter.jsx";
import classes from "./courseCard.module.css";
import ContinueCourseCardFooter from "./continueCourseCardFooter.tsx";

function GeneralCourseCard({ card, owned }) {
  return (
    <div
      className={[
        classes.courseCard,
        card.isGlowing ? classes.highlightedCourseCard : "",
      ].join(" ")}
    >
      <img src={card.pictureHref} />
      <div className={classes.courseCardInfo}>
        <h3>{card.title}</h3>
        <p to={"/catalog?category=" + card.category.id}>{card.category.name}</p>
        <h4>{card.author.name}</h4>
      </div>
      {owned ? (
        <ContinueCourseCardFooter courseId={card.id} />
      ) : card.price > 0 ? (
        <PaidCourseCardFooter price={card.price} courseId={card.id} />
      ) : (
        <FreeCourseCardFooter courseId={card.id} />
      )}
    </div>
  );
}

export default GeneralCourseCard;
