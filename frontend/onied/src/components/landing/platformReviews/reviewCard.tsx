import classes from "./reviews.module.css";
import Quotes from "../../../assets/quotes.svg";

function ReviewCard(props: { reviewCard: ReviewCardData }) {
  return (
    <div className={classes.reviewCard}>
      <div>
        <img src={Quotes} alt="quotes" />
        <p>{props.reviewCard.review}</p>
      </div>
      <div className={classes.reviewCardFooter}>
        <p>{props.reviewCard.author}</p>
        <span></span>
      </div>
    </div>
  );
}

export default ReviewCard;

export type ReviewCardData = {
  review: string;
  author: string;
};
