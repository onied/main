import { memo } from "react";
import classes from "./sidebar.module.css";

const CompletedIcon = (props) => (
  <div className={classes.completedIcon}>
    <svg
      preserveAspectRatio="none"
      viewBox="0 0 25 25"
      fill="none"
      xmlns="http://www.w3.org/2000/svg"
      {...props}
    >
      <circle cx={12.5} cy={12.5} r={12.5} fill="#9715D3" />
      <path
        d="M13.5 17.5L18.1344 7.49998"
        stroke="white"
        strokeWidth={4}
        strokeLinecap="round"
      />
      <path
        d="M6.5 14L13.5 17.5"
        stroke="white"
        strokeWidth={4}
        strokeLinecap="round"
      />
    </svg>
  </div>
);
const Memo = memo(CompletedIcon);
export { Memo as CompletedIcon };
