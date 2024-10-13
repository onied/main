import { PropsWithChildren } from "react";
import classes from "./blocks.module.css";

export default function BlockViewContainer({ children }: PropsWithChildren) {
  return <div className={classes.blockViewContainer}>{children}</div>;
}
