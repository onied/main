import React, { ChangeEvent, useEffect, useState } from "react";
import api from "../../../config/axios";
import { useParams } from "react-router-dom";
import classes from "../editPreview/editPreview.module.css";
import Button from "../../general/button/button";
import InputForm from "../../general/inputform/inputform";
import InputFormArea from "../../general/inputform/inputFormArea";

type PreviewDto = {
  title: string;
  pictureHref: string;
  description: string;
  hoursCount: number;
  price: number;
  category: {
    id: number;
    name: string;
  };
  isArchived: boolean;
  hasCertificates: boolean;
  isContentProgramVisible: boolean;
};

type Errors = {
  Title: string | null;
  Description: string | null;
  Category: string | null;
  Price: string | null;
  CompleteTime: string | null;
  Picture: string | null;
};

function EditPreviewComponent() {
  const { courseId } = useParams();
  const [previewInfo, setPreview] = useState<PreviewDto | undefined>();
  const [errors, setErrors] = useState<Errors>({
    Title: null,
    Description: null,
    Category: null,
    Price: null,
    CompleteTime: null,
    Picture: null,
  });
  const [found, setFound] = useState<boolean | undefined>();
  const notFound = <h1 style={{ margin: "3rem" }}>Курс не найден.</h1>;

  const id = Number(courseId);
  if (isNaN(id)) return notFound;

  const checkPreview = () => {};

  useEffect(() => {
    setFound(undefined);
    api
      .get("courses/" + id)
      .then((response) => {
        console.log(response.data);
        setFound(true);
        setPreview(response.data);
      })
      .catch((error) => {
        console.log(error);

        if ("response" in error && error.response.status == 404) {
          setFound(false);
        }
      });
  }, [courseId]);

  if (!found) return notFound;

  return (
    <>
      <div className={classes.firstBlock}>
        <form>
          <div className={classes.inputChanges}>
            <div className={classes.inputChangesOneColumn}>
              <label className={classes.formLabel} htmlFor="preview_title">
                Название
              </label>
              <div>
                <InputForm
                  id="preview_title"
                  value={previewInfo?.title}
                  onChange={(e: ChangeEvent<HTMLInputElement>) =>
                    setPreview({ ...previewInfo!, title: e.target.value })
                  }
                  style={{ position: "static" }}
                ></InputForm>
                {errors.Title ? <div>{errors.Title}</div> : <></>}
              </div>
              <label
                className={classes.formLabel}
                htmlFor="preview_description"
              >
                Описание
              </label>
              <div className={classes.fixedHeightTextArea}>
                <InputFormArea
                  id="preview_description"
                  type="area"
                  value={previewInfo?.description}
                  onChange={(e: ChangeEvent<HTMLInputElement>) =>
                    setPreview({ ...previewInfo!, description: e.target.value })
                  }
                ></InputFormArea>
                {errors.Description ? <div>{errors.Description}</div> : <></>}
              </div>
              <label className={classes.formLabel} htmlFor="preview_category">
                Категория
              </label>
              <div>
                <InputForm
                  id="preview_category"
                  value={previewInfo?.category.name}
                  onChange={(e: ChangeEvent<HTMLInputElement>) =>
                    setPreview({
                      ...previewInfo!,
                      category: {
                        ...previewInfo!.category,
                        name: e.target.value,
                      },
                    })
                  }
                ></InputForm>
                {errors.Category ? <div>{errors.Category}</div> : <></>}
              </div>
            </div>
          </div>
        </form>
        <div className={classes.previewPrompt}>
          <span>
            Превью страница отображает основную информацию о вашем курсе
          </span>
          <Button
            style={{ width: "50%", margin: "25px 0" }}
            onClick={checkPreview}
          >
            посмотреть
          </Button>
        </div>
      </div>
      <div className={classes.line}></div>
    </>
  );
}

export default EditPreviewComponent;
