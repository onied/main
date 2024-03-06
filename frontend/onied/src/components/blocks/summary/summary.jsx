import { useState } from "react";
import Markdown from 'react-markdown';
import classes from './summary.module.css';
import { Fragment } from 'react';
import FileLink from "./fileLink";

function SummaryDto(title, markdownText, fileName, fileHref) {
    this.title = title;
    this.markdownText = markdownText;
    this
    this.file = {
        name: fileName,
        href: fileHref,
    };
}

function Summary() {

    const [summary, setSummary] = useState(new SummaryDto(
        "Титульник", 
        "Lorem ipsum dolor sit amet, consectetur adipiscing elit, sed do eiusmod tempor incididunt ut labore et dolore magna aliqua. Egestas dui id ornare arcu. Nunc id cursus metus aliquam eleifend mi in nulla posuere. Luctus venenatis lectus magna fringilla urna porttitor. Lobortis elementum nibh tellus molestie. Curabitur gravida arcu ac tortor dignissim convallis aenean. Ut diam quam nulla porttitor massa. Vulputate ut pharetra sit amet aliquam id diam maecenas ultricies. Sagittis id consectetur purus ut faucibus pulvinar elementum integer. Malesuada bibendum arcu vitae elementum curabitur vitae nunc sed velit. Mattis nunc sed blandit libero volutpat sed. Urna neque viverra justo nec. Ullamcorper morbi tincidunt ornare massa. Bibendum est ultricies integer quis auctor elit sed vulputate. Scelerisque eu ultrices vitae auctor eu augue ut lectus.Lacus vel facilisis volutpat est velit. Vitae purus faucibus ornare suspendisse sed nisi lacus. Urna condimentum mattis pellentesque id nibh tortor id. Urna cursus eget nunc scelerisque. Massa id neque aliquam vestibulum morbi. Neque vitae tempus quam pellentesque nec nam aliquam sem et. Mauris pellentesque pulvinar pellentesque habitant morbi. Feugiat in ante metus dictum at. Consequat id porta nibh venenatis cras. Massa massa ultricies mi quis hendrerit dolor. Varius duis at consectetur lorem donec massa sapien faucibus et. Vestibulum sed arcu non odio euismod lacinia at quis risus. \n Molestie ac feugiat sed lectus vestibulum mattis. In tellus integer feugiat scelerisque varius morbi. Neque ornare aenean euismod elementum. Egestas erat imperdiet sed euismod nisi.",
        "file_name.svg",
        "/assets/react.svg"
    ));

    return (<Fragment>
        <div className={classes.summaryContainer} >
            <h2>{summary.title}</h2>
            <Markdown className={classes.markdownText}>{summary.markdownText}</Markdown>
            { 
                summary.file.href == null 
                    ? null 
                    : <FileLink fileName={summary.file.name} fileHref={summary.file.href} />
            }
        </div>
    </Fragment>);
}


export default Summary;