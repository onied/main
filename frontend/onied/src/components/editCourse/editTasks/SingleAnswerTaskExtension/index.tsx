import { useState } from "react";

import classes from "./index.module.css";
import { SingleAnswerTask, Variant } from "../../../../types/task";
import Radio from "../../../general/radio/radio";
import InputForm from "../../../general/inputform/inputform";
import Button from "../../../general/button/button";

function SingleAnswerTaskExtension({
  task,
  onChange,
}: {
  task: SingleAnswerTask;
  onChange: (attr: string, value: any) => void;
}) {
  const [rightVariant, setRightVariant] = useState<number>();

  const handleChange = (event: { target: { value: any } }) =>
    setRightVariant(event.target.value);

  const updateVariantInput = (id: number, value: string) => {};

  const addVariant = (event) => {
    event.preventDefault();
    const newId = task.variants![task.variants!.length - 1].id + 1;
    onChange("variants", task.variants!.concat({ id: newId, description: "" }));
  };

  const removeVariant = (variantId: number) => {
    onChange(
      "variants",
      task.variants!.filter((v) => v.id != variantId)
    );
  };

  return (
    <div className={classes.variantsContainer}>
      {task.variants?.map((variant) => {
        return (
          <div key={variant.id} className={classes.variant}>
            <Radio
              name={task.id}
              id={variant.id}
              value={variant.id}
              onChange={handleChange}
              checked={rightVariant == variant.id ? "t" : ""}
            ></Radio>
            <InputForm
              style={{ width: "100%" }}
              onChange={(event) =>
                updateVariantInput(variant.id, event.target.value)
              }
            />
            <Button
              onClick={(event) => {
                event.preventDefault();
                removeVariant(variant.id);
              }}
            >
              rm
            </Button>
            ;
          </div>
        );
      })}
      <Button onClick={addVariant}>добавить пункт</Button>
    </div>
  );
}

export default SingleAnswerTaskExtension;
