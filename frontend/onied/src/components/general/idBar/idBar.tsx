import classes from "./idBar.module.css";

import { UUID } from "crypto";

const IdBadge = () => <span className={classes.idBadge}>ID</span>;

export default function IdBar({ id }: { id: UUID }) {
  return (
    <div className={classes.idBar}>
      <IdBadge />
      <p>{id}</p>
    </div>
  );
}
