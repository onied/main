import classes from "./customArrow.module.css";

function CustomArrow(props: { onClick: () => void; next: boolean }) {
  return (
    <div
      className={`${classes.customArrow} ${props.next ? classes.right : classes.left}`}
      onClick={() => props.onClick()}
    >
      <span className={classes.arrow}></span>
    </div>
  );
}

export default CustomArrow;
