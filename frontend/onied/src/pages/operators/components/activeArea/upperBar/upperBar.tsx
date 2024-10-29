import { Chat } from "@onied/types/chat";
import classes from "./upperBar.module.css";

import IdBar from "@onied/components/general/idBar/idBar";
import { useAppDispatch } from "@onied/hooks";
import { ChatsStateActionTypes } from "@onied/redux/reducers/chatReducer";

const Return = () => {
  const dispatch = useAppDispatch();

  const clickEvent = () => {
    dispatch({ type: ChatsStateActionTypes.FETCH_CURRENT_CHAT });
  };

  return (
    <div className={classes.buttonSvg} onClick={clickEvent}>
      <svg
        width="20"
        height="20"
        viewBox="0 0 20 20"
        fill="none"
        xmlns="http://www.w3.org/2000/svg"
      >
        <path
          d="M3.33334 8.33333L3.03871 8.628L2.74408 8.33333L3.03871 8.03871L3.33334 
            8.33333ZM17.0833 15C17.0833 15.2301 16.8968 15.4167 16.6667 15.4167C16.4366 
            15.4167 16.25 15.2301 16.25 15H17.0833ZM7.20538 12.7947L3.03871 8.628L3.62796 
            8.03871L7.79463 12.2053L7.20538 12.7947ZM3.03871 8.03871L7.20538 3.87204L7.79463 
            4.46129L3.62796 8.628L3.03871 8.03871ZM3.33334 7.91667H11.6667V8.75H3.33334V7.91667ZM17.0833 
            13.3333V15H16.25V13.3333H17.0833ZM11.6667 7.91667C14.6582 7.91667 17.0833 10.3417 
            17.0833 13.3333H16.25C16.25 10.802 14.198 8.75 11.6667 8.75V7.91667Z"
          fill="#9715D3"
        />
      </svg>
    </div>
  );
};

const Unlink = () => {
  const dispatch = useAppDispatch();

  const clickEvent = () => {
    dispatch({ type: ChatsStateActionTypes.FETCH_CURRENT_CHAT });
  };

  return (
    <div className={classes.buttonSvg} onClick={clickEvent}>
      <svg
        width="20"
        height="20"
        viewBox="0 0 20 20"
        fill="none"
        xmlns="http://www.w3.org/2000/svg"
      >
        <g clipPath="url(#clip0_1660_571)">
          <path
            d="M1.12062 8.8299C-0.372645 7.33664 -0.372645 4.90691 1.12062 3.41297L3.41266 
                1.1196C4.90525 -0.372997 7.33498 -0.372997 8.82959 1.1196L12.5534 4.84541L10.6638 
                6.73572L6.93928 3.00991C6.48675 2.55939 5.75549 2.55939 5.30364 3.00991L3.01093 
                5.30328C2.55974 5.7538 2.55974 6.48707 3.01093 6.93826L6.73607 10.6641L4.84643 
                12.553L1.12062 8.8299ZM18.8794 11.1721L15.1536 7.44626L13.2633 9.33657L16.9884 
                13.0624C17.4396 13.5136 17.4396 14.2462 16.9884 14.6974L14.6957 16.9894C14.2425 
                17.4406 13.5112 17.4392 13.0601 16.9894L9.33492 13.2649L7.44461 15.1552L11.1704 
                18.8797C11.9164 19.6257 12.8976 20 13.8796 20C14.8601 20 15.8414 19.6257 16.588 
                18.8797L18.8807 16.5877C20.3727 15.0951 20.3727 12.6647 18.8794 11.1721ZM18.3493 
                2.51795L17.4042 1.57279L14.8829 4.09343L15.828 5.03859L18.3493 2.51795ZM15.9818 
                6.48038L19.4235 7.40482L19.7711 6.11208L16.328 5.18898L15.9818 6.48038ZM14.7298 
                3.59278L13.806 0.149045L12.5146 0.496627L13.4377 3.93969L14.7298 3.59278ZM1.72822 
                17.2481L2.67404 18.1932L5.19468 15.6726L4.24953 14.7281L1.72822 17.2481ZM5.34708 
                16.1699L6.27085 19.6136L7.56225 19.2674L6.63915 15.8237L5.34708 16.1699ZM4.09445 
                13.2823L0.651385 12.3579L0.30514 13.6506L3.74754 14.5737L4.09445 13.2823Z"
            fill="#9715D3"
          />
        </g>
        <defs>
          <clipPath id="clip0_1660_571">
            <rect width="20" height="20" fill="white" />
          </clipPath>
        </defs>
      </svg>
    </div>
  );
};

const Finish = () => {
  const dispatch = useAppDispatch();

  const clickEvent = () => {
    dispatch({ type: ChatsStateActionTypes.FETCH_CURRENT_CHAT });
  };

  return (
    <div className={classes.buttonSvg} onClick={clickEvent}>
      <svg
        width="20"
        height="20"
        viewBox="0 0 20 20"
        fill="none"
        xmlns="http://www.w3.org/2000/svg"
      >
        <g clipPath="url(#clip0_1660_575)">
          <path
            d="M19.375 0.625C19.0298 0.625 18.75 0.904844 18.75 1.25V1.875H1.25V1.25C1.25 
                0.904844 0.970156 0.625 0.625 0.625C0.279844 0.625 0 0.904844 0 1.25V2.5V10V18.75C0 
                19.0952 0.279844 19.375 0.625 19.375C0.970156 19.375 1.25 19.0952 
                1.25 18.75V10.625H18.75V18.75C18.75 19.0952 19.0298 19.375 19.375 19.375C19.7202 19.375 
                20 19.0952 20 18.75V10V2.5V1.25C20 0.904844 19.7202 0.625 19.375 0.625ZM16.25 
                9.375V6.25H13.125V9.375H10V6.25H6.875V9.375H3.75V6.25H1.25V3.125H3.75V6.25H6.875V3.125H10V6.25H13.125V3.125H16.25V6.25H18.75V9.375H16.25Z"
            fill="#9715D3"
          />
          <path d="M13.125 6.25H10V9.375H13.125V6.25Z" fill="white" />
          <path d="M3.75 3.125H1.25V6.25H3.75V3.125Z" fill="white" />
          <path d="M6.875 6.25H3.75V9.375H6.875V6.25Z" fill="white" />
          <path d="M18.75 6.25H16.25V9.375H18.75V6.25Z" fill="white" />
          <path d="M16.25 3.125H13.125V6.25H16.25V3.125Z" fill="white" />
          <path d="M10 3.125H6.875V6.25H10V3.125Z" fill="white" />
        </g>
        <defs>
          <clipPath id="clip0_1660_575">
            <rect width="20" height="20" fill="white" />
          </clipPath>
        </defs>
      </svg>
    </div>
  );
};

export default function UpperBar({ currentChat }: { currentChat: Chat }) {
  return (
    <div className={classes.upperBar}>
      <Return />
      <IdBar id={currentChat.currentSessionId!} />
      <Unlink />
      <Finish />
    </div>
  );
}
