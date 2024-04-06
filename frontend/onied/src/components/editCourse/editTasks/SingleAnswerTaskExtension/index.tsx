import classes from "./index.module.css";
import { SingleAnswerTask } from "../../../../types/task";
import Radio from "../../../general/radio/radio";
import InputForm from "../../../general/inputform/inputform";
import Button from "../../../general/button/button";
import TrashButton from "../../../general/trashButton";

function SingleAnswerTaskExtension({
  id,
  task,
  onChange,
}: {
  id: string;
  task: SingleAnswerTask;
  onChange: (attr: string, value: any) => void;
}) {
  const setRightVariant = (event: any) => {
    const currentVariantId = Number(event.target.value);
    const updatedVariants = task.variants.map((variant) => {
      if (variant.id === currentVariantId) {
        return { ...variant, isCorrect: true };
      }
      return { ...variant, isCorrect: false };
    });
    onChange("variants", updatedVariants);
  };

  const updateVariantInput = (id: number, description: string) => {
    const newVariants = task.variants;
    newVariants!.find((v) => v.id == id)!.description = description;
    onChange("variants", newVariants);
  };

  const addVariant = (event: any) => {
    event.preventDefault();
    const newId =
      task.variants.length == 0
        ? 1
        : task.variants[task.variants.length - 1].id + 1;
    onChange(
      "variants",
      task.variants!.concat({
        id: newId,
        description: "",
        isNew: true,
        isCorrect: false,
      })
    );
  };

  const removeVariant = (variantId: number) => {
    const newTaskVariants = task.variants.filter((v) => v.id != variantId);
    onChange("variants", newTaskVariants);
    if (newTaskVariants.every((v) => !v.isCorrect))
      onChange(
        "variants",
        newTaskVariants.map((v, i) => {
          if (i == 0) {
            return { ...v, isCorrect: true };
          }
          return v;
        })
      );
  };

  return (
    <div id={id} className={classes.variantsContainer}>
      {task.variants?.map((variant) => {
        return (
          <div key={variant.id} className={classes.variant}>
            <Radio
              name={task.id}
              id={variant.id}
              value={variant.id}
              onChange={setRightVariant}
              checked={variant.isCorrect ? "t" : ""}
            ></Radio>
            <InputForm
              style={{ width: "100%" }}
              value={variant.description}
              onChange={(event: any) =>
                updateVariantInput(variant.id, event.target.value)
              }
            />
            {task.variants.length > 2 && (
              <TrashButton
                onClick={(event: any) => {
                  event.preventDefault();
                  removeVariant(variant.id);
                }}
              />
            )}
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
