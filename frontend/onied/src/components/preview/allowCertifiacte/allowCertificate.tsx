import { ReactNode, memo } from "react";
import classes from "./allowCertificate.module.css";

function AllowCertificate(): ReactNode {
  return (
    <div className={classes.allowCertificate}>
      <svg
        width="50"
        height="50"
        viewBox="0 0 30 30"
        fill="none"
        xmlns="http://www.w3.org/2000/svg"
      >
        <path
          d="M11.875 15L13.9583 17.0833L18.125 12.9167M15 5.625L16.9897 7.57445L19.6875 
                6.88101L20.4358 9.56411L23.119 10.3125L22.4255 13.0103L24.375 15L22.4255 16.9897L23.119
                19.6875L20.4358 20.4358L19.6875 23.119L16.9897 22.4255L15 24.375L13.0103 22.4255L10.3125
                23.119L9.56411 20.4358L6.88101 19.6875L7.57445 16.9897L5.625 15L7.57445 13.0103L6.88101
                10.3125L9.56411 9.56411L10.3125 6.88101L13.0103 7.57445L15 5.625Z"
          stroke="#686868"
          stroke-width="1.6"
          stroke-linecap="round"
          stroke-linejoin="round"
        />
      </svg>
      <span>курс выдает сертификаты</span>
    </div>
  );
}

const Memo = memo(AllowCertificate);
export { Memo as AllowCertificate };
