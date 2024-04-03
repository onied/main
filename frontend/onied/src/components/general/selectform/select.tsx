import classes from "./select.module.css";
import { MenuItem, Select } from "@mui/material";

type Option = {
  label: string;
  value: any;
};

function SelectForm({
  id,
  options,
  onChange,
}: {
  id: any;
  options: Option[];
  onChange: (option: Option) => void;
}) {
  return (
    <Select className={classes.select} id={id}>
      {options.map((opt) => (
        <MenuItem key={opt.value} value={opt.value}>
          {opt.label}
        </MenuItem>
      ))}
    </Select>
  );
}

export default SelectForm;
