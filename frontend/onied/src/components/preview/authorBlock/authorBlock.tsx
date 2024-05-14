import Avatar from "react-avatar";
import classes from "./authorBlock.module.css";

type AuthorBlockProps = {
  authorName: string;
  authorAvatarHref: string;
};

function AuthorBlock(props: AuthorBlockProps) {
  return (
    <div className={classes.authorBlock}>
      <Avatar
        name={props.authorName}
        size="50"
        className={classes.authorAvatarContainer}
        src={props.authorAvatarHref ? props.authorAvatarHref : undefined}
      ></Avatar>
      <span className={classes.authorName}>{props.authorName}</span>
    </div>
  );
}

export default AuthorBlock;
