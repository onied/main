import classes from "./select.module.css";
import { MenuItem, Select, SelectChangeEvent } from "@mui/material";

type Option = {
  label: string;
  value: number;
};

function SelectForm({
  id,
  options,
  value,
  onChange,
  disabled,
}: {
  id: any;
  options: Option[];
  value: number;
  onChange: (event: SelectChangeEvent) => void;
  disabled?: boolean;
}) {
  return (
    <Select
      className={classes.select}
      id={id}
      value={value.toString()}
      disabled={disabled}
      onChange={onChange}
    >
      {options.map((opt) => (
        <MenuItem key={opt.value} value={opt.value}>
          {opt.label}
        </MenuItem>
      ))}
    </Select>
  );
}

export default SelectForm;
