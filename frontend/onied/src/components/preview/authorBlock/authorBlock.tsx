import classes from "./authorBlock.module.css";

type AuthorBlockProps = {
  authorName: string;
  authorAvatarHref: string;
};

function AuthorBlock(props: AuthorBlockProps) {
  return (
    <div className={classes.authorBlock}>
      <div className={classes.authorAvatarContainer}>
        <img className={classes.authorAvatar} src={props.authorAvatarHref} />
      </div>
      <span className={classes.authorName}>{props.authorName}</span>
    </div>
  );
}

export default AuthorBlock;
