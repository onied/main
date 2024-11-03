import classes from "./summary.module.css";

type props = {
  fileName: string;
  fileHref: string;
};

function FileLink({ fileName, fileHref }: props) {
  return (
    <>
      <div
        className={classes.fileLink}
        data-testid="markdown-summary-file-link"
      >
        <svg
          width="25"
          height="32"
          viewBox="0 0 25 32"
          fill="none"
          xmlns="http://www.w3.org/2000/svg"
        >
          <path
            d="M0.5 4C0.5 1.8125 2.25 0 4.5 0H14.8125C15.875 0 16.875 0.4375 
            17.625 1.1875L23.3125 6.875C24.0625 7.625 24.5 8.625 24.5 9.6875V28C24.5
            30.25 22.6875 32 20.5 32H4.5C2.25 32 0.5 30.25 0.5 28V4ZM21.5 28V10H16.5C15.375
            10 14.5 9.125 14.5 8V3H4.5C3.9375 3 3.5 3.5 3.5 4V28C3.5 28.5625 3.9375 29 4.5
            29H20.5C21 29 21.5 28.5625 21.5 28Z"
            fill="#9715D3"
          />
        </svg>
        <a href={fileHref}>{fileName}</a>
      </div>
    </>
  );
}

export default FileLink;
