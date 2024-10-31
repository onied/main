import classes from "./confirmEmail.module.css";
import ConfirmEmailComponent from "../../../components/accountManagement/confirmEmail/confirmEmail";

function ConfirmEmail() {
  return (
    <div className={classes.container}>
      <ConfirmEmailComponent></ConfirmEmailComponent>
    </div>
  );
}

export default ConfirmEmail;
