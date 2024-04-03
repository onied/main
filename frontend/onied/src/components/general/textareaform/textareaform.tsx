import classes from "./textareaform.module.css";

function TextAreaForm(props) {
  return (
    <textarea
      type="text"
      className={classes.textareafrom}
      {...props}
    ></textarea>
  );
}

export default TextAreaForm;
