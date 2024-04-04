import { useState } from "react";

import classes from "./index.module.css";
import { SingleAnswerTask, Variant } from "../../../../types/task";
import Radio from "../../../general/radio/radio";
import InputForm from "../../../general/inputform/inputform";
import Button from "../../../general/button/button";
import TrashButton from "../../../general/trashButton";

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

  const updateVariantInput = (id: number, description: string) => {
    const newVariants = task.variants;
    newVariants!.find((v) => v.id == id)!.description = description;
    onChange("variants", newVariants);
  };

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
              value={variant.description}
              onChange={(event) =>
                updateVariantInput(variant.id, event.target.value)
              }
            />
            <TrashButton
              onClick={(event) => {
                event.preventDefault();
                removeVariant(variant.id);
              }}
            />
          </div>
        );
      })}
      <Button onClick={addVariant} style={{ width: "fit-content" }}>
        добавить пункт
      </Button>
    </div>
  );
}

export default SingleAnswerTaskExtension;
