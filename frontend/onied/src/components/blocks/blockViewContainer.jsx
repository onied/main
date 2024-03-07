import classes from "../../components/blocks/blocks.module.css"

export default function blockViewContainer({ children }) {
    return (<div className={classes.blockViewContainer}>{ children }</div>);
}