import classes from "../../components/blocks/blocks.module.css"

export default function BlockViewContainer({ children }) {
    return (<div className={classes.blockViewContainer}>{ children }</div>);
}